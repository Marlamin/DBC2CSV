using DBCD.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DBC2CSV
{
    public class DBCProvider : IDBCProvider
    {
        private string baseDir = "";
        
        public DBCProvider(string baseDir)
        {
            this.baseDir = baseDir;
        }

        public Stream StreamForTableName(string tableName, string build)
        {
            string filename = GetDBCFile(tableName);
            return new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        private string GetDBCFile(string tableName)
        {
            if (tableName.Contains("."))
                throw new Exception("Invalid DBC name!");

            // DB2
            if (File.Exists(Path.Combine(baseDir, tableName + ".db2")))
                return Path.Combine(baseDir, tableName + ".db2");

            if (File.Exists(tableName + ".db2"))
                return tableName + ".db2";

            // DBC
            if (File.Exists(Path.Combine(baseDir, tableName + ".dbc")))
                return Path.Combine(baseDir, tableName + ".dbc");

            if (File.Exists(tableName + ".dbc"))
                return tableName + ".dbc";

            throw new FileNotFoundException($"Unable to find {tableName}");
        }
    }
}
