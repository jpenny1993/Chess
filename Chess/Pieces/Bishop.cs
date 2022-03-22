using Chess.Actions;

namespace Chess.Pieces;

public sealed class Bishop : Piece
{
    private const PieceType ChessPiece = PieceType.Bishop;

    public Bishop(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new(x, y);
    }

    public override IEnumerable<TheoreticalPath> TheoreticalPaths()
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

        return new TheoreticalPath[]
        {
            path1,
            path2,
            path3,
            path4
        };
    }
}