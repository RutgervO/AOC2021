// ReSharper disable once CheckNamespace
namespace aoc;

abstract class Day
{
    public int DayNumber { get; set; }
    private List<(string Title, Func<long> Action, long? TestResult)> Sequence { get; set; }
    
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
            Out($"\nRunning day {DayNumber} {title}...");
            long? result = action();
            Out($"Result: {result}");
            if (testResult is null) continue;
            if (result == testResult)
            {
                Out("Output is CORRECT. PASS!");
            }
            else
            {
                Out($"Output is INCORRECT. Expected: {testResult}");
                return;
            }
        }
    }

    public void Out(string output)
    {
        Console.WriteLine(output);
    }

    protected List<string> GetListOfLines(string fileName)
    {
        var inputLines = File.ReadLines(@"../../../input/" + fileName).ToList();
        Console.WriteLine($"Input {fileName}: {inputLines.Count} line{(inputLines.Count != 1 ? "s" : "")} read.");
        return inputLines;
    }

    protected List<int> GetListOfIntegers(string fileName)
    {
        var inputLines = GetListOfLines(fileName);
        return inputLines[0].Split(',').ToList().ConvertAll(int.Parse);
    }
}

public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new() where TKey : notnull
{
    public new TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out var val)) return val;
            val = new TValue();
            Add(key, val);
            return val;
        }
        set => base[key] = value;
    }
}