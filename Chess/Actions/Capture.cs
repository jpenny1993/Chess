namespace Chess.Actions;

public sealed class Capture : IAction
{
    public PieceType Piece { get; }

    public Capture(PieceType piece)
    {
        Piece = piece;
    }
}