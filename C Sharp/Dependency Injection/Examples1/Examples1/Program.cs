// See https://aka.ms/new-console-template for more information

public interface IDateTimeProvider
{
    public DateTime DateTimeNow { get; }
}

public class SystemDateProvider : IDateTimeProvider
{
    public DateTime DateTimeNow => DateTime.Now;
}

public class Greeter
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public Greeter(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public string CreateGreetMessage()
    {
        var dateTimeNow = _dateTimeProvider.DateTimeNow;
        switch (dateTimeNow.Hour)
        {
            case >= 5 and < 12:
                return "Good morning";
            case >= 12 and < 18:
                return "Good afternoon";
            default:
                return "Good evening";
        }
    }
}

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Greeter g = new Greeter(new SystemDateProvider());
            Console.WriteLine(g.CreateGreetMessage());
        }
    }
}
