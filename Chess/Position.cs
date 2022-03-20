using System.Text.RegularExpressions;

namespace Chess;

public readonly struct Position
{
    public const char MinX = 'A';
    public const char MaxX = 'H';

    public const int MinY = 1;
    public const int MaxY = 8;
    
    public char X { get; }

    public int Y { get; }

    public Position(char x, int y)
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
    
    public Position(int x, int y)
        : this((char)x, y)
    {
    }

    public Position(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    public bool Equals(Position position) => Y == position.Y && X == position.X;

    public bool Equals(int x, int y) => Y == y && X == Upper((char)x);

    public override bool Equals(object? obj) => (obj is Position position) && Equals(position);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"{X}{Y}";

    public static bool IsValid(int x, int y) => IsValidX(x) && IsValidY(y);

    public static bool IsValidX(int x) => x is >= MinX and <= MaxX;
    
    public static bool IsValidY(int y) => y is >= MinY and <= MaxY;
    
    public static implicit operator string(Position position) => position.ToString();

    public static implicit operator Position(string tileRef)
    {
        if (tileRef == null || tileRef.Length is < 2 or > 2 || !char.IsLetter(tileRef[0]) || !char.IsDigit(tileRef[1]))
            throw new InvalidCastException($"Unexpected cast \"{tileRef}\" to {nameof(Position)}.");
        
        return new (tileRef[0], int.Parse(tileRef.Substring(1,1)));
    }
    
    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !(left == right);
    
    private static char Upper(char c) => char.IsLower(c) ? char.ToUpper(c) : c;
}