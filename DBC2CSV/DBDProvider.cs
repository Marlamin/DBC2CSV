using DBCD.Providers;
using DBDefsLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DBC2CSV
{
    public class DBDProvider : IDBDProvider
    {
        private readonly DBDReader dbdReader;
        private Dictionary<string, (string FilePath, Structs.DBDefinition Definition)> definitionLookup;
        public DBDProvider()
        {
            dbdReader = new DBDReader();
            LoadDefinitions();
        }

        public int LoadDefinitions()
        {
            var definitionsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "definitions");
            Console.WriteLine("Reloading definitions from directory " + definitionsDir);

            // lookup needs both filepath and def for DBCD to work
            // also no longer case sensitive now
            var definitionFiles = Directory.EnumerateFiles(definitionsDir);
            definitionLookup = definitionFiles.ToDictionary(x => Path.GetFileNameWithoutExtension(x), x => (x, dbdReader.Read(x)), StringComparer.OrdinalIgnoreCase);

            Console.WriteLine("Loaded " + definitionLookup.Count + " definitions!");

            return definitionLookup.Count;
        }

        public Stream StreamForTableName(string tableName, string build = null)
        {
            tableName = Path.GetFileNameWithoutExtension(tableName);

            if (definitionLookup.TryGetValue(tableName, out var lookup))
                return new FileStream(lookup.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            throw new FileNotFoundException("Definition for " + tableName);
        }

        public bool TryGetDefinition(string tableName, out Structs.DBDefinition definition)
        {
            if (definitionLookup.TryGetValue(tableName, out var lookup))
            {
                definition = lookup.Definition;
                return true;
            }

            definition = default(Structs.DBDefinition);
            return false;
        }
    }
}
