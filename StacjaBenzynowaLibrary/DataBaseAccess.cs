using System.Data.SQLite;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;

namespace StacjaBenzynowaLibrary
{
    public class DataBaseAccess
    {
        public static SQLiteConnection connection { get; private set; }

        public DataBaseAccess()
        {
            connection = new SQLiteConnection(LoadConnectionString());
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static List<Dictionary<string, string>> GetImportedData(string statement, List<KeyValuePair<string, string>> parameters, List<string> values)
        {
            List<Dictionary<string, string>> imported = new List<Dictionary<string, string>>();
            using (connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText =statement;
                    command.CommandType = CommandType.Text;
                        foreach(KeyValuePair<string,string> valuePair in parameters)
                        {
                            command.Parameters.AddWithValue(valuePair.Key, valuePair.Value);
                        }
                    SQLiteDataReader r = command.ExecuteReader();
                    while (r.Read())
                    {
                        Dictionary<string, string> map = new Dictionary<string, string>();
                        // imported.Add(Convert.ToString(r["IMIE"]));
                        foreach (string value in values)
                        {
                            map.Add(value, Convert.ToString(r[value]));
                        }
                        imported.Add(map);
                    }
                }
            }
            return imported;
        }

        public static int SetData(string statement, List<KeyValuePair<string, string>> parameters)
        {
            int i = 0;
            using (connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    command.CommandType = CommandType.Text;
                    foreach (KeyValuePair<string, string> valuePair in parameters)
                    {
                            command.Parameters.AddWithValue(valuePair.Key, valuePair.Value);
                    }
                    i = command.ExecuteNonQuery();
                }
            }
            return i;
        }
    }
}
