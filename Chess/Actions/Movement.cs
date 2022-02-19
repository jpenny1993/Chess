namespace Chess.Actions;

public sealed class Movement : IAction
{
    public Position Position { get; }

    public Movement(char x, int y)
    {
        Position = new Position(x, y);
    }
    
    public Movement(int x, int y)
    {
        Position = new Position(x, y);
    }

    public Movement(Position position)
    {
        Position = position;
    }
}