using AOC.util;

namespace AOC.days;

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
        var sheet = lines
            .TakeWhile(x => x.Length > 0)
            .Select(x => x.Split(','))
            .Select(x => new Coordinate(int.Parse(x[0]), int.Parse(x[1])))
            .ToHashSet();
        var folds = lines
            .Skip(sheet.Count + 1)
            .Select(x => x.Split('=', ' '))
            .Select(x => new Tuple<char, int>(x[2][0], int.Parse(x[3])))
            .ToList();

        foreach (var (axis, pos) in folds)
        {
            var posX = axis == 'x' ? pos : int.MaxValue;
            var posY = axis == 'y' ? pos : int.MaxValue;
            
            sheet = sheet
                .Select(x => new Coordinate(
                    x.X > posX ? 2 * pos - x.X : x.X,
                    x.Y > posY ? 2 * pos - x.Y : x.Y))
                .ToHashSet();
            
            if (part==1) return sheet.Count;
        }

        var maxX = sheet.Select(x => x.X).Max();
        var maxY = sheet.Select(x => x.Y).Max();
        for (var y = 0; y <= maxY; y++)
        {
            Console.WriteLine();
            for (var x = 0; x <= maxX; x++)
            {
                Console.Write(sheet.Contains(new Coordinate(x, y)) ? '#' : ' ');
            }
        }

        return 0;
    }
}