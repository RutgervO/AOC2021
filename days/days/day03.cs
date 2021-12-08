// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day03 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "03_test01.txt"), 198);
        AddRun("Part 1", () => RunPart(1, "03.txt"));
        AddRun("Test 2", () => RunPart(2, "03_test01.txt"), 230);
        AddRun("Part 2", () => RunPart(2, "03.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputLines = GetListOfLines(inputName).ToArray();

        if (part == 1)
        {
            var gamma = 0;
            var epsilon = 0;
            foreach (var bits in inputLines.Transpose())
            {
                var enumerable = bits as char[] ?? bits.ToArray();
                var winner = (enumerable.Count(x => x == '1') > enumerable.Count(x => x == '0')) ? 1 : 0;
                gamma = 2 * gamma + winner;
                epsilon = 2 * epsilon + 1 - winner;
            }
            return gamma * epsilon;
        }
        
        return Part2Value(true) * Part2Value(false);
        
        int Part2Value(bool most)
        {
            var candidates = Enumerable.Range(0, inputLines.Length).ToList();

            var bit = 0;
            while (candidates.Count > 1)
            {
                var ones = new List<int>();
                var zeros = new List<int>();
                foreach (var candidate in candidates)
                    if (inputLines[candidate][bit] == '1')
                        ones.Add(candidate);
                    else
                        zeros.Add(candidate);
                candidates.Clear();
                candidates.AddRange((most == (ones.Count >= zeros.Count)) ? ones : zeros);
                bit++;
            }
            return Convert.ToInt32(inputLines[candidates[0]], 2);
        }
    }
}