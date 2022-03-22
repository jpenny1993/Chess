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

    protected override Movement? GetMovement(Board board, TheoreticalPath path, Position step)
    {
        // TODO: check is edge of the board for pawn promotion
        // TODO: en passant rule for pawns

        var intersectingPiece = board.FindPiece(step);
        var isCapturingMovement = step.X != Position.X;
 
        if (intersectingPiece == default && isCapturingMovement ||
            intersectingPiece != default && !isCapturingMovement ||
            intersectingPiece != default && IsFriendly(intersectingPiece))
        {
            // Only allow captures of populated enemy tiles
            return default;
        }

        var actions = new List<IAction>();

        if (intersectingPiece != default && isCapturingMovement)
        {
            // Valid move, can capture enemies
            actions.Add(new Capture(intersectingPiece.Type));
        }

        if (IsWhite && Position.Y == Position.MaxY ||
            IsBlack && Position.Y == Position.MinY)
        {
            // Promote piece
            actions.Add(new Promotion(PieceType.Queen)); // TODO: get piece from player or notes
        }

        // Valid move, the tile is empty
        return new (Position, step, actions);
    }

    public override IEnumerable<TheoreticalPath> TheoreticalPaths()
    {
        const int moveSpeed = 1;
        const int moveSpeedAtStart = 2;
        var x = Position.X;
        var y = IsWhite ? Position.Y + moveSpeed : Position.Y - moveSpeed;
        var y2 = IsWhite ? Position.Y + moveSpeedAtStart : Position.Y - moveSpeedAtStart;
        
        // Default move
        if (Position.IsValid(x, y))
        {
            yield return new (x, y);
        }

        // Starting position only move
        var startingRowNumber = IsWhite ? (Position.MinY + 1) : (Position.MaxY - 1);
        if (Position.Y == startingRowNumber)
        {
            yield return new []
            {
                new Position(x, y),
                new Position(x, y2)
            };
        }

        // Captures
        var xLeft = x - moveSpeed;
        if (Position.IsValid(xLeft, y2))
        {
            yield return new (xLeft, y2);
        }
        
        var xRight = x + moveSpeed;
        if (Position.IsValid(xRight, y2))
        {
            yield return new (xRight, y2);
        }
    }
}