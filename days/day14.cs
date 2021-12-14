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

        var pairs = lines
            .Skip(2)
            .Select(x => x.Split(" -> "))
            .ToDictionary(x => new Tuple<char, char>(x[0][0], x[0][1]), x => x[1][0]);

        var polymer = new DefaultDictionary<Tuple<char,char>, long>();
        var first = lines[0][0];
        foreach (var current in lines[0][1..])
        {
            polymer[new Tuple<char, char>(first, current)] += 1;
            first = current;
        }
        
        for (var step = 0; step < (part == 1 ? 10 : 40); step++)
        {
            var newPolymer = new DefaultDictionary<Tuple<char, char>, long>();
            foreach (var (pair, count) in polymer)
            {
                if (!pairs.TryGetValue(pair, out var insert))
                {
                    newPolymer[pair] += count;
                }
                else
                {
                    newPolymer[new Tuple<char, char>(pair.Item1, insert)] += count;
                    newPolymer[new Tuple<char, char>(insert, pair.Item2)] += count;
                }
            }

            polymer.Clear();
            foreach (var (pair, count) in newPolymer)
            {
                polymer[pair] = count;
            }
        }

        var letters = new DefaultDictionary<char, long>();
        foreach (var ((item1, item2), count) in polymer)
        {
            letters[item1] += count;
            letters[item2] += count;
        }

        var counts = letters.Select(x => x.Value).OrderBy(x => x).ToList();
        return (counts.Last() - counts.First() + 1) / 2;
    }
}
