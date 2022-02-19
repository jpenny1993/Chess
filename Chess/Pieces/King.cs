using Chess.Actions;

namespace Chess.Pieces;

public sealed class King : Piece
{
    private const PieceType ChessPiece = PieceType.King;

    public King(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
    
    public override IEnumerable<IAction> PossibleMoves(Board board)
    {
        for (var x = (Position.X - 1); x <= (Position.X + 1); x++)
        for (var y = (Position.Y - 1); y <= (Position.Y + 1); y++)
        {
            if (Position.Equals(x, y))
            {
                continue;
            }

            if (Position.IsValid(x, y))
            {
                yield return new Movement(x, y);
            }
        }
    }
}