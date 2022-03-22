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
            .Range(0, Position.X - Position.MinX)
            .Where(offset => Position.IsValidX(Position.X - offset))
            .Select(offset => new Position(Position.X - offset, Position.Y))
            .ToArray();
        
        var right = Enumerable
            .Range(Position.X, Position.MaxX - Position.X)
            .Where(Position.IsValidX)
            .Select(x => new Position(x, Position.Y))
            .ToArray();
        
        var down = Enumerable
            .Range(0, Position.Y)
            .Where(offset => Position.IsValidY(Position.Y - offset))
            .Select(offset => new Position(Position.X, Position.Y - offset))
            .ToArray();
        
        var up = Enumerable
            .Range(Position.Y, Position.MaxY - Position.Y)
            .Where(Position.IsValidY)
            .Select(y => new Position(Position.X, y))
            .ToArray();
        
        return new TheoreticalPath[] { left, up, right, down };
    }
}