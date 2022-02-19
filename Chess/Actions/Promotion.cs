namespace Chess.Actions;

public sealed class Promotion : IAction
{
    public Position Position { get; }

    public PieceType Piece { get; }

    public Promotion(char x, int y, PieceType piece)
    {
        Position = new Position(x, y);
        Piece = piece;
    }
    
    public Promotion(int x, int y, PieceType piece)
    {
        Position = new Position(x, y);
        Piece = piece;
    }

    public Promotion(Position position, PieceType piece)
    {
        Position = position;
        Piece = piece;
    }
}