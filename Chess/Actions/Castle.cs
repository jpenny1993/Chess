namespace Chess.Actions;

public sealed class Castle : IAction
{
    public Castle(bool isKingSide)
    {
        IsKingSide = isKingSide;
    }
    
    public bool IsKingSide { get; set; }
}