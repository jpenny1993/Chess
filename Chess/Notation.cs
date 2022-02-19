using System.Text.RegularExpressions;
using Chess.Actions;

namespace Chess;

public class Notation
{
    private const string Whitespace = "\\s*"; // Whitespace
    private const string MoveNumber = "(\\d{1,3})"; // Move number between 1 and 999 will precede white side's move.
    private const string Period = "\\.?"; //Literal period, in case move numbers followed by a period. The replace pattern will restore period, so it is not captured.
    private const string Castling = "(?:[Oo0](-[Oo0]){1,2}?)"; // For castling kingside or queenside. Change the O to 0 to work for sites that 0-0 for castling notation
    private const string NonPawnMovement = "(?:[KQNBR][1-8a-h]?x?[a-h]x?[1-8])"; // For piece (non-pawn) moves and piece captures
    private const string PawnMovement = "(?:[a-h]x?[a-h]?[1-8]\\=?[QRNB]?)"; //Pawn moves, captures, and promotions
    private const string MoveTimes = "(?:\\s*\\d+\\.?\\d+?m?s)?"; // Skip over move-times; it is possible to retain move times if you make this a capturing group
    private const string PlayerMove = $"{Whitespace}((?:{Castling}|{NonPawnMovement}|{PawnMovement})\\+?)"; // Allow plus symbol for checks (attacks on king)
    private const string FullRound = $"{Whitespace}{MoveNumber}{Period}{PlayerMove}{MoveTimes}{Period}{PlayerMove}?{MoveTimes}"; // Question mark allows final move to be white side's move without any subsequent black moves

    private static readonly Regex RoundRegex = new (FullRound);
    private static readonly Regex CastlingRegex = new (Castling);
    private static readonly Regex NonPawnMovementRegex = new (NonPawnMovement);
    private static readonly Regex PawnMovementRegex = new (PawnMovement);

    public (IAction white, IAction? black) ReadRound(string chessMove)
    {
        var match = RoundRegex.Match(chessMove);

        if (!match.Success)
        {
            throw new ApplicationException("Not valid chess round");
        }

        var moveNoGroup = match.Groups[1]; // don't care
        
        var whiteMoveGroup = match.Groups[2];
        var whiteMove = whiteMoveGroup.Success
            ? ReadMove(whiteMoveGroup.Value)
            : throw new ApplicationException("Not valid white move");
        
        var blackMoveGroup = match.Groups[3];
        var blackMove = blackMoveGroup.Success
            ? ReadMove(blackMoveGroup.Value)
            : null;

        return (whiteMove, blackMove);
    }

    public IAction ReadMove(string playerMove)
    {
        if (NonPawnMovementRegex.IsMatch(playerMove))
        {
            // TODO [KQNBR][1-8a-h]?x?[a-h]x?[1-8]
            return playerMove.Contains('x', StringComparison.OrdinalIgnoreCase)
                ? ReadNonPawnCapture(playerMove)
                : ReadNonPawnMovement(playerMove);
        }
        
        if (PawnMovementRegex.IsMatch(playerMove))
        {
            // TODO [a-h]x?[a-h]?[1-8]\\=?[QRNB]
            return new Movement('A', 1);
        }
        
        if (CastlingRegex.IsMatch(playerMove))
        {
            // TODO [Oo0](-[Oo0]){1,2}
            return new Castle('A', 1, 'A', 1);
        }

        throw new ApplicationException("Invalid chess move");
    }

    private Actions.Capture ReadNonPawnCapture(string playerMove)
    {
        return new ('A', 1, PieceType.Pawn);
    }
    
    private Movement ReadNonPawnMovement(string playerMove)
    {
        return new ('A', 1);
    }
}