using Chess.Strategies.Helpers;

namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on pawn structure.
/// Considers passed pawns, doubled pawns, isolated pawns, and advancement.
/// Pawn structure becomes increasingly important in endgames.
/// </summary>
public class PawnStructureStrategy : IEvaluationStrategy
{
    public string Name => "Pawn Structure";

    public int Evaluate(Board board, Movement movement)
    {
        int score = 0;

        // Only score pawn moves specially; other pieces get small bonus
        if (!movement.MovingPiece.IsPawn)
        {
            return 0; // Let other strategies evaluate non-pawn moves
        }

        var pawn = movement.MovingPiece;
        var destination = movement.Destination;

        // Check if moving pawn becomes passed
        if (PawnAnalyzer.IsPassedPawn(board, pawn, destination))
        {
            int advancement = PawnAnalyzer.GetPawnAdvancement(pawn);
            score += EvaluatePassedPawn(advancement);
        }

        // Check for doubled pawns after move
        if (PawnAnalyzer.IsDoubledPawn(board, pawn))
        {
            score -= 30; // Penalty for creating/maintaining doubled pawn
        }

        // Check for isolated pawns after move
        if (PawnAnalyzer.IsIsolatedPawn(board, pawn))
        {
            score -= 25; // Penalty for isolated pawn
        }

        // Bonus for pawn advancement
        int advancement_distance = PawnAnalyzer.GetPawnAdvancement(pawn);
        score += advancement_distance * 30;

        // Penalty for pawn islands (measured on whole position)
        int islands = PawnAnalyzer.CountPawnIslands(board, pawn.Colour);
        score -= (islands - 1) * 5; // -5 per island beyond the first

        return score;
    }

    /// <summary>
    /// Evaluates the value of a passed pawn based on how advanced it is.
    /// Values from Grandmaster guidance.
    /// </summary>
    private int EvaluatePassedPawn(int advancement)
    {
        return advancement switch
        {
            0 => 10,    // 2nd rank
            1 => 20,    // 3rd rank
            2 => 35,    // 4th rank
            3 => 60,    // 5th rank
            4 => 100,   // 6th rank
            5 => 180,   // 7th rank (about to promote)
            _ => 0
        };
    }
}
