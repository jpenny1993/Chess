namespace Chess.Notation;

public sealed class NotedTurn
{
    public int TurnNumber { get; set; }

    public PlayerTurn WhitePlayerTurn { get; set; } = null!;

    public PlayerTurn BlackPlayerTurn { get; set; } = null!;

    public override string ToString()
    {
        if (WhitePlayerTurn.IsCheckmate)
        {
            return $"{TurnNumber}. {WhitePlayerTurn}";
        }

        return $"{TurnNumber}. {WhitePlayerTurn}. {BlackPlayerTurn}";
    }
}