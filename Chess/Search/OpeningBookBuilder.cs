namespace Chess.Search;

/// <summary>
/// Fluent builder for constructing opening book data.
/// Simplifies adding positions and moves to the book using method chaining.
/// </summary>
internal sealed class OpeningBookBuilder
{
    private readonly Dictionary<PositionFingerprint, List<OpeningMove>> _book = new();
    private Board _currentBoard = new Board();

    /// <summary>
    /// Starts from the standard starting position.
    /// Resets the builder to initial state.
    /// </summary>
    public OpeningBookBuilder FromStartingPosition()
    {
        _currentBoard = new Board();
        return this;
    }

    /// <summary>
    /// Builds a position by applying the given moves from the starting position.
    /// Example: AfterMoves("e4", "e5", "Nf3")
    /// </summary>
    /// <param name="moves">Sequence of moves in algebraic notation</param>
    /// <returns>This builder for method chaining</returns>
    public OpeningBookBuilder AfterMoves(params string[] moves)
    {
        _currentBoard = new Board();
        var colour = PieceColour.White;

        foreach (var moveNotation in moves)
        {
            // Parse the move notation manually to extract destination and piece type
            var notation = moveNotation;

            // Handle castling
            if (notation.Contains("O-O", StringComparison.OrdinalIgnoreCase) ||
                notation.Contains("0-0", StringComparison.OrdinalIgnoreCase))
            {
                var allPossibleMoves = new List<Movement>();
                foreach (var piece in _currentBoard.Pieces)
                {
                    if (piece != null && piece.Colour == colour && piece.Type == PieceType.King)
                    {
                        var pieceMoves = piece.PossibleMoves(_currentBoard).ToList();
                        allPossibleMoves.AddRange(pieceMoves);
                    }
                }

                var castlingMove = allPossibleMoves.FirstOrDefault(m => m.IsCastling);
                if (castlingMove == null)
                {
                    throw new InvalidOperationException(
                        $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
                }

                _currentBoard.ApplyMovement(castlingMove);
                colour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
                continue;
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

            if (pieceType == null)
                pieceType = PieceType.Pawn;

            // Extract destination square
            var cleanNotation = notation.TrimEnd('+', '#', '?', '!', '=');
            if (cleanNotation.Length < 2)
            {
                throw new InvalidOperationException(
                    $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
            }

            var destStr = cleanNotation.Substring(cleanNotation.Length - 2);
            Position destination;
            try
            {
                destination = (Position)destStr;
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException(
                    $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
            }

            // Find all possible moves for the current side
            var allMoves = new List<Movement>();
            foreach (var piece in _currentBoard.Pieces.ToList())  // Create a copy to avoid collection modified exception
            {
                if (piece != null && piece.Colour == colour)
                {
                    var pieceMoves = piece.PossibleMoves(_currentBoard).ToList();
                    allMoves.AddRange(pieceMoves);
                }
            }

            // Find the move matching this notation
            var movement = allMoves.FirstOrDefault(m =>
                m.Destination.Equals(destination) &&
                m.MovingPiece?.Type == pieceType);

            if (movement == null)
            {
                throw new InvalidOperationException(
                    $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
            }

            // Apply the move to the board
            _currentBoard.ApplyMovement(movement);

            // Toggle to the other colour
            colour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
        }

        return this;
    }

    /// <summary>
    /// Adds a book move for the current position.
    /// </summary>
    /// <param name="notation">Move in algebraic notation</param>
    /// <param name="weight">Probability weight (100 = main line, 10+ = sideline)</param>
    /// <param name="openingName">Optional name of the resulting opening/variation</param>
    /// <returns>This builder for method chaining</returns>
    public OpeningBookBuilder AddMove(string notation, int weight, string? openingName = null)
    {
        var fingerprint = new PositionFingerprint(_currentBoard);

        if (!_book.ContainsKey(fingerprint))
        {
            _book[fingerprint] = new List<OpeningMove>();
        }

        _book[fingerprint].Add(new OpeningMove
        {
            AlgebraicNotation = notation,
            Weight = weight,
            OpeningName = openingName
        });

        return this;
    }

    /// <summary>
    /// Builds and returns the completed opening book dictionary.
    /// </summary>
    /// <returns>Dictionary mapping position fingerprints to weighted moves</returns>
    public Dictionary<PositionFingerprint, List<OpeningMove>> Build()
    {
        return _book;
    }
}
