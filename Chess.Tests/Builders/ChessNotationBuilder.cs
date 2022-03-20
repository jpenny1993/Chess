using System;
using System.Collections.Generic;
using Chess.Notation;

namespace Chess.Tests.Builders;

public sealed class ChessNotationBuilder
{
    private int _turnNumber;
    private readonly List<NotedTurn> _matchNotes = new();

    public ChessNotationBuilder Turn(Action<PlayerTurnBuilder> white, Action<PlayerTurnBuilder> black)
    {
        var whiteTurnBuilder = new PlayerTurnBuilder(PieceColour.White);
        white(whiteTurnBuilder);
        
        var blackTurnBuilder = new PlayerTurnBuilder(PieceColour.Black);
        black(blackTurnBuilder);

        var turn = new NotedTurn
        {
            TurnNumber = ++_turnNumber,
            WhitePlayerTurn = PlayerTurnBuilder.Build(whiteTurnBuilder),
            BlackPlayerTurn = PlayerTurnBuilder.Build(blackTurnBuilder)
        };
        _matchNotes.Add(turn);

        return this;
    }

    public IList<NotedTurn> Build() => _matchNotes;
}