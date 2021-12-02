using System.Runtime.CompilerServices;

var inputLines = File.ReadLines(@"../../../input2.txt").ToList();
System.Console.WriteLine("{0} lines read.", inputLines.Count);
Console.WriteLine("\nDay 02 Step 01");

var position = new Position1();

foreach (var line in inputLines) {
    position.Move(line);
}

position.Report();

System.Console.WriteLine("Result from day 2 step 1: {0}", position.Result());  


Console.WriteLine("\nDay 02 Step 02");

var position2 = new Position2();

foreach (var line in inputLines) {
    position2.Move(line);
}

position2.Report();

System.Console.WriteLine("Result from day 2 step 2: {0}", position2.Result());  

internal class Position1
{
    private int horizontal;
    private int depth;

    private Dictionary<string, Tuple<int, int>> Directions = new Dictionary<string, Tuple<int, int>>()
    {
        {"forward", new Tuple<int, int>(1, 0)},
        {"up", new Tuple<int, int>(0, -1)},
        {"down", new Tuple<int, int>(0, 1)},
    };

    public Position1()
    {
        horizontal = 0;
        depth = 0;
    }

    public void Move(string line)
    {
        var components = line.Split();
        var command = components[0];
        var speed = int.Parse(components[1]);

        var direction = Directions[command];
        horizontal += direction.Item1 * speed;
        depth += direction.Item2 * speed;
    }

    public int Result()
    {
        return horizontal * depth;
    }

    public void Report()
    {
        Console.WriteLine("Horizonal {0} Depth {1} Multiplication {2}",
            horizontal, depth, Result());
    }
};
internal class Position2
{
    private int horizontal;
    private int depth;
    private int aim;

    private Dictionary<string, Tuple<int, int>> Directions = new Dictionary<string, Tuple<int, int>>()
    {
        {"forward", new Tuple<int, int>(1, 0)},
        {"up", new Tuple<int, int>(0, -1)},
        {"down", new Tuple<int, int>(0, 1)},
    };

    public Position2()
    {
        horizontal = 0;
        depth = 0;
        aim = 0;
    }

    public void Move(string line)
    {
        var components = line.Split();
        var command = components[0];
        var speed = int.Parse(components[1]);

        var direction = Directions[command];
        
        aim += direction.Item2 * speed;
        horizontal += direction.Item1 * speed;
        depth += direction.Item1 * aim * speed;
    }

    public int Result()
    {
        return horizontal * depth;
    }

    public void Report()
    {
        Console.WriteLine("Horizonal {0} Depth {1} Aim {2} Multiplication {3}",
            horizontal, depth, aim, Result());
    }
};
