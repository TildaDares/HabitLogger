namespace HabitLogger;

public class Habit(int id, DateOnly date, int quantity, string unit)
{
    public int Id { get; } = id;
    public DateOnly Date { get; private set; } = date;
    public int Quantity { get; private set; } = quantity;
    public string Unit { get; private set; } = unit;
}