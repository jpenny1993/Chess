namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on control of the center squares.
/// Controlling the center provides spatial advantage and mobility.
/// Uses a weighted square system (D4/E4/D5/E5 most valuable).
/// </summary>
public class CenterControlStrategy : IEvaluationStrategy
{
    public string Name => "Center Control";

    // Center square bonus values (in centipawns)
    private static readonly Dictionary<Position, int> CenterBonuses = new()
    {
        // Primary center (D4, E4, D5, E5)
        { new Position('D', 4), 250 },
        { new Position('E', 4), 250 },
        { new Position('D', 5), 250 },
        { new Position('E', 5), 250 },

        // Extended center (C3-F6)
        { new Position('C', 3), 50 },
        { new Position('C', 4), 100 },
        { new Position('C', 5), 100 },
        { new Position('C', 6), 50 },

        { new Position('F', 3), 50 },
        { new Position('F', 4), 100 },
        { new Position('F', 5), 100 },
        { new Position('F', 6), 50 },

        { new Position('D', 3), 100 },
        { new Position('D', 6), 100 },
        { new Position('E', 3), 100 },
        { new Position('E', 6), 100 },
    };

    public int Evaluate(Board board, Movement movement)
    {
        int score = 0;

        // Bonus for moving piece to center square
        if (CenterBonuses.TryGetValue(movement.Destination, out int destBonus))
        {
            score += destBonus;
        }

        // Bonus for controlling center squares (piece can attack them)
        score += EvaluateCenterControl(board, movement.MovingPiece, movement.Destination);

        return score;
    }

    /// <summary>
    /// Evaluates bonus for attacking/controlling center squares from the new position.
    /// </summary>
    private int EvaluateCenterControl(Board board, Piece piece, Position source)
    {
        int score = 0;

        // Check each center square
        var primaryCenter = new[]
        {
            new Position('D', 4),
            new Position('E', 4),
            new Position('D', 5),
            new Position('E', 5)
        };

        foreach (var centerSquare in primaryCenter)
        {
            var pieceOnSquare = board.FindPiece(centerSquare);

            // Piece occupying center: already counted in destination bonus
            if (centerSquare.Equals(source))
            {
                continue;
            }

            // If empty or has enemy piece, check if we control it
            if (pieceOnSquare == null || pieceOnSquare.Colour != piece.Colour)
            {
                if (piece.CanMoveTo(board, centerSquare))
                {
                    // Pawn control is worth 75% of piece control
                    int controlBonus = piece.IsPawn ? 75 : 100;
                    score += controlBonus;
                }
            }
        }

        // Check extended center squares
        var extendedCenter = new[]
        {
            new Position('C', 3), new Position('C', 4), new Position('C', 5), new Position('C', 6),
            new Position('F', 3), new Position('F', 4), new Position('F', 5), new Position('F', 6),
            new Position('D', 3), new Position('D', 6), new Position('E', 3), new Position('E', 6)
        };

        foreach (var centerSquare in extendedCenter)
        {
            var pieceOnSquare = board.FindPiece(centerSquare);

            if (centerSquare.Equals(source))
            {
                continue;
            }

            if (pieceOnSquare == null || pieceOnSquare.Colour != piece.Colour)
            {
                if (piece.CanMoveTo(board, centerSquare))
                {
                    // Extended center control is worth less (50% of piece control)
                    int controlBonus = piece.IsPawn ? 25 : 50;
                    score += controlBonus;
                }
            }
        }

        return score;
    }
}
