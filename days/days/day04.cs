// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day04 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "04_test01.txt"), 4512);
        AddRun("Part 1", () => RunPart(1, "04.txt"));
        AddRun("Test 2", () => RunPart(2, "04_test01.txt"), 1924);
        AddRun("Part 2", () => RunPart(2, "04.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var stopAtFirstWinner = (part == 1);
        var inputLines = GetListOfLines(inputName).ToArray();

        var numbers = inputLines[0].Split(',').ToList().ConvertAll(int.Parse);
        var boardSize = inputLines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        var boards = new List<Board>();
        for (var line = 2; line < inputLines.Length; line += boardSize + 1)
        {
            boards.Add(new Board(boardSize, inputLines[line..(line + boardSize)]));
        }

        var winner = 0;
        foreach (var number in numbers)
        {
            foreach (var board in boards.Where(board => !board.IsWinner))
            {
                var result = board.Move(number);
                if (!board.IsWinner) continue;
                winner = result;
                if (stopAtFirstWinner)
                    return result;
            }
        }

        return winner;
    }

    class Board
    {
        private readonly HashSet<int> _numbers;
        private readonly List<HashSet<int>> _unmarkedLines;
        public bool IsWinner { get; private set; }

        public Board(int size, string[] lines)
        {
            _numbers = new HashSet<int>();
            _unmarkedLines = new List<HashSet<int>>();
            IsWinner = false;

            var columns = new List<HashSet<int>>();
            for (int i = 0; i < size; i++)
                columns.Add(new HashSet<int>());

            foreach (var line in lines)
            {
                var row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim()))
                    .ToList();
                _numbers.UnionWith(row);
                _unmarkedLines.Add(row.ToHashSet());
                foreach (var (number, i) in row.WithIndex())
                {
                    columns[i].Add(number);
                }
            }
            _unmarkedLines.AddRange(columns);
        }

        public int Move(int number)
        {
            if (!_numbers.Remove(number)) return 0;
            foreach (var line in _unmarkedLines)
            {
                line.Remove(number);
                if (line.Count == 0)
                {
                    IsWinner = true;
                    return number * _numbers.Sum();
                }
            }
            return 0;
        }
    }
}