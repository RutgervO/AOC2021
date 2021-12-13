namespace AOC.days;

internal class Day06 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "06_test01.txt"), 5934);
        AddRun("Part 1", () => RunPart(1, "06.txt"));
        AddRun("Test 2", () => RunPart(2, "06_test01.txt"), 26984457539);
        AddRun("Part 2", () => RunPart(2, "06.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var days = part == 1 ? 80 : 256;
        var inputNumbers = GetListOfIntegers(inputName);

        var counts = Enumerable.Repeat((long)0, 9).ToArray();
        foreach (var number in inputNumbers)
            counts[number] += 1;

        for (var day = 1; day <= days; day++)
            counts = new[]
            {
                counts[1], counts[2], counts[3], counts[4], counts[5], counts[6],
                counts[0] + counts[7], counts[8], counts[0]
            };

        return counts.Sum();
    }

}