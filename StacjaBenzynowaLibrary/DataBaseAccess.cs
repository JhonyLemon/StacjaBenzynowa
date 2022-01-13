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

        public static List<Dictionary<string, object>> GetImportedData(string statement, List<KeyValuePair<KeyValuePair<string, string>, string>> parameters)
        {
            List<Dictionary<string, object>> imported = new List<Dictionary<string, object>>();
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
                        Dictionary<string, object> map = new Dictionary<string, object>();
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

        public static int SetData(string statement, List<KeyValuePair<KeyValuePair<string, string>, string>> parameters)
        {
            int returnVal = 0;
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
                    returnVal=command.ExecuteNonQuery();
                    
                }
            }
            return returnVal;
        }
        public static int SetDataTransaction(List<string> statements, List<List<KeyValuePair<KeyValuePair<string, string>, string>>> parameters)
        {
            long firstRowInserted = 0;
            int returnVal = 0;
            using (connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statements.Count; i++)
                        {
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = statements[i];
                                command.CommandType = CommandType.Text;
                                foreach (KeyValuePair<KeyValuePair<string, string>, string> valuePair in parameters[i])
                                {
                                    if (valuePair.Key.Value == "@FIRSTINSERTEDROW")
                                    {
                                        command.Parameters.AddWithValue(valuePair.Key.Value, firstRowInserted);
                                    }
                                    else
                                        command.Parameters.AddWithValue(valuePair.Key.Value, valuePair.Value);
                                }
                                command.ExecuteNonQuery();
                                if (firstRowInserted == 0)
                                    firstRowInserted = connection.LastInsertRowId;
                            }
                        }

                        transaction.Commit();
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        returnVal = 1;
                    }
                }
            }
            return returnVal;
        }
    }
}
