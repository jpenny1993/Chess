namespace Chess.Pieces;

public sealed class Bishop : Piece
{
    private const PieceType ChessPiece = PieceType.Bishop;

    public Bishop(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
    
    public override IEnumerable<MoveAction> PossibleMoves(Board board)
    {
        var possibilities = new List<MoveAction>();

        var intersectedY1 = false;
        var intersectedY2 = false;
        
        // Iterate diagonally left through both Y positions
        for (var x = (Position.X - 1); x >= Position.MinX; x--)
        {
            var difference = Position.X - x;

            // Include only positions on the board
            var y1 = Position.Y - difference;
            if (!intersectedY1 && Position.IsValidY(y1))
            {
                // But only up to an intersecting piece
                var intersection = board.FindPiece(x, y1);
                intersectedY1 = intersection != null;
                if (intersection == null || intersection.Colour != Colour)
                {
                    possibilities.Add(new (x, y1));
                }
            }

            // Do the same for the second Y position
            var y2 = Position.Y + difference;
            if (!intersectedY2 && Position.IsValidY(y2))
            {
                var intersection = board.FindPiece(x, y2);
                intersectedY2 = intersection != null;
                if (intersection == null || intersection.Colour != Colour)
                {
                    possibilities.Add(new (x, y2));
                }
            }
        }
        
        intersectedY1 = false;
        intersectedY2 = false;
        
        // Iterate diagonally right through both Y positions
        for (var x = (Position.X + 1); x <= Position.MaxX; x++)
        {
            var difference = x - Position.X;

            var y1 = Position.Y - difference;
            if (!intersectedY1 && Position.IsValidY(y1))
            {
                var intersection = board.FindPiece(x, y1);
                intersectedY1 = intersection != null;
                if (intersection == null || intersection.Colour != Colour)
                {
                    possibilities.Add(new (x, y1));
                }
            }

            var y2 = Position.Y + difference;
            if (!intersectedY2 && Position.IsValidY(y2))
            {
                var intersection = board.FindPiece(x, y2);
                intersectedY2 = intersection != null;
                if (intersection == null || intersection.Colour != Colour)
                {
                    possibilities.Add(new (x, y2));
                }
            }
        }

        return possibilities;
    }
}