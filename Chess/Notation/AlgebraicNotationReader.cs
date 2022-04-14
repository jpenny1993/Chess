using System.Text.RegularExpressions;
using Chess.Exceptions;

namespace Chess.Notation;

internal static class AlgebraicPlayerMoveNotation
{
    private const string MoveNumber = "(?<turn>\\d{1,3})"; // Move number between 1 and 999 will precede white side's move.
    private const string Period = "\\.?"; //Literal period, in case move numbers followed by a period. The replace pattern will restore period, so it is not captured.
    private const string Whitespace = "\\s*"; // Whitespace
    public const string Castling = "[Oo0](-[Oo0]){1,2}"; // For castling kingside or queenside. Change the O to 0 to work for sites that 0-0 for castling notation
    public const string NonPawnMovement = "[KQNBR][1-8a-hA-H]?x?[a-hA-H]x?[1-8]"; // For piece (non-pawn) moves and piece captures
    public const string PawnMovement = "[a-hA-H]x?[a-hA-H]?[1-8](\\=[QRNB])?"; //Pawn moves, captures, and promotions
    private const string MoveTimes = "(\\s*\\d+\\.?\\d+?m?s)?"; // Skip over move-times; it is possible to retain move times if you make this a capturing group
    private const string PlayerMove = $"{Whitespace}((?:(?:{Castling}?)|(?:{NonPawnMovement})|(?:{PawnMovement}))\\+?#?)"; // Allow plus symbol for checks (attacks on king)
    public const string FullRound = $"{Whitespace}{MoveNumber}{Period}(?<white>{PlayerMove}{MoveTimes}){Period}(?<black>{PlayerMove}?{MoveTimes})"; // Question mark allows final move to be white side's move without any subsequent black moves
}

public sealed class AlgebraicNotationReader
{
    private static readonly Regex RoundRegex = new (AlgebraicPlayerMoveNotation.FullRound);
    private static readonly Regex CastlingRegex = new (AlgebraicPlayerMoveNotation.Castling);
    private static readonly Regex NonPawnMovementRegex = new (AlgebraicPlayerMoveNotation.NonPawnMovement);
    private static readonly Regex PawnMovementRegex = new (AlgebraicPlayerMoveNotation.PawnMovement);
    
    private static readonly char[] CaptureAndCheckChars = new[] { 'x', 'X', '+', '#' };
    private static readonly char[] CastleChars = new[] { 'O', 'o', '0' };

    private int TurnNumber { get; set; }

    public NotedTurn ReadTurn(string chessTurn)
    {
        var match = RoundRegex.Match(chessTurn);

        if (!match.Success)
        {
            throw AlgebraicNotationException.InvalidTurn;
        }

        var moveNoGroup = match.Groups["turn"];
        var turnNumber = moveNoGroup.Success
            ? (int.TryParse(moveNoGroup.Value, out var value) ? value : 0)
            : 0;

        if (turnNumber != ++TurnNumber) // Unable to parse number, or not matching auto increment
        {
            turnNumber = TurnNumber;
        }

        var whiteMoveGroup = match.Groups["white"];
        var whiteMove = whiteMoveGroup.Success
            ? ReadPlayerTurn(PieceColour.White, whiteMoveGroup.Value)
            : throw AlgebraicNotationException.InvalidWhitePlayerMove;
        
        var blackMoveGroup = match.Groups["black"];
        var blackMove = blackMoveGroup.Success
            ? ReadPlayerTurn(PieceColour.Black, blackMoveGroup.Value)
            : whiteMove.IsCheckmate
                ? new(PieceColour.Black)
                : throw AlgebraicNotationException.InvalidBlackPlayerMove; 

        return new()
        {
            TurnNumber = turnNumber,
            WhitePlayerTurn = whiteMove,
            BlackPlayerTurn = blackMove
        };
    }

    private static NotedPlayerTurn ReadPlayerTurn(PieceColour colour, string playerMove)
    {
        var turn = new NotedPlayerTurn(colour)
        {
            IsCapture = playerMove.Contains('x', StringComparison.OrdinalIgnoreCase),
            IsCastling = CastlingRegex.IsMatch(playerMove),
            IsCheck = playerMove.Contains('+'),
            IsCheckmate = playerMove.Contains('#')
        };

        if (turn.IsCastling)
        {
            var count = playerMove.Count(c => CastleChars.Contains(c));
            turn.IsKingSide = count == 2;
            var isWhite = colour == PieceColour.White;

            var y = isWhite ? 1 : 8;
            var kingX = turn.IsKingSide ? 'G' : 'C';
            var rookX = turn.IsKingSide ? 'F' : 'D';

            turn.Moves.Add(new (PieceType.King, new(kingX, y)));
            turn.Moves.Add(new (PieceType.Rook, new(rookX, y)));
        }
        else if (NonPawnMovementRegex.IsMatch(playerMove))
        {
            var piece = (PieceType)playerMove[0];
            var parts = playerMove[1..].Split(CaptureAndCheckChars, StringSplitOptions.RemoveEmptyEntries);
            string hint;
            string position;

            if (parts.Length == 1 && parts[0].Length == 2)
            {
                hint = string.Empty;
                position = parts[0];
            }
            else if (parts.Length == 1 && parts[0].Length > 2)
            {
                hint = parts[0][..1];
                position = parts[0][1..];
            }
            else if (parts.Length > 1)
            {
                hint = parts[0];
                position = parts[1];
            }
            else
            {
                throw AlgebraicNotationException.InvalidPosition;
            }

            turn.Moves.Add(new (piece, position)
            {
                Hint = hint.ToUpperInvariant()
            });
        }
        else if (PawnMovementRegex.IsMatch(playerMove))
        {
            const PieceType piece = PieceType.Pawn;
            NotedPlayerMove movement = !turn.IsCapture
                ? new (piece, playerMove[..2])
                : new (piece, playerMove[2..4])
                {
                    Hint = playerMove[..1].ToUpperInvariant()
                };
            
            turn.Moves.Add(movement);

            var promotionIdentifier = playerMove.IndexOf('=');
            if (promotionIdentifier > -1)
            {
                turn.Promotion = (PieceType)playerMove[promotionIdentifier + 1];
            }
        }

        return turn;
    }
}