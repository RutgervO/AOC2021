// ReSharper disable once CheckNamespace
namespace aoc;

public readonly struct Coordinate
{
    public int X { get; }
    public int Y { get; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coordinate((int X, int Y) coordinates)
    {
        X = coordinates.X;
        Y = coordinates.Y;
    }

    public Coordinate(Coordinate coordinate)
    {
        X = coordinate.X;
        Y = coordinate.Y;
    }

    public Coordinate Add(Coordinate other)
    {
        return new Coordinate(X + other.X, Y + other.Y);
    }

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}