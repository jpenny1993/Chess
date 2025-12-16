namespace Chess.Evaluation;

/// <summary>
/// Detects the current game phase (opening, middlegame, or endgame).
/// This determines which strategy weights should be applied.
/// </summary>
public static class GamePhaseDetector
{
    public enum GamePhase
    {
        Opening,
        Middlegame,
        Endgame
    }

    /// <summary>
    /// Determines the current game phase based on move count and material on board.
    /// </summary>
    public static GamePhase DetectPhase(Board board)
    {
        // Estimate move count from last move
        var moveCount = EstimateMoveCount(board);

        // Calculate total material (excluding kings and pawns)
        var materialValue = CalculateMaterialValue(board);

        // Phase detection rules (from Grandmaster guidance)
        if (moveCount < 12)
        {
            return GamePhase.Opening;
        }

        if (moveCount >= 35 || IsQueenlessPosition(board) || materialValue < 26)
        {
            return GamePhase.Endgame;
        }

        return GamePhase.Middlegame;
    }

    /// <summary>
    /// Estimates the number of moves made so far in the game.
    /// </summary>
    private static int EstimateMoveCount(Board board)
    {
        // Count pieces that have moved (HasMoved flag)
        var movedPieces = board.Pieces.Count(p => p.HasMoved);

        // Estimate: pieces that haven't moved = still on start, but some may have moved and returned
        // Use a heuristic based on board activity
        return board.LastMove?.Origin == default ? 0 : 10; // Placeholder - will improve with LastMove tracking
    }

    /// <summary>
    /// Calculates total material value (excluding kings and pawns).
    /// Used to detect when endgame has been reached.
    /// </summary>
    private static int CalculateMaterialValue(Board board)
    {
        int value = 0;
        foreach (var piece in board.Pieces)
        {
            if (!piece.IsKing && !piece.IsPawn)
            {
                value += PieceValue.GetValue(piece);
            }
        }
        return value;
    }

    /// <summary>
    /// Determines if the position is queens off (major endgame phase).
    /// </summary>
    private static bool IsQueenlessPosition(Board board)
    {
        return !board.Pieces.Any(p => p.IsQueen);
    }
}
