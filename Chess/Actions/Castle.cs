namespace Chess.Actions;

public class Castle : IAction
{
    public Position Position { get; }

    public Position RookPosition { get; }

    public Castle(char x, int y, char rookX, int rookY)
    {
        Position = new Position(x, y);
        RookPosition = new Position(rookX, rookY);
    }

    public Castle(int x, int y, char rookX, int rookY)
    {
        Position = new Position(x, y);
        RookPosition = new Position(rookX, rookY);
    }

    public Castle(Position position, Position rookPosition)
    {
        Position = position;
        RookPosition = rookPosition;
    }
}