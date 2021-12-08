// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day08 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "08_test01.txt"), 26);
        AddRun("Part 1", () => RunPart(1, "08.txt"));
        AddRun("Test 2", () => RunPart(2, "08_test02.txt"), 5353);
        AddRun("Part 2", () => RunPart(2, "08.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputLines = GetListOfLines(inputName);
        if (part == 1)
        {
            int[] hits = {2, 3, 4, 7};
            return inputLines.Sum(line => line.Split('|')
                .Last()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Length)
                .Count(x => hits.Contains(x)));
        }
        
        // part2
        long result = 0;
        foreach (var line in inputLines)
        {
            var samples = line.Split('|')
                .First()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToHashSet())
                .ToLookup(x => x.Count, x => x);
                
            var digits = line.Split('|').Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToHashSet());
            var sets = new HashSet<char>[10];
            
            // easy ones
            sets[1] = samples[2].First();
            sets[7] = samples[3].First();
            sets[4] = samples[4].First();
            sets[8] = samples[7].First();
            
            // "9" + "4" == "9"
            // "0" + "1" == "0", "0" + "6" <> "6"
            sets[9] = samples[6].First(x => x.SetEquals(x.Union(sets[4])));
            sets[0] = samples[6].Where(x => !x.SetEquals(sets[9])).First(x => x.SetEquals(x.Union(sets[1])));
            sets[6] = samples[6].Where(x => !x.SetEquals(sets[9])).First(x => !x.SetEquals(sets[0]));
            
            // "3" + "1" == "3"
            // "5" + "9" == "9", "2" + "9" <> "9"
            sets[3] = samples[5].First(x => x.SetEquals(x.Union(sets[1])));
            sets[5] = samples[5].Where(x => !x.SetEquals(sets[3])).First(x => sets[9].SetEquals(x.Union(sets[9])));
            sets[2] = samples[5].Where(x => !x.SetEquals(sets[3])).First(x => !x.SetEquals(sets[5]));

            var index = sets.Select((x, pos) => new {value = x, number = pos})
                .ToDictionary(x => x.value, x => x.number, HashSet<char>.CreateSetComparer());
            result += digits.Aggregate<HashSet<char>?, long>(0, (current, digit) => 10 * current + index[digit]);
        }

        return result;
    }
}