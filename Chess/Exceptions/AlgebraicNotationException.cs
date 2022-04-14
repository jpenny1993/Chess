namespace Chess.Exceptions;

public sealed class AlgebraicNotationException : DomainException
{
    private const string InvalidTurnMessage = "Not a valid chess turn";
    private const string InvalidColouredMessage = "{0} player's move is invalid";
    private const string PawnMovementMessage = "Unable to parse Pawn's destination";

    private AlgebraicNotationException(string messageTemplate)
        : base(messageTemplate)
    {
    }

    private AlgebraicNotationException(string messageTemplate, string arg)
        : base(string.Format(messageTemplate, arg))
    {
    }

    public static AlgebraicNotationException InvalidPosition => new (PawnMovementMessage);

    public static AlgebraicNotationException InvalidTurn => new (InvalidTurnMessage);

    public static AlgebraicNotationException InvalidBlackPlayerMove => new (InvalidColouredMessage, "Black");

    public static AlgebraicNotationException InvalidWhitePlayerMove => new (InvalidColouredMessage, "White");
}