namespace Chess.Actions;

public sealed class Promotion : IAction
{
    public PieceType Piece { get; }

    public Promotion(PieceType piece)
    {
        Piece = piece;
    }
}