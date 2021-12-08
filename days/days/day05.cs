// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day05 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "05_test01.txt"), 5);
        AddRun("Part 1", () => RunPart(1, "05.txt"));
        AddRun("Test 2", () => RunPart(2, "05_test01.txt"), 12);
        AddRun("Part 2", () => RunPart(2, "05.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputLines = GetListOfLines(inputName);
        var board = new Board(inputLines, part == 1);
        return board.CountOverlaps();
    }

    public class Coordinate
    {
        public int X;
        public int Y;

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coordinate p)
        {
            return X == p.X && Y == p.Y;
        }
    }

    private class Line
    {
        private readonly Coordinate _a;
        private readonly Coordinate _b;
        public bool IsStraight { get; private set; }

        public Line(Coordinate a, Coordinate b)
        {
            _a = a;
            _b = b;
            IsStraight = _a.X == _b.X || _a.Y == _b.Y;
        }


        public IEnumerable<Coordinate> GetCoordinates()
        {
            var diffX = Math.Max(Math.Min(_b.X - _a.X, 1), -1);
            var diffY = Math.Max(Math.Min(_b.Y - _a.Y, 1), -1);
            var pos = new Coordinate(_a.X, _a.Y);
            yield return pos;
            while (!pos.Equals(_b))
            {
                pos.X += diffX;
                pos.Y += diffY;
                yield return pos;
            }
        }
    }

    private class Board
    {
        private readonly DefaultDictionary<Tuple<int, int>, int> _counts;

        public Board(List<string> lines, bool onlyStraight)
        {
            _counts = new DefaultDictionary<Tuple<int, int>, int>();
            char[] separators = {' ', ',', '-', '>'};
            foreach (var line in lines)
            {
                var splits = line.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList()
                    .ConvertAll(int.Parse);
                var l = new Line(new Coordinate(splits[0], splits[1]), new Coordinate(splits[2], splits[3]));
                if (onlyStraight && !l.IsStraight) continue;
                foreach (var pos in l.GetCoordinates())
                {
                    _counts[new Tuple<int, int>(pos.X, pos.Y)] += 1;
                }
            }
        }

        public int CountOverlaps()
        {
            return _counts.Values.Count(x => x > 1);
        }
    }
}