namespace AOC.days;

internal abstract class Day
{
    public int DayNumber { get; set; }
    private List<(string Title, Func<long> Action, long? TestResult)> Sequence { get; }
    
    public abstract long RunPart(int part, string inputName);
    protected abstract void SetSequence();

    protected Day()
    {
        Sequence = new List<(string Title, Func<long> Action, long? TestResult)>();
        Initialize();
    }

    private void Initialize()
    {
        SetSequence();
    }

    protected void AddRun(string title, Func<long> action, long? testResult=null)
    {
        Sequence.Add((title, action, testResult));
    }

    public void Run()
    {
        foreach (var (title, action, testResult) in Sequence)
        {
            Out($"Day {DayNumber} {title}: ");
            long? result = action();
            Out($"{result} ");
            if (testResult is not null)
            {
                if (result == testResult)
                {
                    Out("✓");
                }
                else
                {
                    Out($"❌ Expected: {testResult}\n");
                    return;
                }
            }
            Out("\n");
        }
    }

    private static void Out(string output)
    {
        Console.Write(output);
    }

    protected static List<string> GetListOfLines(string fileName)
    {
        var inputLines = File.ReadLines($@"input/{fileName}").ToList();
        return inputLines;
    }

    protected static List<int> GetListOfIntegers(string fileName)
    {
        var inputLines = GetListOfLines(fileName);
        return inputLines[0].Split(',').ToList().ConvertAll(int.Parse);
    }
    
    protected static List<int> GetListOfLinesAsInt(string fileName)
    {
        var inputLines = GetListOfLines(fileName);
        return inputLines.ConvertAll(int.Parse);
    }
}