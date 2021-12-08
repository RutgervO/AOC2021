// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day07 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "07_test01.txt"), 37);
        AddRun("Part 1", () => RunPart(1, "07.txt"));
        AddRun("Test 2", () => RunPart(2, "07_test01.txt"), 168);
        AddRun("Part 2", () => RunPart(2, "07.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputNumbers = GetListOfIntegers(inputName);

        var counts = new DefaultDictionary<int, long>();
        var first = 0;
        var last = 0;
        foreach (var number in inputNumbers)
        {
            counts[number] += 1;
            last = Math.Max(last, number);
        }

        var cheapest = long.MaxValue;
        for (var target = first; target <= last; target++)
        {
            var cost = counts.Select(x => GetCost(target, x.Key, x.Value, part)).Sum();
            cheapest = Math.Min(cheapest, cost);
        }
        return cheapest;
    }

    private static long GetCost(int target, int pos, long number, int part)
    {
        var dist = Math.Abs(target - pos);
        if (part == 1)
            return dist * number;
        if (dist == 0)
            return 0;
        long result = 0;
        for (var i = 1; i <= dist; i++)
        {
            result += i * number;
        }
        return result;
    }

}