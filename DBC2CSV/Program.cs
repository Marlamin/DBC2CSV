using DBCD;
using DBCD.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DBC2CSV
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Expected argument: db2filename or db2folder");
                return;
            }

            // Force system culture to ensure that decimal separator is always a dot
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            var filesToExport = new List<string>();
            var hotfixFiles = new List<string>();

            foreach (var arg in args)
            {
                if (arg.EndsWith(".db2") || arg.EndsWith(".dbc"))
                {
                    if (!File.Exists(arg))
                    {
                        Console.WriteLine("Input DB2 file or folder could not be found: " + arg);
                        return;
                    }

                    filesToExport.Add(arg);
                }
                else if (arg.EndsWith(".bin"))
                {
                    hotfixFiles.Add(arg);
                }
                else
                {
                    FileAttributes attr = File.GetAttributes(arg);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        hotfixFiles.AddRange(Directory.EnumerateFiles(arg, "*.bin", SearchOption.TopDirectoryOnly).ToList());
                        filesToExport.AddRange(Directory.EnumerateFiles(arg, "*.db2", SearchOption.TopDirectoryOnly).ToList());
                        filesToExport.AddRange(Directory.EnumerateFiles(arg, "*.dbc", SearchOption.TopDirectoryOnly).ToList());
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized file: " + arg);
                    }
                }
            }

            var newLinesInStrings = true;

            var dbdProvider = new DBDProvider();

            // TODO: Somehow figure out how to filter on the right build for the supplied DB2s?
            List<HotfixReader> hotfixReaders = new List<HotfixReader>();

            if (hotfixFiles.Count > 0)
            {
                Console.WriteLine(".bin file(s) supplied, loading hotfixes..");
                hotfixReaders.Add(new HotfixReader(hotfixFiles[0]));

                for (var i = 1; i < hotfixFiles.Count; i++)
                {
                    hotfixReaders[0].CombineCaches(hotfixFiles[i]);
                }

                Console.WriteLine("Loaded hotfixes for build " + hotfixReaders[0].BuildId);
            }

            foreach (var fileToExport in filesToExport)
            {
                var dbcd = new DBCD.DBCD(new DBCProvider(Path.GetDirectoryName(fileToExport)), dbdProvider);

                var tableName = Path.GetFileNameWithoutExtension(fileToExport);
                Console.WriteLine("Exporting table " + tableName);

                try
                {
                    var storage = dbcd.Load(tableName);

                    if (hotfixFiles.Count > 0)
                    {
                        storage.ApplyingHotfixes(hotfixReaders[0]);
                    }

                    if (!storage.Values.Any())
                    {
                        throw new Exception("No rows found!");
                    }

                    var headerWritten = false;

                    using (var exportStream = new MemoryStream())
                    using (var exportWriter = new StreamWriter(exportStream))
                    {
                        foreach (DBCDRow item in storage.Values)
                        {
                            // Write CSV header
                            if (!headerWritten)
                            {
                                for (var j = 0; j < storage.AvailableColumns.Length; ++j)
                                {
                                    string fieldname = storage.AvailableColumns[j];
                                    var field = item[fieldname];

                                    var isEndOfRecord = j == storage.AvailableColumns.Length - 1;

                                    if (field is Array a)
                                    {
                                        for (var i = 0; i < a.Length; i++)
                                        {
                                            var isEndOfArray = a.Length - 1 == i;

                                            exportWriter.Write($"{fieldname}[{i}]");
                                            if (!isEndOfArray)
                                                exportWriter.Write(",");
                                        }
                                    }
                                    else
                                    {
                                        exportWriter.Write(fieldname);
                                    }

                                    if (!isEndOfRecord)
                                        exportWriter.Write(",");
                                }
                                headerWritten = true;
                                exportWriter.WriteLine();
                            }

                            for (var i = 0; i < storage.AvailableColumns.Length; ++i)
                            {
                                var field = item[storage.AvailableColumns[i]];

                                var isEndOfRecord = i == storage.AvailableColumns.Length - 1;

                                if (field is Array a)
                                {
                                    for (var j = 0; j < a.Length; j++)
                                    {
                                        var isEndOfArray = a.Length - 1 == j;
                                        exportWriter.Write(a.GetValue(j));

                                        if (!isEndOfArray)
                                            exportWriter.Write(",");
                                    }
                                }
                                else
                                {
                                    var value = field;
                                    if (value.GetType() == typeof(string))
                                        value = StringToCSVCell((string)value, newLinesInStrings);

                                    exportWriter.Write(value);
                                }

                                if (!isEndOfRecord)
                                    exportWriter.Write(",");
                            }

                            exportWriter.WriteLine();
                        }

                        exportWriter.Dispose();

                        File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(fileToExport), tableName + ".csv"), exportStream.ToArray());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to export DB2 " + tableName + ": " + e.Message);
                }
            }
        }

        public static string StringToCSVCell(string str, bool newLinesInStrings)
        {
            if (!newLinesInStrings)
            {
                str = str.Replace("\n", "").Replace("\r", "");
            }

            var mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                var sb = new StringBuilder();
                sb.Append("\"");
                foreach (var nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }
    }
}
