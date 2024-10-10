using System.Globalization;
using HabitLogger;

HabitLoggerDatabase db = new HabitLoggerDatabase();

void userInput()
{
    var exit = false;
    while (!exit)
    {
        Console.WriteLine("\nWelcome to Habit Logger: ");
        Console.WriteLine("Select an option:");
        Console.WriteLine("1. Insert a habit");
        Console.WriteLine("2. Get a habit");
        Console.WriteLine("3. Get all habits");
        Console.WriteLine("4. Update a habit");
        Console.WriteLine("5. Delete a habit");
        Console.WriteLine("0. Exit");
        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                InsertHabit();
                break;
            case "2":
                GetHabit();
                break;
            case "3":
                GetHabits();
                break;
            default:
                exit = true;
                break;
        }
    }
}

void InsertHabit()
{
    Console.Clear();
    DateOnly date = GetDateInput("Enter the date you want to log in the format dd/mm/yyyy: ");
    Console.Clear();
    
    var quantity = GetNumberInput("Please insert habit measure of your choice in integer (no decimals allowed): ");
    Console.Clear();
    var unit = GetUnitInput("Please insert the unit of your habit e.g litres, glasses e.t.c");
    
    db.InsertHabit(date, quantity, unit);
}

void GetHabit()
{
    GetHabits();
    var id = GetNumberInput("Enter the habit ID you wish to retrieve: ");
    var habit = db.GetHabit(id);

    if (habit == null)
    {
        Console.WriteLine("No habit found!");
        return;
    }
    Console.Clear();
    Console.WriteLine($"Habit Record");
    BuildTableHeader();
    Console.WriteLine($"|{habit.Id,15}|{habit.Date,15}|{habit.Quantity,15}|{habit.Unit,15}|");
    Console.WriteLine("-----------------------------------------------------------------");
}

void GetHabits()
{
    Console.Clear();
    var habits = db.GetAllHabits();
    if (habits == null)
    {
        Console.WriteLine("No habits found!");
        return;
    }
    
    Console.WriteLine("All habit records:");
    BuildTableHeader();
    
    foreach (var habit in habits)
    {
        Console.WriteLine($"|{habit.Id,15}|{habit.Date,15}|{habit.Quantity,15}|{habit.Unit,15}|");
        Console.WriteLine("-----------------------------------------------------------------");
    }
}

DateOnly GetDateInput(string message)
{
    Console.WriteLine("--------------------------");

    DateOnly dateOnly;
    var input = "";
    do
    {
        Console.WriteLine(message);
        input = Console.ReadLine().Trim();
    } while (!DateOnly.TryParseExact(input, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None,  out dateOnly));

    return dateOnly;
}

int GetNumberInput(string message)
{
    Console.WriteLine("--------------------------");

    int quantity;
    var input = "";
    do
    {
        Console.WriteLine(message);
        input = Console.ReadLine().Trim();
    } while (!int.TryParse(input, out quantity));

    return quantity;
}

string GetUnitInput(string message)
{
    Console.WriteLine("--------------------------");
    Console.WriteLine(message);
    
    string unit = Console.ReadLine().Trim();

    return unit;
}

void BuildTableHeader()
{
    Console.WriteLine("-----------------------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("|" + "ID".PadLeft(15) + "|" + "Date".PadLeft(15) + "|" + "Quantity".PadLeft(15) + "|" +
                      "Unit".PadLeft(15) + "|");
    Console.ResetColor();
    Console.WriteLine("-----------------------------------------------------------------");
}

userInput();