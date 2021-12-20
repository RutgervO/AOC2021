using AOC.util;

namespace AOC.days;

internal class Day17 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "17_test01.txt"), 45);
        AddRun("Part 1", () => RunPart(1, "17.txt"));
        AddRun("Test 2", () => RunPart(2, "17_test01.txt"), 112);
        AddRun("Part 2", () => RunPart(2, "17.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var line = GetListOfLines(inputName).Single();
        var separators = new[] {"=", "..", ","};
        var split = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var box = new Box(
            new Coordinate(int.Parse(split[1]), int.Parse(split[4])),
            new Coordinate(int.Parse(split[2]), int.Parse(split[5])));

        var maxY = int.MinValue;
        int range = Math.Max(box.MaxX, box.MaxY);
        var hits = 0;

        foreach (var dx in Enumerable.Range(1, range))
        {
            foreach (var dy in Enumerable.Range(-range, 2 * range + 1))
            {
                if (Fire(part, box, dx, dy, maxY, out var newMaxY))
                {
                    maxY = Math.Max(maxY, newMaxY);
                    hits += 1;
                }
            }
        }
        
        return part == 1 ? maxY : hits;
    }

    private static bool Fire(int part, Box box, int dx, int dy, int oldMaxY, out int maxY)
    {
        int x = 0;
        int y = 0;
        maxY = y;
        while (true)
        {
            if (box.InBox(x, y))
            {
                return true;
            }

            if (dy <= 0 && (box.RightOrBelowBox(x, y) || (part == 1 && maxY < oldMaxY)))
            {
                return false;
            }

            if (dx <= 0 && box.LeftOfBox(x, y))
            {
                return false;
            }

            x += dx;
            y += dy;
            dy -= 1;
            dx = dx > 0 ? dx - 1 : dx < 0 ? dx + 1 : 0;
            maxY = Math.Max(maxY, y);
        }
    }
}