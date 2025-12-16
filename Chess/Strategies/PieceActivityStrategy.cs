namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on piece activity (mobility and control squares).
/// Active pieces control more squares and create more threats.
/// Multipliers based on Grandmaster guidance for ideal square control.
/// </summary>
public class PieceActivityStrategy : IEvaluationStrategy
{
    public string Name => "Piece Activity";

    public int Evaluate(Board board, Movement movement)
    {
        var piece = movement.MovingPiece;
        int score = 0;

        // Count squares the piece controls from destination
        int controlledSquares = CountControlledSquares(board, piece, movement.Destination);

        // Different pieces value mobility differently
        int activityScore = piece.Type switch
        {
            PieceType.Knight => controlledSquares * 50,      // Knights: ideal ~6-8 squares
            PieceType.Bishop => controlledSquares * 15,      // Bishops: ideal ~8-10 squares
            PieceType.Rook => controlledSquares * 10,        // Rooks: ideal ~14 on open file
            PieceType.Queen => controlledSquares * 5,        // Queens: mobility less critical
            PieceType.Pawn => 20,                             // Pawns: small consistent bonus
            PieceType.King => 10,                             // King: minimal activity bonus
            _ => 0,                                            // Unknown piece type (shouldn't happen)
        };

        score += activityScore;

        // Bonus for moving to central squares
        if (IsCentralSquare(movement.Destination))
        {
            score += 100;
        }

        // Penalty for moving to edge/corner
        if (IsEdgeSquare(movement.Destination))
        {
            score -= 50;
        }

        return score;
    }

    /// <summary>
    /// Counts the number of squares a piece can attack/control from a given position.
    /// </summary>
    private int CountControlledSquares(Board board, Piece piece, Position destination)
    {
        int count = 0;

        for (var x = 'A'; x <= 'H'; x++)
        {
            for (var y = 1; y <= 8; y++)
            {
                var targetSquare = new Position(x, y);

                // Skip the destination square itself
                if (targetSquare.Equals(destination))
                {
                    continue;
                }

                // Check if piece can attack this square
                // Create a temporary board state to avoid modifying the real board
                var targetPiece = board.FindPiece(targetSquare);

                // Piece can control a square if:
                // 1. It's empty
                // 2. It has an enemy piece
                if (targetPiece == null || targetPiece.Colour != piece.Colour)
                {
                    if (piece.CanMoveTo(board, targetSquare))
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Determines if a square is in the central area (D4, E4, D5, E5 and surroundings).
    /// </summary>
    private bool IsCentralSquare(Position position)
    {
        // Central squares: D4, E4, D5, E5
        return (position.X >= 'D' && position.X <= 'E') &&
               (position.Y >= 4 && position.Y <= 5);
    }

    /// <summary>
    /// Determines if a square is on the edge or corner of the board.
    /// </summary>
    private bool IsEdgeSquare(Position position)
    {
        return position.X == 'A' || position.X == 'H' ||
               position.Y == 1 || position.Y == 8;
    }
}
