namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on forcing sequences (checks and checkmates).
/// Checkmate is the ultimate goal, checks restrict opponent options.
/// </summary>
public class CheckmateStrategy : IEvaluationStrategy
{
    public string Name => "Checkmate/Check";

    public int Evaluate(Board board, Movement movement)
    {
        // Checkmate is an instant win
        if (movement.IsCheckmate)
        {
            return 100000;
        }

        if (!movement.IsCheck)
        {
            return 0; // Not a forcing move
        }

        // This is a check - evaluate how restrictive it is
        // Count opponent's escape moves after this check
        var opponentColor = movement.MovingPiece.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
        int escapeMoves = CountEscapeMoves(board, opponentColor, movement);

        // Score based on severity of restriction
        int score = escapeMoves switch
        {
            0 => 8000,   // Severe restriction (only 1 square)
            1 => 6000,   // Moderate-high restriction (2-3 squares)
            2 => 3000,   // Moderate restriction (3-4 squares)
            3 => 1500,   // Mild restriction (4-5 squares)
            _ => 500     // Light restriction (many escapes)
        };

        // Extra bonus if check is also a capture
        if (movement.IsCapture)
        {
            score += 1000;
        }

        return score;
    }

    /// <summary>
    /// Estimates the number of legal escape moves the opponent has after this check.
    /// This is a simplified count - actual legal move validation would be more accurate.
    /// </summary>
    private int CountEscapeMoves(Board board, PieceColour opponentColor, Movement movement)
    {
        // Find the opponent's king
        var opponentKing = board.Pieces.FirstOrDefault(p => p.IsKing && p.Colour == opponentColor);
        if (opponentKing == null)
        {
            return 0;
        }

        // Count squares the king can potentially move to
        int escapeSquares = 0;
        for (var x = 'A'; x <= 'H'; x++)
        {
            for (var y = 1; y <= 8; y++)
            {
                var position = new Position(x, y);

                // Check if this square is empty or has an enemy piece
                var pieceAtSquare = board.FindPiece(position);
                if (pieceAtSquare != null && pieceAtSquare.Colour == opponentColor)
                {
                    continue; // Can't move to square occupied by own piece
                }

                // Check if king can move to this square
                if (opponentKing.CanMoveTo(board, position))
                {
                    escapeSquares++;
                }
            }
        }

        return escapeSquares;
    }
}
