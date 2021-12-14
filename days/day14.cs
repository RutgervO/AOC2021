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
        var startLetters = lines[0].ToList();
        var rules = lines.Skip(2)
            .Select(x => x.Split(" -> "))
            .ToDictionary(x => (x[0][0], x[0][1]), x => x[1][0]);

        var countLetters = new DefaultDictionary<char, long>(
            startLetters.GroupBy(x => x, _ => (long) 1, (x, y) => (K:x, V:y.Sum()))
                .ToDictionary(x => x.K, x => x.V));

        var countPairs = new DefaultDictionary<(char a, char b), long>(
            startLetters.Zip(startLetters.Skip(1), (a, b) => (T:(a, b), V:(long)1))
                .GroupBy(x => x.T, x => x.V, (t, v) => (K:t, V:v.Sum()))
                .ToDictionary(x => x.K, x => x.V));
        
        for (var step = 0; step < (part == 1 ? 10 : 40); step++)
        {
            var previousPairs = countPairs.ToList();
            countPairs.Clear();
            foreach (var (pair, count) in previousPairs)
            {
                if (rules.TryGetValue(pair, out var insert))
                {
                    countPairs[(pair.Item1, insert)] += count;
                    countPairs[(insert, pair.Item2)] += count;
                    countLetters[insert] += count;
                }
                else
                {
                    countPairs[pair] += count;
                }
            }
        }

        var counts = countLetters.Select(x => x.Value).ToList();
        return counts.Max() - counts.Min();
    }
}
