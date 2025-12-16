namespace Chess.Search;

/// <summary>
/// Represents a theoretical opening move from the opening book.
/// Includes algebraic notation and probability weight for selection.
/// </summary>
public sealed class OpeningMove
{
    /// <summary>
    /// The move in standard algebraic notation (e.g., "e4", "Nf3", "O-O").
    /// </summary>
    public required string AlgebraicNotation { get; init; }

    /// <summary>
    /// Relative weight for probabilistic selection (higher = more likely).
    /// Typical range: 100 (main line) to 10 (sideline).
    /// Used in weighted random selection algorithm.
    /// </summary>
    public required int Weight { get; init; }

    /// <summary>
    /// Optional name of the resulting position or variation.
    /// Example: "Ruy Lopez", "Italian Game", "Sicilian Defense"
    /// Useful for debugging and understanding book structure.
    /// </summary>
    public string? OpeningName { get; init; }

    /// <summary>
    /// Converts this opening move to an actual Movement on the given board.
    /// Uses AlgebraicNotationReader to parse the notation.
    /// </summary>
    /// <param name="board">The current board position</param>
    /// <param name="colour">The colour of the side to move</param>
    /// <returns>The Movement corresponding to this opening move</returns>
    /// <exception cref="InvalidOperationException">If the move cannot be found or is illegal</exception>
    public Movement ToMovement(Board board, PieceColour colour)
    {
        // Parse the algebraic notation manually to extract destination and piece type
        var notation = AlgebraicNotation;

        // Handle castling: O-O (kingside) or O-O-O (queenside)
        if (notation.Contains("O-O", StringComparison.OrdinalIgnoreCase) ||
            notation.Contains("0-0", StringComparison.OrdinalIgnoreCase))
        {
            // Find all possible moves for the king
            var allPossibleMoves = new List<Movement>();
            foreach (var piece in board.Pieces)
            {
                if (piece != null && piece.Colour == colour && piece.Type == PieceType.King)
                {
                    var pieceMoves = piece.PossibleMoves(board).ToList();
                    allPossibleMoves.AddRange(pieceMoves);
                }
            }

            // Find a castling move (return first one matching the notation)
            var castlingMove = allPossibleMoves.FirstOrDefault(m => m.IsCastling);
            if (castlingMove == null)
            {
                throw new InvalidOperationException($"Cannot find castling move for {notation}");
            }
            return castlingMove;
        }

        // Extract piece type from notation
        var pieceChar = notation[0];
        PieceType? pieceType = null;

        if (char.IsUpper(pieceChar))
        {
            pieceType = pieceChar switch
            {
                'K' => PieceType.King,
                'Q' => PieceType.Queen,
                'R' => PieceType.Rook,
                'B' => PieceType.Bishop,
                'N' => PieceType.Knight,
                _ => null
            };
        }

        // If no piece letter found, it's a pawn move
        if (pieceType == null)
            pieceType = PieceType.Pawn;

        // Extract destination square (last 2 characters before optional promotion/check/mate symbols)
        var cleanNotation = notation.TrimEnd('+', '#', '?', '!', '=');
        if (cleanNotation.Length < 2)
        {
            throw new InvalidOperationException($"Invalid notation: {notation}");
        }

        var destStr = cleanNotation.Substring(cleanNotation.Length - 2);
        Position destination;
        try
        {
            destination = (Position)destStr;
        }
        catch (InvalidCastException)
        {
            throw new InvalidOperationException($"Cannot parse destination from {notation}");
        }

        // Get all possible moves for the side to move
        var allMoves = new List<Movement>();
        foreach (var piece in board.Pieces.ToList())  // Create a copy to avoid collection modified exception
        {
            if (piece != null && piece.Colour == colour)
            {
                var pieceMoves = piece.PossibleMoves(board).ToList();
                allMoves.AddRange(pieceMoves);
            }
        }

        // Find the matching move by destination and piece type
        var movement = allMoves.FirstOrDefault(m =>
            m.Destination.Equals(destination) &&
            m.MovingPiece?.Type == pieceType);

        if (movement == null)
        {
            throw new InvalidOperationException(
                $"Opening book move '{AlgebraicNotation}' is not a legal move in current position");
        }

        return movement;
    }

    /// <summary>
    /// Returns a string representation of this opening move for debugging.
    /// </summary>
    public override string ToString() =>
        string.IsNullOrEmpty(OpeningName)
            ? $"{AlgebraicNotation} (weight: {Weight})"
            : $"{AlgebraicNotation} - {OpeningName} (weight: {Weight})";
}
