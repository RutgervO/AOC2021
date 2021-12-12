// ReSharper disable once CheckNamespace
namespace aoc;

internal class Day12 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "12_test01.txt"), 10);
        AddRun("Test 2", () => RunPart(1, "12_test02.txt"), 19);
        AddRun("Test 3", () => RunPart(1, "12_test03.txt"), 226);
        AddRun("Part 1", () => RunPart(1, "12.txt"));
        AddRun("Test 4", () => RunPart(2, "12_test01.txt"), 36);
        AddRun("Test 5", () => RunPart(2, "12_test02.txt"), 103);
        AddRun("Test 6", () => RunPart(2, "12_test03.txt"), 3509);
        AddRun("Part 2", () => RunPart(2, "12.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        var graph = new NamedNodeGraph();
        foreach (var line in lines)
        {
            var name1 = line.Split('-').First();
            var name2 = line.Split('-').Last();
            graph.AddNodesAndEdge(name1, name2);
        }

        return GetRoutes(part, new List<NamedNode>{graph.Get("start")}).Count();
    }

    private static IEnumerable<List<NamedNode>> GetRoutes(int part, List<NamedNode> visited)
    {
        var results = new List<List<NamedNode>>();
        var current = visited.Last();
        if (current.Name == "end")
        {
            results.Add(visited);
        }
        else
        {
            var visitedDupes = visited
                .Select(x => x.Name)
                .Where(x => char.IsLower(x[0]))
                .GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();
            
            foreach (var node in current.Connections.Where(x=>x.Name != "start" && !visitedDupes.Contains(x.Name)))
            {
                var potential = visited.Append(node).ToList();
                var newDupes = potential
                    .Select(x => x.Name)
                    .Where(x => char.IsLower(x[0]))
                    .GroupBy(x => x)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .Count();
                if (newDupes < part)
                {
                    results.AddRange(GetRoutes(part, potential.ToList()));
                }
            }
        }
        return results;
    }
}