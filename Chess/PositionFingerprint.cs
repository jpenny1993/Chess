namespace Chess;

/// <summary>
/// Lightweight position identifier for opening book lookups.
/// Uses piece placement and castling rights to uniquely identify positions.
/// Sufficient for opening book usage (4-6 moves) where en passant and move clocks aren't critical.
/// </summary>
public sealed class PositionFingerprint : IEquatable<PositionFingerprint>
{
    private readonly string _hash;

    /// <summary>
    /// Creates a fingerprint from the current board position.
    /// </summary>
    public PositionFingerprint(Board board)
    {
        _hash = ComputeHash(board);
    }

    /// <summary>
    /// Computes a string hash representing the position.
    /// Format: "piece1|piece2|...|castlingRights"
    /// Example: "WPe2|WNf3|BPe7|...|WK|WQ|BK|BQ"
    /// Pieces are sorted by position for consistency.
    /// </summary>
    private static string ComputeHash(Board board)
    {
        var components = new List<string>();

        // Add all pieces sorted by position (Y ascending, then X ascending)
        var pieces = board.Pieces
            .Where(p => p != null)
            .OrderBy(p => p!.Position.Y)
            .ThenBy(p => p!.Position.X)
            .ToList();

        foreach (var piece in pieces)
        {
            if (piece == null)
                continue;

            var colorChar = piece.IsWhite ? 'W' : 'B';
            var typeChar = (char)piece.Type;
            var posStr = piece.Position.ToString();
            components.Add($"{colorChar}{typeChar}{posStr}");
        }

        // Add castling rights based on king and rook HasMoved flags
        var whiteKing = pieces.FirstOrDefault(p => p!.IsWhite && p.IsKing);
        var blackKing = pieces.FirstOrDefault(p => p!.IsBlack && p.IsKing);

        if (whiteKing != null && !whiteKing.HasMoved)
        {
            // Check kingside rook (H1)
            var whiteKingsideRook = board.FindPiece('H', 1);
            if (whiteKingsideRook?.IsRook == true && !whiteKingsideRook.HasMoved)
                components.Add("WK");

            // Check queenside rook (A1)
            var whiteQueensideRook = board.FindPiece('A', 1);
            if (whiteQueensideRook?.IsRook == true && !whiteQueensideRook.HasMoved)
                components.Add("WQ");
        }

        if (blackKing != null && !blackKing.HasMoved)
        {
            // Check kingside rook (H8)
            var blackKingsideRook = board.FindPiece('H', 8);
            if (blackKingsideRook?.IsRook == true && !blackKingsideRook.HasMoved)
                components.Add("BK");

            // Check queenside rook (A8)
            var blackQueensideRook = board.FindPiece('A', 8);
            if (blackQueensideRook?.IsRook == true && !blackQueensideRook.HasMoved)
                components.Add("BQ");
        }

        return string.Join("|", components);
    }

    /// <summary>
    /// Gets the hash string for this position.
    /// </summary>
    public override string ToString() => _hash;

    /// <summary>
    /// Determines equality based on position hash.
    /// </summary>
    public override bool Equals(object? obj) =>
        obj is PositionFingerprint other && Equals(other);

    /// <summary>
    /// Determines equality with another PositionFingerprint.
    /// </summary>
    public bool Equals(PositionFingerprint? other) =>
        other != null && _hash == other._hash;

    /// <summary>
    /// Returns the hash code for this position.
    /// </summary>
    public override int GetHashCode() => _hash.GetHashCode();
}
