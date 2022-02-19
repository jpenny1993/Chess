namespace Chess.Actions;

public sealed class Capture : IAction
{
    public Position Position { get; }

    public PieceType Piece { get; }

    public Capture(char x, int y, PieceType piece)
    {
        Position = new Position(x, y);
        Piece = piece;
    }
    
    public Capture(int x, int y, PieceType piece)
    {
        Position = new Position(x, y);
        Piece = piece;
    }

    public Capture(Position position, PieceType piece)
    {
        Position = position;
        Piece = piece;
    }
}