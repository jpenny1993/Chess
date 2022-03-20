namespace Chess.Notation;

public sealed class PlayerMove
{
    public PlayerMove(PieceType piece, Position moveTo)
    {
        Piece = piece;
        MoveTo = moveTo;
        Hint = string.Empty;
    }

    public PieceType Piece { get; set; }

    public Position MoveTo { get; set; }

    public string Hint { get; set; }
}