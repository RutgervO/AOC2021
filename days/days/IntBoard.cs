// ReSharper disable once CheckNamespace
namespace aoc;

public class IntBoard
{
    private int[,] _board;
    public int Width { get; }
    public int Height { get; }
    public bool DiagonalNeighbours { get; }

    public (int dx, int dy)[] NeighbourDeltas { get; }

    public IntBoard(IEnumerable<string> lines, bool hasDiagonalNeighbours=true)
    {
        var input = lines.ToArray();
        Width = input.First().Length;
        Height = input.Length;
        _board = new int[Width, Height];
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _board[x, y] = int.Parse(input[y][x].ToString());
            }
        }

        DiagonalNeighbours = hasDiagonalNeighbours;
        NeighbourDeltas = DiagonalNeighbours ?
            new (int dx, int dy)[] {(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)} :
            new (int dx, int dy)[] {(0, -1), (-1, 0), (1, 0), (0, 1)};
    }

    IEnumerable<(int X, int Y)> GetAllCoordinates()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                yield return (X:x, Y:y);
            }
        }
    }

    public void ApplyFunctionToAllCells(Func<int, int> f)
    {
        foreach (var (x, y) in GetAllCoordinates())
            _board[x, y] = f(_board[x, y]);
    }

    public IEnumerable<(int X, int Y)> GetCoordinatesThatMatch(Func<int,bool> f)
    {
        foreach (var (x, y) in GetAllCoordinates())
            if (f(_board[x, y]))
                yield return (X:x, Y:y);
    }

    public bool IsOnBoard((int X, int Y) coord)
    {
        return 0 <= coord.X && coord.X < Width && 0 <= coord.Y && coord.Y < Height;
    }
    
    public IEnumerable<(int X, int Y)> GetNeighbours((int x, int y)coord)
    {
        foreach (var (dx, dy) in NeighbourDeltas)
        {
            var neighbour = (X:coord.x + dx, Y:coord.y + dy);
            if (IsOnBoard(neighbour))
                yield return neighbour;
        }
    }

    public IEnumerable<(int X, int Y)> GetNeighbours(IEnumerable<(int X, int Y)> coordinates)
    {
        return coordinates.SelectMany(GetNeighbours);
    }

    public void ApplyValueToCoordinates(int value, IEnumerable<(int X, int Y)> coordinates)
    {
        foreach (var coordinate in coordinates)
            _board[coordinate.X, coordinate.Y] = value;
    }
    
    public void ApplyFunctionToCoordinates(Func<int,int> f, IEnumerable<(int X, int Y)> coordinates)
    {
        foreach (var coordinate in coordinates)
            _board[coordinate.X, coordinate.Y] = f(_board[coordinate.X, coordinate.Y]);
    }

    public IEnumerable<int> GetAllValues()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                yield return _board[x, y];
            }
        }
    }
    
    public void Print(string? title = null, Func<int,bool>?highlight = null)
    {
        if (title is not null)
        {
            Console.Out.WriteLine(title);
        }
        var maxValueLen = GetAllValues().Max().ToString().Length;
        var multiDigit = maxValueLen > 1;
        var formatString = $"D{maxValueLen}";
        
        
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var value = _board[x, y];
                var highlighted = highlight is not null && highlight(value);
                if (highlighted)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (multiDigit) { 
                    Console.Out.Write($"{_board[x, y].ToString(formatString)} ");
                }
                else
                {
                    Console.Out.Write(_board[x, y]);
                }
                if (highlighted)
                {
                    Console.ResetColor();
                }
            }
            Console.Out.WriteLine("");
        }
    }
}