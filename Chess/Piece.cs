using Chess.Actions;

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
        Position = default;
        Colour = colour;
        Type = type;
        Notation = type == PieceType.Pawn ? string.Empty : ((char)type).ToString();
    }

    /// <summary>
    /// Defines whether the piece can move to the suggested position,
    /// regardless of whether there is another piece on that tile,
    /// but should be prevented by any intersecting pieces.
    /// </summary>
    public bool CanMoveTo(Board board, Position position)
    {
        var pathsContainingDestination = TheoreticalPaths()
            .Where(path => path.Contains(position));
        
        foreach (var path in pathsContainingDestination)
        foreach (var step in path)
        {
            var movement = GetMovement(board, path, step);
            if (movement == default)
            {
                break;
            }

            if (movement.Destination.Equals(position))
            {
                return true;
            }

            if (movement.IsCapture)
            {
                break;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns <see langword="true" /> if the colour of the pieces are matching. 
    /// </summary>
    public bool IsFriendly(Piece piece) => piece.Colour == Colour;

    /// <summary>
    /// Defines all the actually possible moves that a piece can make.
    /// This excludes passing intersecting pieces and moving to positions occupied by the same team. 
    /// </summary>
    public IEnumerable<Movement> PossibleMoves(Board board)
    {
        foreach (var path in TheoreticalPaths())
        foreach (var step in path)
        {
            var movement = GetMovement(board, path, step);
            if (movement == default)
            {
                break;
            }
            
            yield return movement;
            if (movement.IsCapture)
            {
                break;
            }
        }
    }
    
    /// <summary>
    /// Defines all possible moves/paths that a piece could make, if the board was currently empty.
    /// </summary>
    public virtual IEnumerable<TheoreticalPath> TheoreticalPaths() => Enumerable.Empty<TheoreticalPath>();

    protected virtual Movement? GetMovement(Board board, TheoreticalPath path, Position step)
    {
        var intersectingPiece = board.FindPiece(step);
        // TODO: check sniper rules for castling
        // TODO: check next move to see if it's check
        // TODO: check if the current position, moves the player out of check
            
        if (intersectingPiece == default)
        {
            // Valid move, the tile is empty
            return new (Position, step);
        }

        // Invalid move, can't capture teammates
        if (IsFriendly(intersectingPiece))
        {
            return default;
        }
            
        // Valid move, can capture enemies
        return new (Position, step, new Capture(intersectingPiece.Type));
    }
}