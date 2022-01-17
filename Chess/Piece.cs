namespace Chess;

public class Piece
{
    public Position Position { get; set; }
    
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
        Position = Position.Empty;
        Colour = colour;
        Type = type;
        Notation = type == PieceType.Pawn ? string.Empty : ((char)type).ToString();
    }

    public virtual IEnumerable<MoveAction> PossibleMoves()
    {
        return Enumerable.Empty<MoveAction>();
    }
}