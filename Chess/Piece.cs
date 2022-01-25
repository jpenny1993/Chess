namespace Chess;

public abstract class Piece
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

    /// <summary>
    /// Defines whether the piece can move to the suggested position,
    /// regardless of whether there is another piece on that tile,
    /// but should be prevented by any intersecting pieces.
    /// </summary>
    public virtual bool CanMoveTo(Board board, Position position) => false; // TODO: make abstract

    /// <summary>
    /// Returns <see langword="true" /> if the colour of the pieces are matching. 
    /// </summary>
    public bool IsFriendly(Piece piece) => piece.Colour == Colour;

    /// <summary>
    /// Defines all the actually possible moves that a piece can make.
    /// This excludes passing intersecting pieces and moving to positions occupied by the same team. 
    /// </summary>
    public virtual IEnumerable<MoveAction> PossibleMoves(Board board) => Enumerable.Empty<MoveAction>(); // TODO: make abstract
    
    /// <summary>
    /// Defines all possible moves/paths that a piece could make, if the board was currently empty.
    /// </summary>
    public virtual IEnumerable<IEnumerable<Position>> TheoreticalPaths() => Enumerable.Empty<IEnumerable<Position>>(); // TODO: make abstract
}