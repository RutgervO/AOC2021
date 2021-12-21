namespace AOC.days;

internal class Day21 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "21_test01.txt"), 739785);
        AddRun("Part 1", () => RunPart(1, "21.txt"));
        AddRun("Test 2", () => RunPart(2, "21_test01.txt"), 444356092776315);
        AddRun("Part 2", () => RunPart(2, "21.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var positions = GetListOfLines(inputName).Select(x => int.Parse(x.Split(' ')[4])).ToArray();
        
        if (part == 1) return RunPart1(positions);
        
        var (wins1, wins2) = CountWins(0, 0, positions[0], positions[1]);
        return Math.Max(wins1, wins2);
    }

    private readonly Dictionary<(long, long, int, int), (long, long)> _cache =
        new Dictionary<(long, long, int, int), (long, long)>();

    private readonly Dictionary<int, int> _throwDistribution = new Dictionary<int, int>()
    {
        {3, 1}, {4, 3}, {5, 6}, {6, 7}, {7, 6}, {8, 3}, {9, 1}
    };

    private (long, long) CountWins(long score1, long score2, int pos1, int pos2)
    {
        var key = (score1, score2, pos1, pos2);
        if (_cache.ContainsKey(key))
        {
            return _cache[key];
        }
        long wins1 = 0;
        long wins2 = 0;
        foreach (var (roll, times) in _throwDistribution)
        {
            var pos = (pos1 + roll - 1) % 10 + 1;
            var score = score1 + pos;
            if (score >= 21)
            {
                wins1 += times;
            }
            else
            {
                var wins = CountWins(score2, score, pos2, pos);
                wins1 += times * wins.Item2;
                wins2 += times * wins.Item1;
            }
        }

        _cache[key] = (wins1, wins2);
        return (wins1, wins2);
    }

    private long RunPart1(int[] positions)
    {
        const int maxScore = 1000;
        const int maxRolls = 3;
        var die = 1;
        var scores = new[] {0, 0};
        var player = 0;
        var rolls = 0;

        while (true)
        {
            var roll = 0;
            foreach (var _ in Enumerable.Range(0, maxRolls))
            {
                roll += die;
                die = (die % 100) + 1;
                rolls++;
            }

            positions[player] = (positions[player] + roll -1) % 10 + 1;
            scores[player] += positions[player];
            if (scores[player] >= maxScore)
            {
                return rolls * scores[1 - player];
            }

            player = 1 - player;
        }
    }
}