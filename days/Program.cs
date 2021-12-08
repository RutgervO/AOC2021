// ReSharper disable once CheckNamespace
namespace aoc;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("AOC 2021 runner");

        var rootType = typeof(Day);
        var days = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.BaseType == rootType)
            .ToList();
        var dayNumbers = days.Select(d => int.Parse(d.Name[3..])).ToArray();
        foreach (var dayNumber in dayNumbers)
        {
            Console.WriteLine($"Running day {dayNumber}...");
            var classToRun = days.First(d => d.Name[3..].EndsWith(dayNumber.ToString()));
            var day = Activator.CreateInstance(classToRun) as Day;
            if (day == null) return;
            day.DayNumber = dayNumber;
            day.Run();
        }
    }
}
