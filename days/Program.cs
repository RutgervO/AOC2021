﻿// ReSharper disable once CheckNamespace
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
            var classToRun = days.Single(d => d.Name[3..].EndsWith(dayNumber.ToString("D2")));
            if (Activator.CreateInstance(classToRun) is not Day day) return;
            day.DayNumber = dayNumber;
            day.Run();
        }
    }
}
