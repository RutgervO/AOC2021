namespace AOC.days;

internal class Day22 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "22_test01.txt"), 39);
        AddRun("Test 2", () => RunPart(1, "22_test02.txt"), 590784);
        AddRun("Part 1", () => RunPart(1, "22.txt"));
        AddRun("Test 3", () => RunPart(2, "22_test03.txt"), 2758514936282235);
        AddRun("Part 2", () => RunPart(2, "22.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var lines = GetListOfLines(inputName);
        var cubes = new List<Cube>(); // non-overlapping cubes that are on
        const int maxCoord = 50;

        foreach (var line in lines)
        {
            var split = line.Split(new[] {" ", "=", "..", ","}, StringSplitOptions.RemoveEmptyEntries);
            var turnOn = split[0] == "on";
            var newCube = new Cube(new Coordinate3D(int.Parse(split[2])*2-1, int.Parse(split[5])*2-1, int.Parse(split[8])*2-1),
                new Coordinate3D(int.Parse(split[3])*2+1, int.Parse(split[6])*2+1, int.Parse(split[9])*2+1));
            
            int[] coordinates = {newCube.A.X, newCube.A.Y, newCube.A.Z, newCube.B.X, newCube.B.Y, newCube.B.Z};
            if (part == 1 && coordinates.Select(x => Math.Abs((int)x)).Any(x => x > maxCoord * 2 + 1)) continue;
            
            if (turnOn)
            {
                var newCubes = new List<Cube> {newCube};
                newCubes = cubes.Aggregate(newCubes, (current, cube) => cube.Complements(current).ToList());
                cubes.AddRange(newCubes);
            }
            else
            {
                var newCubes = new List<Cube>();
                foreach (var cube in cubes)
                {
                    newCubes.AddRange(newCube.Complements(cube));
                }
                cubes = newCubes;
            }
        }

        return cubes.Select(x => x.Volume).Sum()/8;
    }

    public class Cube
    {
        public Coordinate3D A { get; }
        public Coordinate3D B { get; }
        public long Volume { get; }

        public Cube(Coordinate3D a, Coordinate3D b)
        {
            A = new Coordinate3D(a.X, a.Y, a.Z);
            B = new Coordinate3D(b.X, b.Y, b.Z);
            Volume = ((long)b.X - (long)a.X) * ((long)b.Y - (long)a.Y) * ((long)b.Z - (long)a.Z);
        }

        public IEnumerable<Cube> Complements(IEnumerable<Cube> cubes)
        {
            return cubes.SelectMany(this.Complements);
        }

        public IEnumerable<Cube> Complements(Cube cube)
        {
            if (IsDisjointed(cube))
            {
                yield return cube;
            }
            else
            {
                var cubes = new List<Cube> {cube};
                foreach (var part in
                    SplitCubes(
                        SplitCubes(
                            SplitCubes(
                                SplitCubes(
                                    SplitCubes(
                                        SplitCubes(cubes, null, null, A.Z),
                                        null, null, B.Z),
                                    null, A.Y, null),
                                null, B.Y, null),
                            A.X, null, null),
                        B.X, null, null).Where(part => !this.Contains(part)))
                {
                    yield return part;
                }
            }
        }

        private bool Contains(Cube cube)
        {
            return cube.A.X >= this.A.X && cube.B.X <= this.B.X
                && cube.A.Y >= this.A.Y && cube.B.Y <= this.B.Y
                && cube.A.Z >= this.A.Z && cube.B.Z <= this.B.Z;
        }

        private bool IsDisjointed(Cube c)
        {
            return (c.B.X <= A.X || c.A.X >= B.X)
                   || (c.B.Y <= A.Y || c.A.Y >= B.Y)
                   || (c.B.Z <= A.Z || c.A.Z >= B.Z);
        }

        private static IEnumerable<Cube> SplitCubes(IEnumerable<Cube> cubes, int? x, int? y, int? z)
        {
            foreach (var cube in cubes)
            {
                if (z is not null && cube.A.Z < z && z < cube.B.Z)
                {
                    yield return new Cube(cube.A, new Coordinate3D(cube.B.X, cube.B.Y, (int) z));
                    yield return new Cube(new Coordinate3D(cube.A.X, cube.A.Y, (int) z), cube.B);
                }
                else
                {
                    if (y is not null && cube.A.Y < y && y < cube.B.Y)
                    {
                        yield return new Cube(cube.A, new Coordinate3D(cube.B.X, (int) y, cube.B.Z));
                        yield return new Cube(new Coordinate3D(cube.A.X, (int) y, cube.A.Z), cube.B);
                    }
                    else
                    {
                        if (x is not null && cube.A.X < x && x < cube.B.X)
                        {
                            yield return new Cube(cube.A, new Coordinate3D((int) x, cube.B.Y, cube.B.Z));
                            yield return new Cube(new Coordinate3D((int) x, cube.A.Y, cube.A.Z), cube.B);
                        }
                        else
                        {
                            yield return cube;
                        }
                    }
                }
            }
        }
    }
}