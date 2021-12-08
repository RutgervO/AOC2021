// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day02 : Day
{
    protected override void SetSequence()
    {
        AddRun("Part 1", () => RunPart(1, "02.txt"));
        AddRun("Part 2", () => RunPart(2, "02.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var inputLines = GetListOfLines(inputName);

        if (part == 1)
        {
            var position = new Position1();
            foreach (var line in inputLines)
            {
                position.Move(line);
            }

            return position.Result();
        }

        var position2 = new Position2();
        foreach (var line in inputLines)
        {
            position2.Move(line);
        }

        return position2.Result();
    }

    private class Position1
    {
        private int _horizontal;
        private int _depth;

        private readonly Dictionary<string, Tuple<int, int>> _directions = new Dictionary<string, Tuple<int, int>>()
        {
            {"forward", new Tuple<int, int>(1, 0)},
            {"up", new Tuple<int, int>(0, -1)},
            {"down", new Tuple<int, int>(0, 1)},
        };

        public Position1()
        {
            _horizontal = 0;
            _depth = 0;
        }

        public void Move(string line)
        {
            var components = line.Split();
            var command = components[0];
            var speed = int.Parse(components[1]);

            var direction = _directions[command];
            _horizontal += direction.Item1 * speed;
            _depth += direction.Item2 * speed;
        }

        public int Result()
        {
            return _horizontal * _depth;
        }
    };

    private class Position2
    {
        private int _horizontal;
        private int _depth;
        private int _aim;

        private readonly Dictionary<string, Tuple<int, int>> _directions = new Dictionary<string, Tuple<int, int>>()
        {
            {"forward", new Tuple<int, int>(1, 0)},
            {"up", new Tuple<int, int>(0, -1)},
            {"down", new Tuple<int, int>(0, 1)},
        };

        public Position2()
        {
            _horizontal = 0;
            _depth = 0;
            _aim = 0;
        }

        public void Move(string line)
        {
            var components = line.Split();
            var command = components[0];
            var speed = int.Parse(components[1]);

            var direction = _directions[command];

            _aim += direction.Item2 * speed;
            _horizontal += direction.Item1 * speed;
            _depth += direction.Item1 * _aim * speed;
        }

        public int Result()
        {
            return _horizontal * _depth;
        }
    }
}