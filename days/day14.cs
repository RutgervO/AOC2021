using AOC.util;

namespace AOC.days;

internal class Day14 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "14_test01.txt"), 1588);
        AddRun("Part 1", () => RunPart(1, "14.txt"));
        AddRun("Test 2", () => RunPart(2, "14_test01.txt"), 2188189693529);
        AddRun("Part 2", () => RunPart(2, "14.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);

        var rules = lines
            .Skip(2)
            .Select(x => x.Split(" -> "))
            .ToDictionary(x => new Tuple<char, char>(x[0][0], x[0][1]), x => x[1][0]);

        var countLetters = new DefaultDictionary<char, long>();
        var countPairs = new DefaultDictionary<Tuple<char,char>, long>();
        
        var first = lines[0][0];
        foreach (var second in lines[0][1..])
        {
            countLetters[first] += 1;
            countPairs[new Tuple<char, char>(first, second)] += 1;
            first = second;
        }
        countLetters[first] += 1;
        
        for (var step = 0; step < (part == 1 ? 10 : 40); step++)
        {
            var listOfPairs = countPairs.ToList();
            countPairs.Clear();
            foreach (var (pair, count) in listOfPairs)
            {
                if (rules.TryGetValue(pair, out var insert))
                {
                    countPairs[new Tuple<char, char>(pair.Item1, insert)] += count;
                    countPairs[new Tuple<char, char>(insert, pair.Item2)] += count;
                    countLetters[insert] += count;
                }
                else
                {
                    countPairs[pair] += count;
                }
            }
        }

        var counts = countLetters.Select(x => x.Value).OrderBy(x => x).ToList();
        return counts.Last() - counts.First();
    }
}
