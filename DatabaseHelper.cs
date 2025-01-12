using System;
using System.Data.SQLite;
using System.Text.Json;
using static WorkTimeTracker.Models.WorkLogModel;

namespace WorkTimeTracker.Helpers
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Data Source=worklog.db;Version=3;";

        static DatabaseHelper()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS WorkLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT NOT NULL,
                        Title TEXT NOT NULL,
                        Duration TEXT NOT NULL
                    )";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool SaveWorkLogToSQLite(string log)
        {
            try
            {
                var logJSON = JsonSerializer.Deserialize<WorkLog>(log);
                if(logJSON != null)
                {
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string insertQuery = "INSERT INTO WorkLog (Date, Title, Duration) VALUES (@date, @title, @duration)";
                        using (var command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@date", logJSON.Date.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@title", logJSON.Title);
                            command.Parameters.AddWithValue("@duration", logJSON.Duration);
                            command.ExecuteNonQuery();
                        }
                    }
                    return true;
                }else
                {
                    throw new JsonException("Could not deserialize new worklog");
                }
            }
            catch (JsonException jsonEx)
            { 
                Console.WriteLine($"JSON Error: {jsonEx.Message}");
                return false;
            }
            catch (SQLiteException sqlEx)
            {
                Console.WriteLine($"SQLite Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            
        }

        public static SQLiteDataReader GetWorkLogsFromSQLite()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            string selectQuery = "SELECT * FROM WorkLog";
            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                return command.ExecuteReader();
            }
        }

        public static void ClearWorkLogs()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM WorkLog";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void ClearWorklogTable(SQLiteConnection connection)
        {
            string dropTableQuery = "DROP TABLE IF EXISTS WorkLog";
            using (var command = new SQLiteCommand(dropTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
