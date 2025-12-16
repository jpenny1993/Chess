namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on material gain (capturing pieces).
/// Rewards capturing valuable pieces, penalizes risky captures.
/// </summary>
public class MaterialGainStrategy : IEvaluationStrategy
{
    public string Name => "Material Gain";

    public int Evaluate(Board board, Movement movement)
    {
        if (!movement.IsCapture)
        {
            return -10; // Small penalty for non-forcing moves
        }

        // Get the captured piece
        var capturedPiece = board.FindPiece(movement.Destination);
        if (capturedPiece == null)
        {
            return 0; // No piece to capture (shouldn't happen)
        }

        int capturedValue = PieceValue.GetValue(capturedPiece) * 100; // Convert to centipawns
        int movingPieceValue = PieceValue.GetValue(movement.MovingPiece) * 100;

        // Evaluate if the capture is safe
        if (movement.IsSafeCapture || !movement.IsDefended)
        {
            // Safe capture: full value of captured piece
            return capturedValue;
        }

        // Risky capture: evaluate material exchange
        // If captured piece is worth more than our piece, it's still good
        if (capturedValue >= movingPieceValue)
        {
            return capturedValue - (movingPieceValue * 80 / 100);
        }

        // Losing material in trade
        return capturedValue - movingPieceValue;
    }
}
