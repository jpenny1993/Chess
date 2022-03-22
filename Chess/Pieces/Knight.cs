namespace Chess.Pieces;

public sealed class Knight : Piece
{
    private const PieceType ChessPiece = PieceType.Knight;

    public Knight(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
    
    public override IEnumerable<TheoreticalPath> TheoreticalPaths()
    {
        const int range = 2;
        const int offset = 1;

        // Left
        var x = Position.X - range;
        var y = Position.Y - offset;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }
        
        y = Position.Y + offset;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }
        
        // Up
        x = Position.X - offset;
        y = Position.Y + range;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }
        
        x = Position.X + offset;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }

        // Right
        x = Position.X + range;
        y = Position.Y + offset;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }
        
        y = Position.Y - offset;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }

        // Down
        x = Position.X + offset;
        y = Position.Y - range;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }
        
        x = Position.X - offset;
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }
    }
}