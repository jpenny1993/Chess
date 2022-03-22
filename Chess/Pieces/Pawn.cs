using Chess.Actions;

namespace Chess.Pieces;

public sealed class Pawn : Piece
{
    private const PieceType ChessPiece = PieceType.Pawn;

    public Pawn(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
    
    public override IEnumerable<IEnumerable<Position>> TheoreticalPaths()
    {
        var x = Position.X;
        var y = IsWhite ? Position.Y + 1 : Position.Y - 1;
        var y2 = IsWhite ? Position.Y + 2 : Position.Y - 2;
        
        // Default move
        if (Position.IsValid(x, y))
        {
            yield return new[] { new Position(x, y) };
        }

        // Starting position only move
        if (IsWhite && Position.Y == 2 || IsBlack && Position.Y == 7)
        {
            yield return new[]
            {
                new Position(x, y),
                new Position(x, y2)
            };
        }

        // Captures
        var xLeft = x - 1;
        if (Position.IsValid(xLeft, y2))
        {
            yield return new[] { new Position(xLeft, y2) };
        }
        
        var xRight = x + 1;
        if (Position.IsValid(xRight, y2))
        {
            yield return new[] { new Position(xRight, y2) };
        }
    }
}