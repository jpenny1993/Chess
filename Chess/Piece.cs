namespace Chess;

public class Piece
{
    public char X { get; set; }
    
    public int Y { get; set; }
    
    public PieceColour Colour { get; }
    
    public string Notation { get; }
    
    public PieceType Type { get; }

    public bool IsBlack => Colour == PieceColour.Black;
    public bool IsWhite => Colour == PieceColour.White;

    public bool IsBishop => Type == PieceType.Bishop;
    public bool IsKnight => Type == PieceType.Knight;
    public bool IsKing => Type == PieceType.King;
    public bool IsPawn => Type == PieceType.Pawn;
    public bool IsQueen => Type == PieceType.Queen;
    public bool IsRook => Type == PieceType.Rook;
    
    internal Piece(PieceColour colour, PieceType type)
    {
        Colour = colour;
        Type = type;
        Notation = type == PieceType.Pawn ? string.Empty : ((char)type).ToString();
    }
    
    protected void SetPosition(char x, int y)
    {
        if (y < 1 | y > 8)
        {
            throw new ArgumentException("Invalid Y location.", nameof(y));
        }

        var xUppercase = Upper(x);
        if (xUppercase < 'A' | xUppercase > 'H')
        {
            throw new ArgumentException("Invalid X location.", nameof(x));
        }

        X = xUppercase;
        Y = y;
    }

    public bool IsAtLocation(char x, int y)
    {
        return Y == y && Upper(X) == Upper(x);
    }
    
    private static char Upper(char c)
    {
        return char.IsLower(c) ? char.ToUpper(c) : c;
    }
}