using System.Diagnostics;

Console.WriteLine("\nDay 06 Step 01 Test 01");
var result = RunPart("test_input06_1.txt", 80);
Console.WriteLine($"Test result: {result}");
Debug.Assert(result == 5934);

Console.WriteLine("\nDay 06 Step 01");
result = RunPart("input06.txt", 80);
Console.WriteLine($"Result: {result}");

Console.WriteLine("\nDay 06 Step 02 Test 01");
result = RunPart("test_input06_1.txt", 256);
Console.WriteLine($"Test result: {result}");
Debug.Assert(result == 26984457539);

Console.WriteLine("\nDay 06 Step 02");
result = RunPart("input06.txt", 256);
Console.WriteLine($"Result: {result}");

long RunPart(string inputName, int days)
{
    var inputLines = File.ReadLines(@"../../../" + inputName).ToList();
    Console.WriteLine("{0} lines read.", inputLines.Count);
    var inputNumbers = inputLines[0].Split(',').ToList().ConvertAll(int.Parse);

    var counts = Enumerable.Repeat((long)0, 9).ToArray();
    foreach (var number in inputNumbers)
        counts[number] += 1;

    for (var day = 1; day <= days; day++)
        counts = new long[] {
            counts[1], counts[2], counts[3], counts[4], counts[5], counts[6],
            counts[0] + counts[7], counts[8], counts[0]
        };

    return counts.Sum();
}

