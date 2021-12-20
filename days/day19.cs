using AOC.util;

namespace AOC.days;

internal class Day19 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "19_test01.txt"), 79);
        AddRun("Part 1", () => RunPart(1, "19.txt"));
        AddRun("Test 2", () => RunPart(2, "19_test01.txt"), 3621);
        AddRun("Part 2", () => RunPart(2, "19.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        var scanners = new List<Scanner>();
        foreach (var line in lines.Where(line => line.Length != 0))
        {
            if (line.StartsWith("--"))
            {
                scanners.Add(new());
            }
            else
            {
                scanners.Last().AddBeacon(line);
            }
        }
        
        scanners[0].PositionKnown = true;
        var absBeacons = new HashSet<Coordinate3D>(scanners[0].Beacons[0]);

        while (scanners.Exists(x => !x.PositionKnown))
        {
            foreach (var scanner in scanners.Where(x => !x.PositionKnown).ToList())
            {
                foreach (var rotation in Enumerable.Range(0, 24))
                {
                    var offsetCounts = new DefaultDictionary<Coordinate3D, int>();
                    var newBeacons = scanner.Beacons[rotation];
                    var hits = 0;
                    foreach (var absBeacon in absBeacons)
                    {
                        if (hits < 12) {
                            foreach (var offset in newBeacons.Select(beacon => Subtract(beacon, absBeacon)))
                            {
                                offsetCounts[offset] += 1;
                                hits = Math.Max(hits, offsetCounts[offset]);
                            }
                        }
                    }

                    if (hits <= 11)
                        continue;
                    var hit = offsetCounts
                        .Where(x => x.Value > 11)
                        .Select(x => x.Key)
                        .Take(1).Single();
                    scanner.PositionKnown = true;
                    scanner.AbsPosition = Subtract(scanner.AbsPosition, hit);
                    foreach (var beacon in newBeacons)
                    {
                        absBeacons.Add(Subtract(beacon, hit));
                    }
                    break;
                }
            }
        }

        if (part == 1) return absBeacons.Count;

        return (from s1 in scanners from s2 in scanners
            select Math.Abs(s1.AbsPosition.X - s2.AbsPosition.X)
                 + Math.Abs(s1.AbsPosition.Y - s2.AbsPosition.Y)
                 + Math.Abs(s1.AbsPosition.Z - s2.AbsPosition.Z)).Max();
    }

    private static readonly Func<Coordinate3D, Coordinate3D>[] FaceRotations = {
        (c) => new(+c.X, +c.Y, +c.Z), // not rotated
        (c) => new(+c.Z, +c.Y, -c.X), // up face rotated 90 right
        (c) => new(-c.X, +c.Y, -c.Z), // up face rotated 180
        (c) => new(-c.Z, +c.Y, +c.X), // up face rotated 90 left
    };

    private static readonly Func<Coordinate3D, Coordinate3D>[] CubeRotations = {
        (c) => new(+c.X, +c.Y, +c.Z), // not rotated
        (c) => new(-c.Y, +c.X, +c.Z), // right side up
        (c) => new(+c.Y, -c.X, +c.Z), // left side up
        (c) => new(-c.X, -c.Y, +c.Z), // bottom side up
        (c) => new(+c.X, -c.Z, +c.Y), // front side up
        (c) => new(+c.X, +c.Z, -c.Y), // back side up
    };
    private static Coordinate3D Rotate(Coordinate3D coordinate, int rotation)
    {
        var upFace = rotation / 6;
        var rotated = rotation - (upFace * 6);
        return CubeRotations[rotated](FaceRotations[upFace](coordinate));
    }

    private Coordinate3D Subtract(Coordinate3D a, Coordinate3D b)
    {
        return new Coordinate3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    private class Scanner
    {
        public readonly HashSet<Coordinate3D>[] Beacons;
        public bool PositionKnown;
        public Coordinate3D AbsPosition;

        public Scanner()
        {
            Beacons = new HashSet<Coordinate3D>[24];
            foreach (var rotation in Enumerable.Range(0, 24))
                Beacons[rotation] = new HashSet<Coordinate3D>();
            PositionKnown = false;
            AbsPosition = new(0, 0, 0);
        }

        public void AddBeacon(string line)
        {
            var split = line.Split(',').Select(int.Parse).ToArray();
            var coordinate = new Coordinate3D(split[0], split[1], split[2]);
            foreach (var rotation in Enumerable.Range(0, 24))
                Beacons[rotation].Add(Rotate(coordinate, rotation));
        }

    }
}

public record Coordinate3D(int X, int Y, int Z);