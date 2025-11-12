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

    internal protected override Movement? GetMovement(Piece piece, Board board, TheoreticalPath path, Position step)
    {
        var intersectingPiece = board.FindPiece(step);
        var isCapturingMovement = step.X != Position.X;

        // Check for en passant
        if (intersectingPiece == default && isCapturingMovement)
        {
            var enPassantMove = GetEnPassantMovement(piece, board, step);
            if (enPassantMove != default)
            {
                return enPassantMove;
            }

            // Not a valid en passant, and no piece to capture
            return default;
        }

        if (intersectingPiece != default && !isCapturingMovement ||
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

        var isPromotionRank = (IsWhite && step.Y == Position.MaxY) ||
                              (IsBlack && step.Y == Position.MinY);

        if (isPromotionRank)
        {
            // Promote piece - default to Queen
            // Note: To support other promotions, the caller should use GetPromotionMovements() instead
            actions.Add(new Promotion(PieceType.Queen));
        }

        // Valid move, the tile is empty
        return new (piece, Position, step, actions);
    }

    /// <summary>
    /// Gets all possible promotion moves for a pawn reaching the promotion rank.
    /// Returns 4 movements for each promotion option: Queen, Rook, Bishop, Knight.
    /// </summary>
    public IEnumerable<Movement> GetPromotionMovements(Board board, Position destination)
    {
        // Verify this is actually a promotion move
        var isPromotionRank = (IsWhite && destination.Y == Position.MaxY) ||
                              (IsBlack && destination.Y == Position.MinY);

        if (!isPromotionRank)
        {
            yield break; // Not a promotion
        }

        // Check if this is a capture
        var intersectingPiece = board.FindPiece(destination);
        var isCapture = intersectingPiece != default && !IsFriendly(intersectingPiece);

        // Promotion options: Queen, Rook, Bishop, Knight
        var promotionPieces = new[] { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight };

        foreach (var pieceType in promotionPieces)
        {
            var actions = new List<IAction>();

            if (isCapture)
            {
                actions.Add(new Capture(intersectingPiece!.Type));
            }

            actions.Add(new Promotion(pieceType));

            yield return new Movement(this, Position, destination, actions);
        }
    }

    private Movement? GetEnPassantMovement(Piece piece, Board board, Position destination)
    {
        // En passant is only possible if:
        // 1. Last move exists
        // 2. Last move was by an enemy pawn
        // 3. Enemy pawn moved exactly 2 squares forward
        // 4. Current pawn is on the correct rank (5 for white, 4 for black)
        // 5. Current pawn is adjacent to the enemy pawn that just moved

        if (board.LastMove == default)
        {
            return default;
        }

        var lastMove = board.LastMove;
        var lastMovedPiece = board.FindPiece(lastMove.Destination);

        // Check if last moved piece is an enemy pawn
        if (lastMovedPiece == default || !lastMovedPiece.IsPawn || IsFriendly(lastMovedPiece))
        {
            return default;
        }

        // Check if enemy pawn moved exactly 2 squares forward
        var verticalDistance = Math.Abs(lastMove.Destination.Y - lastMove.Origin.Y);
        if (verticalDistance != 2)
        {
            return default;
        }

        // Check if current pawn is on the correct rank
        var correctRank = IsWhite ? 5 : 4;
        if (Position.Y != correctRank)
        {
            return default;
        }

        // Check if current pawn is adjacent to the enemy pawn
        var horizontalDistance = Math.Abs(Position.X - lastMove.Destination.X);
        if (horizontalDistance != 1)
        {
            return default;
        }

        // Check if the destination matches where the enemy pawn "passed through"
        // The passed-through square is between the origin and destination of the enemy pawn's move
        var passedThroughY = (lastMove.Origin.Y + lastMove.Destination.Y) / 2;
        var passedThroughPosition = new Position(lastMove.Destination.X, passedThroughY);

        if (!destination.Equals(passedThroughPosition))
        {
            return default;
        }

        // All conditions met - this is a valid en passant
        var actions = new List<IAction>
        {
            new EnPassant(lastMove.Destination)
        };

        return new Movement(piece, Position, destination, actions);
    }

    public override IEnumerable<TheoreticalPath> TheoreticalPaths()
    {
        const int moveSpeed = 1;
        const int moveSpeedAtStart = 2;
        var x = Position.X;
        var y = IsWhite ? Position.Y + moveSpeed : Position.Y - moveSpeed;
        var y2 = IsWhite ? Position.Y + moveSpeedAtStart : Position.Y - moveSpeedAtStart;

        // Check if at starting position
        var startingRowNumber = IsWhite ? (Position.MinY + 1) : (Position.MaxY - 1);
        var isAtStartingPosition = Position.Y == startingRowNumber;

        // Forward move(s)
        if (isAtStartingPosition && Position.IsValid(x, y2))
        {
            // Starting position: can move one or two squares forward
            yield return new []
            {
                new Position(x, y),
                new Position(x, y2)
            };
        }
        else if (Position.IsValid(x, y))
        {
            // Regular position: can only move one square forward
            yield return new (x, y);
        }

        // Captures
        var xLeft = x - moveSpeed;
        if (Position.IsValid(xLeft, y))
        {
            yield return new (xLeft, y);
        }

        var xRight = x + moveSpeed;
        if (Position.IsValid(xRight, y))
        {
            yield return new (xRight, y);
        }
    }
}