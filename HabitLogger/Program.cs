using System.Globalization;
using HabitLogger;

HabitLoggerDatabase db = new HabitLoggerDatabase();

void userInput()
{
    var exit = false;
    while (!exit)
    {
        Console.WriteLine("Welcome to Habit Logger: ");
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
            default:
                exit = true;
                break;
        }
    }
}

void InsertHabit()
{
    DateOnly date = GetDateInput("Enter the date you want to log in the format dd/mm/yyyy: ");
    var quantity = GetNumberInput("Please insert habit measure of your choice in integer (no decimals allowed): ");
    var unit = GetUnitInput("Please insert the unit of your habit e.g litres, glasses e.t.c");
    
    db.InsertHabit(date, quantity, unit);
}

DateOnly GetDateInput(string message)
{
    Console.Clear();
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
    Console.Clear();
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
    Console.Clear();
    Console.WriteLine("--------------------------");
    Console.WriteLine(message);
    
    string unit = Console.ReadLine().Trim();

    return unit;
}

userInput();