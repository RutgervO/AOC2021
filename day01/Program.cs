var inputLines = File.ReadLines(@"../../../input1.txt").ToList();
var inputValues = inputLines.ConvertAll(int.Parse);
System.Console.WriteLine("{0} lines read.", inputValues.Count);

int CountLarger(IEnumerable<int> input)
{
    return input
        .Skip(1)
        .Zip(input, (curr, prev) => curr > prev)
        .Count(x => x == true);
}


Console.WriteLine("\nDay 01 Step 01");
  
System.Console.WriteLine("{0} values were larger than the previous value.", CountLarger(inputValues));  


Console.WriteLine("\nDay 01 Step 02");

var windows = inputValues
    .Skip(2)
    .Zip(inputValues.Skip(1), (one, two) => one + two)
    .Zip(inputValues, (oneTwo, three) => oneTwo + three);

System.Console.WriteLine("{0} windows were larger than the previous window.", CountLarger(windows));