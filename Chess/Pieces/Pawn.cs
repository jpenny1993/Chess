namespace Chess.Pieces;

public sealed class Pawn : Piece
{
    private const PieceType ChessPiece = PieceType.Pawn;

    public Pawn(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
    
    public override IEnumerable<MoveAction> PossibleMoves(Board board)
    {
        // TODO: Takes are missing
        var y = Colour == PieceColour.White
            ? (Position.Y + 1)
            : (Position.Y - 1);

        if (Position.IsValid(Position.X, y))
        {
            return new[]
            {
                new MoveAction(Position.X, y)
            };
        }

        return Enumerable.Empty<MoveAction>();
    }
}