// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day11 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "11_test01.txt"), 1656);
        AddRun("Part 1", () => RunPart(1, "11.txt"));
        AddRun("Test 2", () => RunPart(2, "11_test01.txt"), 195);
        AddRun("Part 2", () => RunPart(2, "11.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var board = new IntBoard(GetListOfLines(inputName));
        long result = 0;
        for (var step = 1; part == 2 || step <= 100; step++)
        {
            board.ApplyFunctionToAllCells(x => x + 1);
            List<(int X, int Y)> flashers;
            do
            {
                flashers = board.GetCoordinatesThatMatch(x => x > 9).ToList();
                board.ApplyValueToCoordinates(0, flashers);
                board.ApplyFunctionToCoordinates(x => x == 0 ? 0 : x + 1, board.GetNeighbours(flashers));
                result += flashers.Count;
            } while (flashers.Count > 0);

            if (part == 2 && board.GetAllValues().Sum() == 0)
                return step;
        }
        return result;
    }
}