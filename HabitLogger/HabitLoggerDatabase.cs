using Microsoft.Data.Sqlite;

namespace HabitLogger;

public class HabitLoggerDatabase
{
    private readonly string FileName = "habitLoggerDb.db";

    public HabitLoggerDatabase()
    {
        CreateHabitLoggerDB();
    }

    public void CloseHabitLoggerConnection()
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        connection.Close();
    }

    public void InsertHabit(DateOnly date, int quantity, string unit)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO habitLogger(date, quantity, unit) VALUES ('{date}', {quantity}, '{unit}')";
            command.ExecuteNonQuery();
            Console.WriteLine($"Record successfully inserted!");
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Record not inserted. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
        
    }
    
    private void CreateHabitLoggerDB()
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @" CREATE TABLE IF NOT EXISTS habitLogger (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                date TEXT NOT NULL,
                quantity INTEGER NOT NULL,
                unit TEXT )";
        command.ExecuteNonQuery();
        connection.Close();
    }
}