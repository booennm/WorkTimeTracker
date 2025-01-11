using System;
using System.Data.SQLite;

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
                        Timestamp TEXT NOT NULL,
                        Duration TEXT NOT NULL
                    )";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool SaveWorkLog(string duration)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO WorkLog (Timestamp, Duration) VALUES (@timestamp, @duration)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@duration", duration);
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public static SQLiteDataReader GetWorkLogs()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            string selectQuery = "SELECT * FROM WorkLog";
            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                return command.ExecuteReader();
            }
        }
    }
}
