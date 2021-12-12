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
        foreach (var names in lines.Select(line => line.Split('-').ToList()))
        {
            graph.AddNodesAndEdge(names.First(), names.Last());
        }

        return GetRoutes(part, new HashSet<NamedNode>(), false, graph.Get("start"));
    }

    private static int GetRoutes(int part, IReadOnlySet<NamedNode> visitedOnce, bool visitedTwice, NamedNode newNode)
    {
        if (newNode.Name == "end") return 1;
        var isLowerCaseNode = char.IsLower(newNode.Name[0]);
        var newVisitedOnce = new HashSet<NamedNode>(visitedOnce);
        if (visitedOnce.Contains(newNode))
        {
            if (part == 1 || visitedTwice)
            {
                return 0;
            }
            visitedTwice = true;
        }
        else
        {
            if (isLowerCaseNode)
            {
                newVisitedOnce.Add(newNode);
            }
        }

        return newNode.Connections
            .Where(x => x.Name != "start")
            .Select(x => GetRoutes(part, newVisitedOnce, visitedTwice, x))
            .Sum();
    }
}