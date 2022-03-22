namespace Chess.Notation;

public sealed class NotedTurn
{
    public int TurnNumber { get; set; }

    public NotedPlayerTurn WhitePlayerTurn { get; set; } = null!;

    public NotedPlayerTurn BlackPlayerTurn { get; set; } = null!;

    public override string ToString()
    {
        if (WhitePlayerTurn.IsCheckmate)
        {
            return $"{TurnNumber}. {WhitePlayerTurn}";
        }

        return $"{TurnNumber}. {WhitePlayerTurn}. {BlackPlayerTurn}";
    }
}