namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on piece development in the opening.
/// Development is crucial early but irrelevant later in the game.
/// </summary>
public class PieceDevelopmentStrategy : IEvaluationStrategy
{
    public string Name => "Piece Development";

    public int Evaluate(Board board, Movement movement)
    {
        // Development only matters in opening (first 12 moves)
        var moveCount = EstimateMoveCount(board);
        if (moveCount > 12)
        {
            return 0; // Development bonuses end after move 12
        }

        int score = 0;
        var piece = movement.MovingPiece;

        // Bonus for developing undeveloped pieces
        if (!piece.HasMoved && !piece.IsKing && !piece.IsPawn)
        {
            score += 200; // Developing a new piece is good

            // Extra bonus for moving toward center
            if (MovesTowardCenter(piece.Position, movement.Destination))
            {
                score += 100;
            }
        }

        // Penalty for moving same piece multiple times early
        if (piece.HasMoved && moveCount < 6)
        {
            score -= 50; // Don't waste time moving same piece twice
        }

        // Bonus for king safety in opening
        if (piece.IsKing && movement.ToString().Contains("O-O"))
        {
            score += 300; // Castling is excellent development move
        }

        // Bonus for developing both knights
        if (AreAllMinorPiecesDeveloped(board, piece.Colour))
        {
            score += 200; // Big bonus for completing minor piece development
        }

        // Bonus for connecting rooks
        if (AreRooksConnected(board, piece.Colour))
        {
            score += 100;
        }

        return score;
    }

    /// <summary>
    /// Estimates move count from the board state.
    /// </summary>
    private int EstimateMoveCount(Board board)
    {
        // Count pieces that haven't moved yet
        var unmoved = board.Pieces.Count(p => !p.HasMoved && !p.IsKing);

        // Start with 16 pieces, subtract unmoved, rough estimate
        int movedCount = 16 - unmoved;
        return movedCount / 2; // Rough conversion to plies
    }

    /// <summary>
    /// Determines if a move goes toward the center.
    /// </summary>
    private bool MovesTowardCenter(Position from, Position to)
    {
        var centerX = (from.X + to.X) / 2.0;
        var centerY = (from.Y + to.Y) / 2.0;

        // Moving toward center if destination is closer to center than origin
        return Math.Abs(to.X - 'D' + 'E') < Math.Abs(from.X - 'D' + 'E') ||
               Math.Abs(to.Y - 4 - 5) < Math.Abs(from.Y - 4 - 5);
    }

    /// <summary>
    /// Checks if all minor pieces (knights and bishops) have been developed.
    /// </summary>
    private bool AreAllMinorPiecesDeveloped(Board board, PieceColour color)
    {
        var minorPieces = board.Pieces.Where(p =>
            (p.IsKnight || (p.IsBishop)) && p.Colour == color);

        return minorPieces.All(p => p.HasMoved || p.Position.Y == (color == PieceColour.White ? 1 : 8));
    }

    /// <summary>
    /// Checks if rooks are connected (no pieces between them on back rank).
    /// </summary>
    private bool AreRooksConnected(Board board, PieceColour color)
    {
        var rank = color == PieceColour.White ? 1 : 8;
        var rooks = board.Pieces.Where(p => p.IsRook && p.Colour == color && p.Position.Y == rank).ToList();

        if (rooks.Count < 2)
            return false;

        // Check if pieces between rooks are all developed (moved)
        var betweenRooks = board.Pieces.Where(p =>
            p.Colour == color && p.Position.Y == rank &&
            p.Position.X > Math.Min(rooks[0].Position.X, rooks[1].Position.X) &&
            p.Position.X < Math.Max(rooks[0].Position.X, rooks[1].Position.X));

        return betweenRooks.All(p => p.HasMoved || p.IsKing);
    }
}
