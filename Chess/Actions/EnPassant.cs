namespace Chess.Actions;

public sealed class EnPassant : IAction
{
    public EnPassant(Position capturedPawnPosition)
    {
        CapturedPawnPosition = capturedPawnPosition;
    }

    /// <summary>
    /// The position of the pawn that is captured via en passant.
    /// This is NOT the destination square, but the square where the enemy pawn currently sits.
    /// </summary>
    public Position CapturedPawnPosition { get; }
}
