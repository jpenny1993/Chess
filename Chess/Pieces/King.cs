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
    
    public override IEnumerable<IEnumerable<Position>> TheoreticalPaths()
    {
        const int moveSpeed = 1;
        for (var x = (Position.X - moveSpeed); x <= (Position.X + moveSpeed); x++)
        for (var y = (Position.Y - moveSpeed); y <= (Position.Y + moveSpeed); y++)
        {
            if (Position.Equals(x, y))
            {
                continue;
            }

            if (Position.IsValid(x, y))
            {
                yield return new [] { new Position(x, y) };
            }
        }
    }
}