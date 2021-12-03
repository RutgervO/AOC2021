using System.Diagnostics;

Console.WriteLine("\nDay 03 Step 01 Test 01");
var result = RunPart1("test_input_3_1.txt");
Console.WriteLine($"Test result: {result}");
Debug.Assert(result == 198);

Console.WriteLine("\nDay 03 Step 01");
result = RunPart1("input3.txt");
Console.WriteLine($"Result: {result}");

Console.WriteLine("\nDay 03 Step 02 Test 01");
result = RunPart2("test_input_3_1.txt");
Console.WriteLine($"Test result: {result}");
Debug.Assert(result == 230);

Console.WriteLine("\nDay 03 Step 02");
result = RunPart2("input3.txt");
Console.WriteLine($"Result: {result}");


int RunPart1(string inputName)
{
    var inputLines = File.ReadLines(@"../../../" + inputName).ToArray();
    Console.WriteLine("{0} lines read.", inputLines.Length);

    var gamma = 0;
    var epsilon = 0;
    foreach (var bits in inputLines.Transpose())
    {
        var winner = (bits.Count(x => x == '1') > bits.Count(x => x == '0')) ? 1 : 0;
        gamma = 2 * gamma + winner;
        epsilon = 2 * epsilon + 1 - winner;
    }
    return gamma * epsilon;
}

int RunPart2(string inputName)
{
    var inputLines = File.ReadLines(@"../../../" + inputName).ToList();
    Console.WriteLine("{0} lines read.", inputLines.Count);

    var ogr = Part2Value(true);
    var csr = Part2Value(false);

    return ogr * csr;

    int Part2Value(bool most)
    {
        var candidates = Enumerable.Range(0, inputLines.Count).ToList();

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

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> l)
    {
        return l.SelectMany(inner => inner.Select((item, index) => new { item, index }))
            .GroupBy(i => i.index, i => i.item)
            .Select(g => g.ToList());
    }
}