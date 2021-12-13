namespace AOC.days;

internal class Day09 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "09_test01.txt"), 15);
        AddRun("Part 1", () => RunPart(1, "09.txt"));
        AddRun("Test 2", () => RunPart(2, "09_test01.txt"), 1134);
        AddRun("Part 2", () => RunPart(2, "09.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var board = GetListOfLines(inputName).ToArray();
        var maxX = board[0].Length - 1;
        var maxY = board.Length - 1;
        var result = 0;
        var basins = new List<int>();
        var directions = new(int X,int Y)[] {(-1, 0),(1, 0),(0, -1),(0, 1)};
        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var current = board[y][x];
                if ((y == 0 || current < board[y - 1][x])
                    && (x == 0 || current < board[y][x - 1])
                    && (x == maxX || current < board[y][x + 1])
                    && (y == maxY || current < board[y + 1][x]))
                {
                    if (part == 1)
                    {
                        result += 1 + int.Parse(current.ToString());
                    }
                    else
                    {
                        var members = new HashSet<(int X, int Y)> {(x, y)};
                        var newMembers = 1;
                        while (newMembers > 0)
                        {
                            newMembers = 0;
                            foreach (var member in members.ToArray())
                            {
                                current = board[member.Y][member.X];
                                foreach (var (dx, dy) in directions)
                                {
                                    var newX = member.X + dx;
                                    var newY = member.Y + dy;
                                    if (newX < 0 || newX > maxX || newY < 0 || newY > maxY ||
                                        members.Contains((newX, newY)))
                                        continue;
                                    var value = board[newY][newX];
                                    if (value <= current || value >= '9') continue;
                                    members.Add((newX, newY));
                                    newMembers++;
                                }
                            }
                        }
                        basins.Add(members.Count);
                        result = basins.OrderByDescending(i => i).Take(3).Aggregate(1, (i1,i2) => i1 * i2);
                    }
                }
            }
        }

        return result; 
    }
}