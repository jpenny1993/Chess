namespace Chess.Pieces;

public sealed class Rook : Piece
{
    private const PieceType ChessPiece = PieceType.Rook;

    public Rook(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }

    public override IEnumerable<TheoreticalPath> TheoreticalPaths()
    {
        var left = Enumerable
            .Range(1, Position.X - Position.MinX)
            .Where(offset => Position.IsValidX(Position.X - offset))
            .Select(offset => new Position(Position.X - offset, Position.Y))
            .ToArray();

        var right = Enumerable
            .Range(Position.X + 1, Position.MaxX - Position.X)
            .Where(Position.IsValidX)
            .Select(x => new Position(x, Position.Y))
            .ToArray();

        var down = Enumerable
            .Range(1, Position.Y - Position.MinY)
            .Where(offset => Position.IsValidY(Position.Y - offset))
            .Select(offset => new Position(Position.X, Position.Y - offset))
            .ToArray();

        var up = Enumerable
            .Range(Position.Y + 1, Position.MaxY - Position.Y)
            .Where(Position.IsValidY)
            .Select(y => new Position(Position.X, y))
            .ToArray();

        return new TheoreticalPath[] { left, up, right, down };
    }
}