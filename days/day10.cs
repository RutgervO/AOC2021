namespace AOC.days;

internal class Day10 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "10_test01.txt"), 26397);
        AddRun("Part 1", () => RunPart(1, "10.txt"));
        AddRun("Test 2", () => RunPart(2, "10_test01.txt"), 288957);
        AddRun("Part 2", () => RunPart(2, "10.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        var part1Values = new Dictionary<char, long>()
        {
            {')', 3},
            {']', 57},
            {'}', 1197},
            {'>', 25137},
        };
        long result = 0;
        var part2Results = new List<long>();
        
        foreach (var line in lines)
        {
            var error = FindError(1, line);
            if (error.Length > 0)
            {
                if (part != 1) continue;
                result += part1Values[error[0]];
                continue;
            }

            if (part != 2) continue;
            error = FindError(2, line);
            result = 0;
            foreach (var character in error)
            {
                result *= 5;
                result += " )]}>".IndexOf(character);
            }
            part2Results.Add(result);
        }

        if (part == 2)
            result = part2Results.OrderBy(x => x).Skip(part2Results.Count / 2).First();
        return result;
    }

    private readonly Dictionary<char, char> _opposites = new()
    {
        {'(', ')'},
        {'[', ']'},
        {'{', '}'},
        {'<', '>'},
    };

    private string FindError(int part, string line, string expect = "")
    {
        while (true)
        {
            if (line.Length == 0)
            {
                return part == 1 ? "" : expect;
            }
            var brace = line[0];
            if (!_opposites.ContainsKey(brace))
            {
                if (expect.Length == 0 || brace != expect[0]) return brace.ToString();
                line = line[1..];
                expect = expect[1..];
                continue;
            }

            line = line[1..];
            expect = _opposites[brace] + expect;
        }
    }
}