namespace Chess;

public class MoveAction
{
    public char X { get; }

    public int Y { get; }

    public MoveAction(char x, int y)
    {
        X = x;
        Y = y;
    }
    
    public MoveAction(int x, int y)
    {
        X = (char)x;
        Y = y;
    }
}