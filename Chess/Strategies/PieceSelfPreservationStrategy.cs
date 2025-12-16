namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on piece safety and self-preservation.
/// Prevents blunders by penalizing moves that leave pieces undefended,
/// engage in bad trades, or move high-value pieces into danger.
/// </summary>
public class PieceSelfPreservationStrategy : IEvaluationStrategy
{
    public string Name => "Piece Self-Preservation";

    /// <summary>
    /// Evaluates a move based on how it affects piece safety.
    /// Penalizes hanging pieces, bad trades, and exposing high-value pieces.
    /// Bonuses rescuing threatened pieces.
    /// </summary>
    public int Evaluate(Board board, Movement movement)
    {
        // Don't penalize captures inherently - material gain strategy handles that
        // Instead, focus on non-capture moves that expose pieces
        var score = 0;

        // 1. Hanging Piece Detection: Penalize leaving own pieces undefended
        score += EvaluateMovingPieceSafety(board, movement);

        // 2. Exposed Ally Detection: Penalize moves that expose other friendly pieces
        score += EvaluateExposedAllies(board, movement);

        // 3. Rescue Bonus: Reward moving threatened pieces to safety
        if (IsThreatenedPiece(board, movement.MovingPiece, movement.Origin))
        {
            score += EvaluateRetreatSafety(board, movement);
        }

        // 4. High-Value Exposure Prevention: Penalize moving queen/rook into danger
        score += EvaluateHighValueExposure(board, movement);

        return score;
    }

    /// <summary>
    /// Evaluates if moving the piece leaves it hanging (undefended after move).
    /// </summary>
    private int EvaluateMovingPieceSafety(Board board, Movement movement)
    {
        // Simulate the move to check piece safety
        var piece = movement.MovingPiece;
        var originalPosition = piece.Position;
        var originalHasMoved = piece.HasMoved;

        // Simulate move
        var capturedPiece = board.RemovePiece(movement.Destination);
        piece.Position = movement.Destination;
        piece.HasMoved = true;

        // Evaluate safety at new position
        var analysis = new BoardAnalysis(board);
        var enemyColor = piece.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

        int attackerCount = analysis.CountAttackers(movement.Destination, enemyColor);
        int defenderCount = analysis.CountAttackers(movement.Destination, piece.Colour);

        // Undo move
        piece.Position = originalPosition;
        piece.HasMoved = originalHasMoved;
        if (capturedPiece != null)
        {
            board.AddPiece(capturedPiece);
        }

        // If no attackers, piece is safe
        if (attackerCount == 0)
            return 0;

        // If adequately defended, piece is safe
        if (defenderCount > attackerCount)
            return 0;

        // If captured piece, don't penalize (material gain strategy handles this)
        if (movement.IsCapture)
            return 0;

        // Piece becomes hanging: penalize heavily
        int pieceValue = PieceValue.GetValue(piece);
        if (defenderCount == 0)
        {
            // Completely undefended: heavy penalty
            return -pieceValue;
        }

        // Underdefended: moderate penalty
        return -(pieceValue / 2);
    }

    /// <summary>
    /// Evaluates if moving a piece exposes other friendly pieces.
    /// E.g., removing a defender causes another piece to become hanging.
    /// </summary>
    private int EvaluateExposedAllies(Board board, Movement movement)
    {
        // Simulate the move
        var piece = movement.MovingPiece;
        var originalPosition = piece.Position;
        var originalHasMoved = piece.HasMoved;

        var capturedPiece = board.RemovePiece(movement.Destination);
        piece.Position = movement.Destination;
        piece.HasMoved = true;

        var penalty = 0;
        var analysis = new BoardAnalysis(board);
        var enemyColor = piece.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

        // Check each friendly piece to see if it became less safe
        foreach (var allyPiece in board.Pieces.Where(p => p.Colour == piece.Colour && !p.Equals(piece)))
        {
            int newAttackerCount = analysis.CountAttackers(allyPiece.Position, enemyColor);
            int newDefenderCount = analysis.CountAttackers(allyPiece.Position, piece.Colour);

            // If this ally is now hanging, penalize
            if (newAttackerCount > 0 && newDefenderCount == 0)
            {
                penalty -= PieceValue.GetValue(allyPiece) / 2; // Moderate penalty
            }
        }

        // Undo move
        piece.Position = originalPosition;
        piece.HasMoved = originalHasMoved;
        if (capturedPiece != null)
        {
            board.AddPiece(capturedPiece);
        }

        return penalty;
    }

    /// <summary>
    /// Evaluates if a threatened piece is being moved to safety (rescue).
    /// </summary>
    private int EvaluateRetreatSafety(Board board, Movement movement)
    {
        var piece = movement.MovingPiece;
        var destination = movement.Destination;
        var analysis = new BoardAnalysis(board);
        var enemyColor = piece.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

        int attackersAtDestination = analysis.CountAttackers(destination, enemyColor);

        // If retreat still under attack, penalize
        if (attackersAtDestination > 0)
            return -PieceValue.GetValue(piece) / 4; // Modest penalty for unsafe retreat

        // Successful rescue: bonus based on piece value
        return (PieceValue.GetValue(piece) * 30) / 100; // 30% of piece value as bonus
    }

    /// <summary>
    /// Evaluates exposure of high-value pieces (queen, rook) to lower-value attackers.
    /// </summary>
    private int EvaluateHighValueExposure(Board board, Movement movement)
    {
        var piece = movement.MovingPiece;
        var destination = movement.Destination;

        // Only penalize for high-value pieces
        if (piece.Type != PieceType.Queen && piece.Type != PieceType.Rook)
            return 0;

        var analysis = new BoardAnalysis(board);
        var enemyColor = piece.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

        var attackers = analysis.GetAttackers(destination, enemyColor).ToList();
        if (!attackers.Any())
            return 0;

        int lowestAttackerValue = attackers.Min(a => PieceValue.GetValue(a));
        int pieceValue = PieceValue.GetValue(piece);

        // If attacked by much lower-value piece, penalize heavily
        if (lowestAttackerValue < pieceValue)
        {
            int valueDifference = pieceValue - lowestAttackerValue;
            return -valueDifference; // Strong penalty for bad positional trades
        }

        return 0;
    }

    /// <summary>
    /// Determines if a piece is currently threatened (attacked and not adequately defended).
    /// </summary>
    private bool IsThreatenedPiece(Board board, Piece piece, Position position)
    {
        var analysis = new BoardAnalysis(board);
        var enemyColor = piece.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

        int attackerCount = analysis.CountAttackers(position, enemyColor);
        int defenderCount = analysis.CountAttackers(position, piece.Colour);

        // Threatened if attacked and not adequately defended
        return attackerCount > 0 && defenderCount <= attackerCount;
    }
}
