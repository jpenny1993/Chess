namespace Chess.Strategies;

/// <summary>
/// Evaluates moves based on king safety.
/// Protects the king in opening/middlegame, but in endgame evaluates king activity instead.
/// </summary>
public class KingSafetyStrategy : IEvaluationStrategy
{
    public string Name => "King Safety";

    // Endgame king centralization bonus table
    private static readonly int[,] KingEndgameBonus = new int[,]
    {
        // a   b    c    d    e    f    g   h
        {  0,  5,   5,   5,   5,   5,   5,  0 }, // rank 1
        {  5, 10,  15,  15,  15,  15,  10,  5 }, // rank 2
        {  5, 15,  25,  30,  30,  25,  15,  5 }, // rank 3
        {  5, 15,  30,  40,  40,  30,  15,  5 }, // rank 4
        {  5, 15,  30,  40,  40,  30,  15,  5 }, // rank 5
        {  5, 15,  25,  30,  30,  25,  15,  5 }, // rank 6
        {  5, 10,  15,  15,  15,  15,  10,  5 }, // rank 7
        {  0,  5,   5,   5,   5,   5,   5,  0 }  // rank 8
    };

    public int Evaluate(Board board, Movement movement)
    {
        var piece = movement.MovingPiece;
        int score = 0;

        // Detect if we're in endgame (queens off or low material)
        bool isEndgame = IsEndgame(board);

        if (isEndgame && piece.IsKing)
        {
            // In endgame: evaluate king activity and centralization
            score += EvaluateKingEndgameActivity(movement.Destination);
        }
        else if (!isEndgame)
        {
            // In opening/middlegame: evaluate king safety
            score += EvaluateKingSafety(board, movement);
        }

        return score;
    }

    /// <summary>
    /// Evaluates king safety in opening and middlegame.
    /// </summary>
    private int EvaluateKingSafety(Board board, Movement movement)
    {
        var piece = movement.MovingPiece;
        int score = 0;

        if (piece.IsKing)
        {
            // Castling is excellent for king safety
            if (movement.ToString().Contains("O-O"))
            {
                // Kingside castle slightly better than queenside
                score += movement.ToString() == "O-O" ? 400 : 350;
                return score;
            }

            // Check if king is moving to a safer or more exposed square
            var analysis = new BoardAnalysis(board);
            int attackersAfter = CountAttackers(board, movement.Destination, piece.Colour);

            if (attackersAfter == 0)
            {
                score += 300; // Safe square
            }
            else if (attackersAfter == 1)
            {
                score -= 200; // Under attack by one piece
            }
            else
            {
                score -= 500; // Under attack by multiple pieces (very dangerous)
            }
        }
        else
        {
            // Non-king moves: bonus for defending king
            var ownKing = board.Pieces.FirstOrDefault(p => p.IsKing && p.Colour == piece.Colour);
            if (ownKing != null && CanDefendKing(piece, movement.Destination, ownKing.Position))
            {
                score += 150;
            }

            // Penalty if this move exposes our king
            if (LeavesKingExposed(board, piece, movement.Destination))
            {
                score -= 200;
            }
        }

        return score;
    }

    /// <summary>
    /// Evaluates king position in the endgame (centralization).
    /// </summary>
    private int EvaluateKingEndgameActivity(Position destination)
    {
        // Map position to board coordinates (rank 0-7, file A-H = 0-7)
        int fileIndex = destination.X - 'A';
        int rankIndex = destination.Y - 1;

        // Ensure indices are within bounds
        if (fileIndex < 0 || fileIndex > 7 || rankIndex < 0 || rankIndex > 7)
        {
            return 0;
        }

        return KingEndgameBonus[rankIndex, fileIndex];
    }

    /// <summary>
    /// Determines if we're in an endgame position.
    /// </summary>
    private bool IsEndgame(Board board)
    {
        // Queens off or minimal material
        bool noQueens = !board.Pieces.Any(p => p.IsQueen);
        int materialValue = board.Pieces
            .Where(p => !p.IsKing && !p.IsPawn)
            .Sum(p => PieceValue.GetValue(p));

        return noQueens || materialValue < 26;
    }

    /// <summary>
    /// Counts how many enemy pieces attack a given square.
    /// </summary>
    private int CountAttackers(Board board, Position square, PieceColour defenderColor)
    {
        var enemyColor = defenderColor == PieceColour.White ? PieceColour.Black : PieceColour.White;
        int count = 0;

        foreach (var piece in board.Pieces.Where(p => p.Colour == enemyColor))
        {
            if (piece.CanMoveTo(board, square))
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Determines if a piece can defend the king from a given position.
    /// </summary>
    private bool CanDefendKing(Piece piece, Position newPosition, Position kingPosition)
    {
        // Check if piece at new position would protect king
        // Simple version: check if piece can move back to defend
        return Math.Abs(newPosition.X - kingPosition.X) <= 2 &&
               Math.Abs(newPosition.Y - kingPosition.Y) <= 2;
    }

    /// <summary>
    /// Determines if moving this piece would leave the king exposed to attacks.
    /// </summary>
    private bool LeavesKingExposed(Board board, Piece piece, Position newPosition)
    {
        // Find own king
        var ownKing = board.Pieces.FirstOrDefault(p => p.IsKing && p.Colour == piece.Colour);
        if (ownKing == null)
            return false;

        // If piece was defending the king and can't reach it anymore, it's exposed
        if (piece.CanMoveTo(board, ownKing.Position))
        {
            // Check if piece at new position still defends king
            return !CanDefendKing(piece, newPosition, ownKing.Position);
        }

        return false;
    }
}
