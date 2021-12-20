using AOC.util;

namespace AOC.days;

internal class Day20 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "20_test01.txt"), 35);
        AddRun("Part 1", () => RunPart(1, "20.txt"));
        AddRun("Test 2", () => RunPart(2, "20_test01.txt"), 3351);
        AddRun("Part 2", () => RunPart(2, "20.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        var board = new Board2D<int>(lines.Skip(2), Parser)
        {
            NeighbourDeltas = new (int dx, int dy)[]
                {(-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1)}
        };
        var enhancements = lines[0].Select(x => Parser(x.ToString())).ToArray();
        var startX = 0;
        var startY = 0;
        var width = board.Width;
        var height = board.Height;
        var result = 0;
        var infiniteValue = 0;

        foreach (var _ in Enumerable.Range(1, part == 1 ? 2 : 50))
        {
            startX -= 2;
            startY -= 2;
            width += 4;
            height += 4;
            var newBoard = new DefaultDictionary<Coordinate, int>();
            result = 0;
            foreach (var x in Enumerable.Range(startX, width))
            {
                foreach (var y in Enumerable.Range(startY, height))
                {
                    var coordinate = new Coordinate(x, y);
                    int value = 0;
                    foreach (var neighbour in AllNeighbours(board, coordinate))
                    {
                        var bit = infiniteValue;
                        if (board.Board.ContainsKey(neighbour))
                        {
                            bit = board.Board[neighbour];
                        }
                        value = 2 * value + bit;
                    }
                    value = enhancements[value];
                    result += value;
                    newBoard[coordinate] = value;
                }
            }
            board.Board = newBoard;
            infiniteValue = enhancements[511 * infiniteValue];
        }

        return result;
    }

    private IEnumerable<Coordinate> AllNeighbours(Board2D<int> board, Coordinate coordinate)
    {
        return board.NeighbourDeltas.Select(delta => coordinate.Add(new Coordinate(delta)));
    }

    private static int Parser(string item) => item == "#" ? 1 : 0;
}