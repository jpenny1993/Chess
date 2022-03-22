namespace Chess.Notation;

public sealed class NotedPlayerMove
{
    public NotedPlayerMove(PieceType piece, Position destination)
    {
        Piece = piece;
        Destination = destination;
        Hint = string.Empty;
    }

    public PieceType Piece { get; set; }

    public Position Destination { get; set; }

    public string Hint { get; set; }
}