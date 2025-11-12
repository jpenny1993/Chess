using Chess.Actions;

namespace Chess.Pieces;

public sealed class King : Piece
{
    private const PieceType ChessPiece = PieceType.King;

    public King(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }

    public override IEnumerable<TheoreticalPath> TheoreticalPaths()
    {
        const int moveSpeed = 1;

        // Regular king moves (8 surrounding squares)
        for (var x = (Position.X - moveSpeed); x <= (Position.X + moveSpeed); x++)
        for (var y = (Position.Y - moveSpeed); y <= (Position.Y + moveSpeed); y++)
        {
            if (Position.Equals(x, y))
            {
                continue;
            }

            if (Position.IsValid(x, y))
            {
                yield return new (x, y);
            }
        }

        // Castling moves
        // Kingside castle: King moves two squares to the right
        if (Position.IsValid(Position.X + 2, Position.Y))
        {
            yield return new (Position.X + 2, Position.Y);
        }

        // Queenside castle: King moves two squares to the left
        if (Position.IsValid(Position.X - 2, Position.Y))
        {
            yield return new (Position.X - 2, Position.Y);
        }
    }

    internal protected override Movement? GetMovement(Piece piece, Board board, TheoreticalPath path, Position step)
    {
        var intersectingPiece = board.FindPiece(step);

        // Check if this is a castling move (king moving 2 squares horizontally)
        var horizontalDistance = Math.Abs(step.X - Position.X);
        var isCastlingMove = horizontalDistance == 2 && step.Y == Position.Y;

        if (isCastlingMove)
        {
            // For castling, we need to do full validation
            // Note: This will be null during attack map building to avoid infinite recursion
            return GetCastlingMovement(piece, board, step);
        }

        // Regular king move logic
        if (intersectingPiece != default && IsFriendly(intersectingPiece))
        {
            return default;
        }

        var actions = new List<IAction>();

        if (intersectingPiece != default)
        {
            actions.Add(new Capture(intersectingPiece.Type));
        }

        return new (piece, Position, step, actions);
    }

    private Movement? GetCastlingMovement(Piece piece, Board board, Position kingDestination)
    {
        // King must not have moved
        if (HasMoved)
        {
            return default;
        }

        // King cannot castle out of check
        // We need to avoid infinite recursion here - BoardAnalysis will call GetMovement,
        // which might call GetCastlingMovement, which creates a new BoardAnalysis.
        // To prevent this, we check if ANY enemy piece can attack the king's current position
        // without using BoardAnalysis.IsKingInCheck().
        if (IsUnderDirectAttack(board))
        {
            return default;
        }

        // Determine if this is kingside or queenside castling
        var isKingside = kingDestination.X > Position.X;

        // Get the rook position based on castling side
        var rookStartX = isKingside ? (char)'H' : (char)'A';
        var rookStartPos = new Position(rookStartX, Position.Y);
        var rook = board.FindPiece(rookStartPos);

        // Rook must exist, be a rook, be the same color, and not have moved
        if (rook == default || !rook.IsRook || !IsFriendly(rook) || rook.HasMoved)
        {
            return default;
        }

        // Check that squares between king and rook are empty
        var squaresBetween = GetSquaresBetween(Position, rookStartPos);
        foreach (var square in squaresBetween)
        {
            if (board.FindPiece(square) != default)
            {
                return default;
            }
        }

        // King cannot castle through check - verify each square the king passes through
        var kingPath = GetKingCastlingPath(kingDestination);
        foreach (var square in kingPath)
        {
            // Check if this square is under attack without using BoardAnalysis (to avoid recursion)
            if (IsSquareUnderAttack(board, square))
            {
                return default; // Cannot castle through check
            }
        }

        // All castling conditions met
        var actions = new List<IAction>
        {
            new Castle(isKingside)
        };

        return new (piece, Position, kingDestination, actions);
    }

    /// <summary>
    /// Gets the squares the king passes through during castling (including origin and destination).
    /// </summary>
    private IEnumerable<Position> GetKingCastlingPath(Position destination)
    {
        // King moves from current position through intermediate square to destination
        var direction = destination.X > Position.X ? 1 : -1;

        yield return Position; // Start position (already checked for check above)
        yield return new Position((char)(Position.X + direction), Position.Y); // Middle square
        yield return destination; // End position
    }

    private IEnumerable<Position> GetSquaresBetween(Position start, Position end)
    {
        // For castling, we only move horizontally on the same rank
        var minX = Math.Min(start.X, end.X);
        var maxX = Math.Max(start.X, end.X);

        for (char x = (char)(minX + 1); x < maxX; x++)
        {
            yield return new Position(x, start.Y);
        }
    }

    /// <summary>
    /// Checks if the king's current position is under direct attack by enemy pieces.
    /// This method doesn't use BoardAnalysis to avoid infinite recursion.
    /// </summary>
    private bool IsUnderDirectAttack(Board board)
    {
        return IsSquareUnderAttack(board, Position);
    }

    /// <summary>
    /// Checks if a specific square is under attack by enemy pieces.
    /// This method manually checks each enemy piece's ability to attack the square
    /// without using BoardAnalysis to avoid infinite recursion during attack map building.
    /// </summary>
    private bool IsSquareUnderAttack(Board board, Position square)
    {
        var enemyColour = Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
        var enemyPieces = board.Pieces.Where(p => p.Colour == enemyColour);

        foreach (var enemyPiece in enemyPieces)
        {
            // Skip checking enemy king for castling purposes (avoid recursion)
            if (enemyPiece.IsKing)
            {
                // Manually check if enemy king can attack this square (1 square distance)
                var dx = Math.Abs(enemyPiece.Position.X - square.X);
                var dy = Math.Abs(enemyPiece.Position.Y - square.Y);
                if (dx <= 1 && dy <= 1 && !(dx == 0 && dy == 0))
                {
                    return true;
                }
                continue;
            }

            // For other pieces, check using CanMoveTo
            if (enemyPiece.CanMoveTo(board, square))
            {
                return true;
            }
        }

        return false;
    }
}
