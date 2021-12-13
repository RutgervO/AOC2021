// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day13 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "13_test01.txt"), 17);
        AddRun("Part 1", () => RunPart(1, "13.txt"));
        AddRun("Part 2", () => RunPart(2, "13.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        var board = lines
            .TakeWhile(x => x.Length > 0)
            .Select(x => x.Split(','))
            .Select(x => new Coordinate(int.Parse(x[0]), int.Parse(x[1])))
            .ToHashSet();
        var commands = lines
            .Skip(board.Count + 1)
            .Select(x => x.Split('=', ' '))
            .Select(x => new Tuple<char, int>(x[2][0], int.Parse(x[3])))
            .ToList();

        foreach (var (dir, pos) in commands)
        {
            var posX = dir == 'x' ? pos : int.MaxValue;
            var posY = dir == 'y' ? pos : int.MaxValue;
            
            board = board
                .Select(x => new Coordinate(
                    x.X > posX ? 2 * pos - x.X : x.X,
                    x.Y > posY ? 2 * pos - x.Y : x.Y))
                .ToHashSet();
            
            if (part==1) return board.Count;
        }

        var maxX = board.Select(x => x.X).Max();
        var maxY = board.Select(x => x.Y).Max();
        for (var y = 0; y <= maxY; y++)
        {
            Console.WriteLine();
            for (var x = 0; x <= maxX; x++)
            {
                Console.Write(board.Contains(new Coordinate(x, y)) ? '#' : ' ');
            }
        }

        return 0;
    }
}