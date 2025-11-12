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
    internal const string PlayerMove = $"((?:(?:{Castling})|(?:{NonPawnMovement})|(?:{PawnMovement}))\\+?#?)"; // Allow plus symbol for checks (attacks on king)
    public const string FullRound = $"{Whitespace}(?:{MoveNumber}{Period}{Whitespace})?(?<white>{PlayerMove}{MoveTimes})(?:{Whitespace})(?<black>{PlayerMove}?{MoveTimes})"; // Move number and period are optional; white and black moves are separated by whitespace; question mark allows final move to be white side's move without any subsequent black moves
}

public sealed partial class AlgebraicNotationReader
{
    private static readonly char[] CaptureAndCheckChars = ['x', 'X', '+', '#'];
    private static readonly char[] CastleChars = ['O', 'o', '0'];

    private int TurnNumber { get; set; }

    public NotedTurn ReadTurn(string chessTurn)
    {
        var match = FullRoundRegex().Match(chessTurn);

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

    /// <summary>
    /// Reads a white player's move in algebraic notation (e.g., "e4", "Nf3", "O-O")
    /// </summary>
    /// <param name="whiteMove">The white player's move in algebraic notation</param>
    /// <returns>A NotedPlayerTurn representing the white player's move</returns>
    /// <exception cref="AlgebraicNotationException">Thrown when the move notation is invalid</exception>
    public NotedPlayerTurn WhiteTurn(string whiteMove)
    {
        if (string.IsNullOrWhiteSpace(whiteMove))
        {
            throw AlgebraicNotationException.InvalidWhitePlayerMove;
        }

        var trimmedMove = whiteMove.Trim();

        if (!PlayerMoveRegex().IsMatch(trimmedMove))
        {
            throw AlgebraicNotationException.InvalidWhitePlayerMove;
        }

        return ReadPlayerTurn(PieceColour.White, trimmedMove);
    }

    /// <summary>
    /// Reads a black player's move in algebraic notation (e.g., "e5", "Nc6", "O-O-O")
    /// </summary>
    /// <param name="blackMove">The black player's move in algebraic notation</param>
    /// <returns>A NotedPlayerTurn representing the black player's move</returns>
    /// <exception cref="AlgebraicNotationException">Thrown when the move notation is invalid</exception>
    public NotedPlayerTurn BlackTurn(string blackMove)
    {
        if (string.IsNullOrWhiteSpace(blackMove))
        {
            throw AlgebraicNotationException.InvalidBlackPlayerMove;
        }

        var trimmedMove = blackMove.Trim();

        if (!PlayerMoveRegex().IsMatch(trimmedMove))
        {
            throw AlgebraicNotationException.InvalidBlackPlayerMove;
        }

        return ReadPlayerTurn(PieceColour.Black, trimmedMove);
    }

    private static NotedPlayerTurn ReadPlayerTurn(PieceColour colour, string playerMove)
    {
        var turn = new NotedPlayerTurn(colour)
        {
            IsCapture = playerMove.Contains('x', StringComparison.OrdinalIgnoreCase),
            IsCastling = CastlingRegex().IsMatch(playerMove),
            IsCheck = playerMove.Contains('+'),
            IsCheckmate = playerMove.Contains('#')
        };

        if (turn.IsCastling)
        {
            var count = playerMove.Count(c => CastleChars.Contains(c));
            turn.IsKingSide = count == 2; // 2 O's for kingside (O-O), 3 O's for queenside (O-O-O)
            var isWhite = colour == PieceColour.White;

            var y = isWhite ? 1 : 8;
            var kingX = turn.IsKingSide ? 'G' : 'C';
            var rookX = turn.IsKingSide ? 'F' : 'D';

            turn.Moves.Add(new (PieceType.King, new(kingX, y)));
            turn.Moves.Add(new (PieceType.Rook, new(rookX, y)));
        }
        else if (NonPawnMovementRegex().IsMatch(playerMove))
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
        else if (PawnMovementRegex().IsMatch(playerMove))
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

    [GeneratedRegex(AlgebraicPlayerMoveNotation.FullRound)]
    private static partial Regex FullRoundRegex();

    [GeneratedRegex("^" + AlgebraicPlayerMoveNotation.PlayerMove + "$")]
    private static partial Regex PlayerMoveRegex();

    [GeneratedRegex(AlgebraicPlayerMoveNotation.Castling)]
    private static partial Regex CastlingRegex();

    [GeneratedRegex(AlgebraicPlayerMoveNotation.NonPawnMovement)]
    private static partial Regex NonPawnMovementRegex();

    [GeneratedRegex(AlgebraicPlayerMoveNotation.PawnMovement)]
    private static partial Regex PawnMovementRegex();
}