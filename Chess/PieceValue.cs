namespace Chess;

/// <summary>
/// Provides standard chess piece values for material evaluation.
/// </summary>
public static class PieceValue
{
    /// <summary>
    /// Gets the standard material value of a piece type.
    /// Values are in centipawns (1 pawn = 100).
    /// </summary>
    public static int GetValue(PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.Pawn => 100,
            PieceType.Knight => 300,
            PieceType.Bishop => 300,
            PieceType.Rook => 500,
            PieceType.Queen => 900,
            PieceType.King => 10000, // King is invaluable
            _ => 0
        };
    }

    /// <summary>
    /// Gets the value of a piece.
    /// </summary>
    public static int GetValue(Piece piece)
    {
        return GetValue(piece.Type);
    }
}
