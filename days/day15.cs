using AOC.util;

namespace AOC.days;

internal class Day15 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "15_test01.txt"), 40);
        AddRun("Part 1", () => RunPart(1, "15.txt"));
        AddRun("Test 2", () => RunPart(2, "15_test01.txt"), 315);
        AddRun("Part 2", () => RunPart(2, "15.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        var board = new Board2D<int>(lines, int.Parse, diagonalNeighbours: false);
        if (part == 2)
        {
            var width = board.Width;
            var height = board.Height;
            board.SetDimensions(width * 5, height * 5);
            for (var y = 0; y < height * 5; y++)
            {
                for (var x = 0; x < width * 5; x++)
                {
                    if (x >= width || y >= height)
                    {
                        var factor = x / width + y / height;
                        board.Board[new Coordinate(x, y)] =
                            1 + (board.Get(new Coordinate(x % width, y % width)) + factor - 1) % 9;
                    }
                }
            }
        }

        var start = new Coordinate(0, 0);
        var finish = new Coordinate(board.Width - 1, board.Height - 1);
        var done = new DefaultDictionary<Coordinate, int>();

        var costs = new Dictionary<Coordinate, long> {[finish] = board.Get(finish)};
        while (!costs.ContainsKey(start))
        {
            foreach (var current in costs.Keys.ToList())
            {
                if (done[current]++ > 1)
                {
                    costs.Remove(current);
                    continue;
                }
                var currentCost = costs[current];
                var currentValue = board.Get(current);
                foreach (var neighbour in board.Neighbours(current))
                {
                    {
                        var neighbourValue = board.Get(neighbour);
                        var viaMeNeighbourCost = currentCost + neighbourValue;
                        if (costs.ContainsKey(neighbour))
                        {
                            var neighbourCost = costs[neighbour];
                            if (viaMeNeighbourCost < neighbourCost)
                            {
                                costs[neighbour] = viaMeNeighbourCost;
                            }
                            else
                            {
                                if (neighbourCost + currentValue < currentCost)
                                {
                                    currentCost = neighbourCost + currentValue;
                                    costs[current] = currentCost;
                                }
                            }
                        }
                        else
                        {
                            costs[neighbour] = viaMeNeighbourCost;
                        }
                    }
                }
            }
        }

        return costs[start] - board.Get(start);

    }
}
