namespace Chess;

public sealed class Position
{
    public const char MinX = 'A';
    public const char MaxX = 'H';

    public const int MinY = 1;
    public const int MaxY = 8;
    
    public char X { get; private set; }

    public int Y { get; private set; }

    private Position()
    {
    }

    public Position(char x, int y)
    {
        Set(x, y);
    }
    
    public Position(int x, int y)
        : this((char)x, y)
    {
    }

    public void Set(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    public void Set(char x, int y)
    {
        if (!IsValidY(y))
        {
            throw new ArgumentException("Invalid Y location.", nameof(y));
        }

        var xUppercase = Upper(x);
        if (!IsValidX(xUppercase))
        {
            throw new ArgumentException("Invalid X location.", nameof(x));
        }

        X = xUppercase;
        Y = y;
    }

    public bool Equals(Position position)
    {
        return Y == position.Y && X == position.X;
    }

    public bool Equals(int x, int y)
    {
        return Y == y && X == Upper((char)x);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Position position)
        {
            return Equals(position);
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{X}{Y}";
    }

    public static Position Empty => new()
    {
        X = char.MinValue,
        Y = 0
    };

    public static bool IsValid(int x, int y)
    {
        return IsValidX(x) && IsValidY(y);
    }

    public static bool IsValidX(int x)
    {
        return x is >= MinX and <= MaxX;
    }
    
    public static bool IsValidY(int y)
    {
        return y is >= MinY and <= MaxY;
    }
    
    public static implicit operator string(Position position)
    {
        return position.ToString();
    }

    private static char Upper(char c)
    {
        return char.IsLower(c) ? char.ToUpper(c) : c;
    }
}