using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitLogger;

public class HabitLoggerDatabase
{
    private const string FileName = "habitLoggerDb.db";

    public HabitLoggerDatabase()
    {
        CreateHabitLoggerDB();
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

    public Habit GetHabit(int habitId)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        Habit habit = null;
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM habitLogger WHERE id = {habitId} LIMIT 1";
            
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var date = DateOnly.Parse(reader.GetString(1), new CultureInfo("en-US"), DateTimeStyles.None);
                    var quantity = reader.GetInt32(2);
                    var unit = reader.GetString(3);

                    habit = new Habit(id, date, quantity, unit);
                }
                Console.WriteLine("Habit retrieved!");
            }

            return habit;
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to retrieve habit record with {habitId}. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
        
        return habit;
    }
    
    public List<Habit> GetAllHabits()
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        var habits = new List<Habit>();
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM habitLogger";
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var date = DateOnly.Parse(reader.GetString(1), new CultureInfo("en-US"), DateTimeStyles.None);
                    var quantity = reader.GetInt32(2);
                    var unit = reader.GetString(3);
                    
                    habits.Add(new Habit(id, date, quantity, unit));
                }
                Console.WriteLine("All habits retrieved!");
            }

            return habits;
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to retrieve all habit records. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
        
        return habits;
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