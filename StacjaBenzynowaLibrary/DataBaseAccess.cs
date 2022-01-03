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

        public static List<Dictionary<string, string>> GetImportedData(string statement, List<KeyValuePair<KeyValuePair<string, string>, string>> parameters)
        {
            List<Dictionary<string, string>> imported = new List<Dictionary<string, string>>();
            using (connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText =statement;
                    command.CommandType = CommandType.Text;
                    foreach (KeyValuePair<KeyValuePair<string, string>, string> valuePair in parameters)
                        {
                            command.Parameters.AddWithValue(valuePair.Key.Value, valuePair.Value);
                        }
                    SQLiteDataReader r = command.ExecuteReader();
                    while (r.Read())
                    {
                        Dictionary<string, string> map = new Dictionary<string, string>();
                        foreach (KeyValuePair<KeyValuePair<string, string>, string> value in parameters)
                        {
                            try
                            {
                                map.Add(value.Key.Key, Convert.ToString(r[value.Key.Key]));
                            }
                            catch(Exception e)
                            {

                            }
                        }
                        imported.Add(map);
                    }
                }
            }
            return imported;
        }

        public static long SetData(string statement, List<KeyValuePair<KeyValuePair<string, string>, string>> parameters)
        {
            long rowID;
            using (connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = statement;
                    command.CommandType = CommandType.Text;
                    foreach (KeyValuePair<KeyValuePair<string, string>, string> valuePair in parameters)
                    {
                        command.Parameters.AddWithValue(valuePair.Key.Value, valuePair.Value);
                    }
                    command.ExecuteNonQuery();
                    rowID = connection.LastInsertRowId;
                }
            }
            return rowID;
        }
        public static long SetDataTransaction(string statement, List<KeyValuePair<KeyValuePair<string, string>, string>> parameters)
        {
            using (connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                        using (var command = connection.CreateCommand())
                        {

                            command.ExecuteNonQuery();
                        }

                    transaction.Commit();
                }
            }
            return 0;
        }
    }
}
