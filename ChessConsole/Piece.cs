namespace ChessConsole;

public class Piece
{
    public char X { get; set; }
    
    public int Y { get; set; }
    
    public PieceColour PieceColour { get; }
    
    public string Notation { get; }
    
    public PieceType Type { get; }

    public bool IsBlack => PieceColour == PieceColour.Black;

    public bool IsWhite => PieceColour == PieceColour.White;

    public Piece(char x, int y, PieceColour pieceColour, PieceType type)
    {
        PieceColour = pieceColour;
        Type = type;
        Notation = type == PieceType.Pawn ? string.Empty : ((char)type).ToString();
        X = Upper(x);
        Y = y;

        if (X < 'A' | X > 'H')
        {
            throw new ArgumentException("Invalid X location.", nameof(x));
        }

        if (Y < 1 | Y > 8)
        {
            throw new ArgumentException("Invalid Y location.", nameof(y));
        }
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