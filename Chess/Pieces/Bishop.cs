namespace Chess.Pieces;

public sealed class Bishop : Piece
{
    private const PieceType ChessPiece = PieceType.Bishop;

    public Bishop(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new(x, y);
    }

    public override bool CanMoveTo(Board board, Position position)
    {
        var pathsContainingDestination = TheoreticalPaths()
            .Where(path => path.Any(pos => pos.Equals(position)));
        
        foreach (var path in pathsContainingDestination)
        foreach (var step in path)
        {
            if (step.Equals(position))
            {
                return true;
            }

            // The path has been intersected by the current step
            var destination = board.FindPiece(step);
            if (destination != null)
            {
                break;
            }
        }

        return false;
    }

    public override IEnumerable<MoveAction> PossibleMoves(Board board)
    {
        foreach (var path in TheoreticalPaths())
        foreach (var position in path)
        {
            var intersectingPiece = board.FindPiece(position);
            if (intersectingPiece == null)
            {
                // Valid move, the tile is empty
                yield return new MoveAction(position.X, position.Y);
                continue;
            }

            // Invalid move, can't capture teammates
            if (IsFriendly(intersectingPiece))
            {
                break;
            }
            
            // Valid move, can capture enemies
            // TODO: replace with capture action
            yield return new MoveAction(position.X, position.Y);
            break;
        }
    }

    public override IEnumerable<IEnumerable<Position>> TheoreticalPaths()
    {
        var path1 = new List<Position>();
        var path2 = new List<Position>();
        var path3 = new List<Position>();
        var path4 = new List<Position>();
        
        // Iterate diagonally left through both Y positions
        for (var x = (Position.X - 1); x >= Position.MinX; x--)
        {
            var difference = Position.X - x;
            var y1 = Position.Y - difference;
            if (Position.IsValidY(y1))
            {
                path1.Add(new(x, y1));  
            }

            var y2 = Position.Y + difference;
            if (Position.IsValidY(y2))
            {
                path2.Add(new(x, y2));
            }
        }

        // Iterate diagonally right through both Y positions
        for (var x = (Position.X + 1); x <= Position.MaxX; x++)
        {
            var difference = x - Position.X;
            var y1 = Position.Y - difference;
            if (Position.IsValidY(y1))
            {
                path3.Add(new(x, y1));
            }

            var y2 = Position.Y + difference;
            if (Position.IsValidY(y2))
            {
                path4.Add(new(x, y2));
            }
        }

        return new[]
        {
            path1,
            path2,
            path3,
            path4
        };
    }
}