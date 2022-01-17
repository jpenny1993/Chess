namespace Chess.Pieces;

public sealed class Bishop : Piece
{
    private const PieceType ChessPiece = PieceType.Bishop;

    public Bishop(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
    
    public override IEnumerable<MoveAction> PossibleMoves()
    {
        var possibilities = new List<MoveAction>();

        for (var x = (Position.X - 1); x >= Position.MinX; x--)
        {
            var difference = Position.X - x;
            
            var y1 = Position.Y - difference;
            if (Position.IsValidY(y1))
            {
                possibilities.Add(new (x, y1));
            }

            var y2 = Position.Y + difference;
            if (Position.IsValidY(y2))
            {
                possibilities.Add(new (x, y2));
            }
        }
        
        for (var x = (Position.X + 1); x <= Position.MaxX; x++)
        {
            var difference = x - Position.X;

            var y1 = Position.Y - difference;
            if (Position.IsValidY(y1))
            {
                possibilities.Add(new (x, y1));
            }

            var y2 = Position.Y + difference;
            if (Position.IsValidY(y2))
            {
                possibilities.Add(new (x, y2));
            }
        }

        return possibilities;
    }
}