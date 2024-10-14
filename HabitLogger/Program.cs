using System.Globalization;
using HabitLogger;

var db = new HabitLoggerDatabase();

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
            case "4":
                UpdateHabit();
                break;
            case "5":
                DeleteHabit();
                break;
            default:
                exit = true;
                break;
        }
    }
}

userInput();
return;

void InsertHabit()
{
    Console.Clear();
    var date = GetDateInput("Enter the date you want to log in the format dd/mm/yyyy: ");
    Console.Clear();
    
    var habitType = GetStringInput("Please insert the habit type e.g Running, Sleeping e.t.c");
    habitType = habitType.ToLower();
    Console.Clear();
    
    var quantity = GetNumberInput("Please insert habit measure of your choice in integer (no decimals allowed): ");
    Console.Clear();
    
    var unit = GetStringInput("Please insert the unit of your habit e.g minutes, kilometres e.t.c");
    unit = unit.ToLower();
    
    db.InsertHabit(date, quantity, unit, habitType);
    continueMenu();
}

void GetHabit()
{
    if (!HasHabitsRecord())
    {
        continueMenu();
        return;
    }
    
    GetHabits();
    var id = GetNumberInput("Enter the habit ID you wish to retrieve: ");
    var habit = db.GetHabit(id);

    if (habit == null)
    {
        Console.WriteLine("No habit found with that ID!");
        continueMenu();
        return;
    }
    
    Console.Clear();
    Console.WriteLine($"Habit Record");
    BuildTableHeader();
    BuildTableRows(habit);
    continueMenu();
}

void GetHabits()
{
    Console.Clear();
    var habits = db.GetAllHabits();
    if (!habits.Any())
    {
        Console.WriteLine("No habits found!");
        continueMenu();
        return;
    }
    
    Console.WriteLine("All habit records:");
    BuildTableHeader();
    
    foreach (var habit in habits)
    {
        BuildTableRows(habit);
    }

    continueMenu();
}

void UpdateHabit()
{
    if (!HasHabitsRecord())
    {
        continueMenu();
        return;
    }
    
    GetHabits();
    var id = GetNumberInput("Enter the habit ID you wish to update: ");
    Console.Clear();
    var date = GetDateInput("Enter the updated date in the format dd/mm/yyyy: ");
    Console.Clear();
    
    var habitType = GetStringInput("Enter the updated habit type e.g Running, Sleeping e.t.c");
    Console.Clear();
    
    var quantity = GetNumberInput("Enter the updated habit measure of your choice in integer (no decimals allowed): ");
    Console.Clear();
    var unit = GetStringInput("Enter the updated unit of your habit e.g litres, glasses e.t.c");
    db.UpdateHabit(id, date, quantity, unit, habitType);
    continueMenu();
}

void DeleteHabit()
{
    if (!HasHabitsRecord())
    {
        continueMenu();
        return;
    }
    
    GetHabits();
    var id = GetNumberInput("Enter the habit ID you wish to delete: ");
    db.DeleteHabit(id);
    continueMenu();
}

bool HasHabitsRecord()
{
    var count = db.CountHabits();
    if (count < 1)
    {
        Console.WriteLine("No habits found!");
        return false;
    }

    return true;
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

string GetStringInput(string message)
{
    Console.WriteLine("--------------------------");
    Console.WriteLine(message);
    
    var res = Console.ReadLine().Trim();

    return res;
}

void BuildTableHeader()
{
    Console.WriteLine("-----------------------------------------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("|" + "ID".PadLeft(15) + "|" + "Date".PadLeft(15) + "|" + "Type".PadLeft(15) + "|" + "Quantity".PadLeft(15) + "|" +
                      "Unit".PadLeft(15) + "|");
    Console.ResetColor();
    Console.WriteLine("-----------------------------------------------------------------------------------");
}

void BuildTableRows(Habit habit)
{
    Console.WriteLine($"|{habit.Id,15}|{habit.Date,15}|{habit.Type,15}|{habit.Quantity,15}|{habit.Unit,15}|");
    Console.WriteLine("-----------------------------------------------------------------------------------");
}

void continueMenu()
{
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadLine();
}