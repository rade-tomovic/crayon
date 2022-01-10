namespace CurrencyConverter.Application;

public class DateComparer : IEqualityComparer<DateTime>
{
    public bool Equals(DateTime x, DateTime y)
    {
        return x.Day == y.Day && x.Month == y.Month && x.Year == y.Year;
    }

    public int GetHashCode(DateTime obj)
    {
        return HashCode.Combine(obj.Day, obj.Month, obj.Year);
    }
}