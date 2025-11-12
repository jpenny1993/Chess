using Chess.Actions;

namespace Chess;

public sealed class Movement
{
    public Position Origin { get; }

    public Position Destination { get; }

    public IEnumerable<IAction> Actions { get; }

    /// <summary>
    /// The piece that is moving. Used for generating algebraic notation.
    /// </summary>
    public Piece MovingPiece { get; }

    /// <summary>
    /// Indicates whether this move puts the opponent's king in check.
    /// </summary>
    public bool IsCheck { get; internal set; }

    /// <summary>
    /// Indicates whether this move results in checkmate of the opponent's king.
    /// </summary>
    public bool IsCheckmate { get; internal set; }

    /// <summary>
    /// Indicates whether this move promotes a pawn.
    /// </summary>
    public bool IsPromotion => Actions.OfType<Promotion>().Any();

    /// <summary>
    /// Indicates whether the destination square is defended by enemy pieces.
    /// If true, capturing on this square may lose material.
    /// </summary>
    public bool IsDefended { get; internal set; }

    /// <summary>
    /// Indicates whether this capture is safe (won't lose material).
    /// Only meaningful when IsCapture is true.
    /// A capture is safe if the destination is not defended, or if the captured piece
    /// is more valuable than the capturing piece.
    /// </summary>
    public bool IsSafeCapture { get; internal set; }

    public Movement(Piece movingPiece, Position origin, Position destination, params IAction[] actions)
    {
        MovingPiece = movingPiece;
        Origin = origin;
        Destination = destination;
        Actions = actions ?? Enumerable.Empty<IAction>();
    }

    public Movement(Piece movingPiece, Position origin, Position destination, IEnumerable<IAction> actions)
    {
        MovingPiece = movingPiece;
        Origin = origin;
        Destination = destination;
        Actions = actions;
    }

    // Existing convenience properties
    public bool IsCapture => Actions.OfType<Capture>().Any();
    public bool IsCastling => Actions.OfType<Castle>().Any();
    public bool IsEnPassant => Actions.OfType<EnPassant>().Any();

    /// <summary>
    /// Converts the movement to standard algebraic notation (SAN).
    /// Examples: e4, Nf3, Bxe5, O-O, e8=Q, Qh5+, Nf7#
    ///
    /// Note: This method generates basic algebraic notation without disambiguation.
    /// For full disambiguation (when multiple pieces of the same type can move to the same square),
    /// the board context would be needed to check other pieces' possible moves.
    /// </summary>
    public override string ToString()
    {
        // Handle castling
        if (IsCastling)
        {
            var castleAction = Actions.OfType<Castle>().First();
            return castleAction.IsKingSide ? "O-O" : "O-O-O";
        }

        var notation = new System.Text.StringBuilder();

        // Piece notation (empty for pawns)
        if (!MovingPiece.IsPawn)
        {
            notation.Append(MovingPiece.Notation);
        }

        // For pawn captures, include the origin file
        if (MovingPiece.IsPawn && IsCapture)
        {
            notation.Append(char.ToLower(Origin.X));
        }

        // Capture indicator
        if (IsCapture)
        {
            notation.Append('x');
        }

        // Destination square
        notation.Append(Destination.ToString().ToLower());

        // Promotion
        if (IsPromotion)
        {
            var promotionAction = Actions.OfType<Promotion>().First();
            notation.Append('=');
            notation.Append((char)promotionAction.Piece);
        }

        // Check or checkmate
        if (IsCheckmate)
        {
            notation.Append('#');
        }
        else if (IsCheck)
        {
            notation.Append('+');
        }

        return notation.ToString();
    }
}