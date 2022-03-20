using System;
using Chess.Notation;

namespace Chess.Tests.Builders;

public sealed class PlayerTurnBuilder
{
    private readonly PieceColour _colour;
    private PlayerMove[] _moves = Array.Empty<PlayerMove>();
    
    private bool _isCapture;
    private bool _isCheck;
    private bool _isCheckmate;
    private bool _isKingSide;

    public PlayerTurnBuilder(PieceColour colour)
    {
        _colour = colour;
    }

    public PlayerTurnBuilder Capture()
    {
        _isCapture = true;
        return this;
    }
    
    public PlayerTurnBuilder Castle(bool isKingSide)
    {
        _isKingSide = isKingSide;

        Position kingPos;
        Position rookPos;
        if (_colour == PieceColour.White && isKingSide)
        {
            kingPos = "G1";
            rookPos = "F1";
        }
        else if (_colour == PieceColour.White) // queen-side
        {
            kingPos = "C1";
            rookPos = "D1";
        }
        else if (isKingSide) // Black king-side
        {
            kingPos = "G8";
            rookPos = "F8";
        }
        else // Black queen-side
        {
            kingPos = "C8";
            rookPos = "D8";
        }

        _moves = new[]
        {
            new PlayerMove(PieceType.King, kingPos), 
            new PlayerMove(PieceType.Rook, rookPos)
        };

        return this;
    }

    public PlayerTurnBuilder Check()
    {
        _isCheck = true;
        return this;
    }

    public PlayerTurnBuilder Checkmate()
    {
        _isCheckmate = true;
        return this;
    }

    public PlayerTurnBuilder Hint(string column)
    {
        _moves[0].Hint = column;
        return this;
    }

    public PlayerTurnBuilder Move(Position moveTo, PieceType piece)
    {
        _moves = new[] { new PlayerMove(piece, moveTo) };
        return this;
    }

    public static PlayerTurn Build(PlayerTurnBuilder builder)
    {
        var isCastling = builder._moves.Length == 2;
        var turn = new PlayerTurn(builder._colour)
        {
            IsCapture = builder._isCapture,
            IsCastling = isCastling,
            IsKingSide = isCastling && builder._isKingSide,
            IsCheck = builder._isCheck,
            IsCheckmate = builder._isCheckmate
        };

        foreach (var move in builder._moves)
        {
            turn.Moves.Add(move);
        }

        return turn;
    }
}