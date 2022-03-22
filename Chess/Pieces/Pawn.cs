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
        const int moveSpeed = 1;
        const int moveSpeedAtStart = 2;
        var x = Position.X;
        var y = IsWhite ? Position.Y + moveSpeed : Position.Y - moveSpeed;
        var y2 = IsWhite ? Position.Y + moveSpeedAtStart : Position.Y - moveSpeedAtStart;
        var startingRowNumber = IsWhite ? 2 : 7;
        
        // Default move
        if (Position.IsValid(x, y))
        {
            yield return new[] { new Position(x, y) };
        }

        // Starting position only move
        if (Position.Y == startingRowNumber)
        {
            yield return new[]
            {
                new Position(x, y),
                new Position(x, y2)
            };
        }

        // Captures
        var xLeft = x - moveSpeed;
        if (Position.IsValid(xLeft, y2))
        {
            yield return new[] { new Position(xLeft, y2) };
        }
        
        var xRight = x + moveSpeed;
        if (Position.IsValid(xRight, y2))
        {
            yield return new[] { new Position(xRight, y2) };
        }
    }
}