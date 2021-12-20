using System.Text.RegularExpressions;

namespace AOC.days;

internal class Day18 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "18_test01.txt"), 4140);
        AddRun("Part 1", () => RunPart(1, "18.txt"));
        AddRun("Test 2", () => RunPart(2, "18_test01.txt"), 3993);
        AddRun("Part 2", () => RunPart(2, "18.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        if (part == 1)
        {
            var sum = "";
            foreach (var line in lines)
            {
                if (sum.Length > 0)
                {
                    sum = $"[{sum},{line}]";
                    sum = Reduce(sum);
                }
                else
                {
                    sum = line;
                }

            }

            return Magnitude(sum);
        }

        long max = 0;
        foreach (var l1 in lines)
        {
            foreach (var l2 in lines)
            {
                max = Math.Max(max, Magnitude(Reduce($"[{l1},{l2}]")));
            }
        }

        return max;
    }

    private string Reduce(string sum)
    {
        while (true)
        {
            if (FindDeepPair(sum, out var pos, out var pair, out var length))
            {
                var leftSum = AddLeft(sum[0..pos], pair.Item1);
                var rightSum = AddRight(sum[(pos + length)..], pair.Item2);
                sum = $"{leftSum}0{rightSum}";
                continue;
            }
            if (FindTooLarge(sum, out pos, out var value, out length))
            {
                var leftValue = value / 2;
                var rightValue = value - leftValue;
                sum = $"{sum[0..pos]}[{leftValue},{rightValue}]{sum[(pos + length)..]}";
                continue;
            }
            return sum;
        }
    }

    private static long Magnitude(string sum)
    {
        while (GetPair(sum, out var pos, out var pair, out var length))
        {
            var leftSum = sum[0..pos];
            var rightSum = sum[(pos + length)..];
            sum = $"{leftSum}{3 * pair.Item1 + 2 * pair.Item2}{rightSum}";
        }
        return long.Parse(sum);
    }
    
    private static bool FindTooLarge(string sum, out int pos, out int value, out int length)
    {
        pos = -1;
        value = 0;
        length = 0;
        var match = Regex.Match(sum, @"\d\d+");
        if (!match.Success) return false;
        pos = match.Index;
        value = int.Parse(match.Value);
        length = match.Length;
        return true;
    }

    private bool FindDeepPair(string sum, out int pos, out (int,int) pair, out int pairLength)
    {
        pos = -1;
        pair = (-1, -1);
        pairLength = 0;
        var level = 0;
        for (var i = 0; i < sum.Length; i++)
        {
            var c = sum[i];
            switch (c)
            {
                case '[':
                    level += 1;
                    if (level >= 5)
                    {
                        if (GetPairAt(sum, i, out pair, out pairLength))
                        {
                            pos = i;
                            return true;
                        }
                    }
                    break;
                case ']':
                    level -= 1;
                    break;
            }
        }
        return false;
    }

    private static bool GetPairAt(string sum, int pos, out (int, int) pair, out int length)
    {
        pair = (-1, -1);
        length = 0;
        if (!char.IsDigit(sum[pos + 1])) return false;
        var match = Regex.Match(sum[pos..], @"\[(\d+),(\d+)\]");
        if (!match.Success || match.Index != 0) return false;
        length = match.Captures[0].Length;
        pair = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        return true;
    }
    
    private static bool GetPair(string sum, out int pos, out (int, int) pair, out int length)
    {
        pair = (-1, -1);
        length = 0;
        pos = -1;
        var match = Regex.Match(sum, @"\[(\d+),(\d+)\]");
        if (!match.Success) return false;
        length = match.Captures[0].Length;
        pair = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        pos = match.Index;
        return true;
    }

    private string AddLeft(string sum, int value)
    {
        var match = Regex.Match(sum, @"\d+", RegexOptions.RightToLeft);
        if (!match.Success) return sum;
        var newValue = int.Parse(match.Value) + value;
        return $"{sum[0..match.Index]}{newValue}{sum[(match.Index + match.Length)..]}";
    }
    
    private string AddRight(string sum, int value)
    {
        var match = Regex.Match(sum, @"\d+");
        if (!match.Success) return sum;
        var newValue = int.Parse(match.Value) + value;
        return $"{sum[0..match.Index]}{newValue}{sum[(match.Index + match.Length)..]}";
    }
}