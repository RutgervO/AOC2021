// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day01 : Day
{
    protected override void SetSequence()
    {
        AddRun("Part 1", () => RunPart(1, "01.txt"));
        AddRun("Part 2", () => RunPart(2, "01.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputValues = GetListOfLinesAsInt(inputName);
        if (part == 1)
            return CountLarger(inputValues);
        
        return CountLarger(inputValues
            .Skip(2)
            .Zip(inputValues.Skip(1), (one, two) => one + two)
            .Zip(inputValues, (oneTwo, three) => oneTwo + three));
    }
    
    int CountLarger(IEnumerable<int> input)
    {
        return input
            .Skip(1)
            .Zip(input, (curr, prev) => curr > prev)
            .Count(x => x);
    }
}