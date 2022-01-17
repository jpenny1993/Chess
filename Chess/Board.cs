using Chess.Pieces;

namespace Chess;

public class Board
{
    private readonly List<Piece> _pieces = Enumerable.Empty<Piece>()
        .Concat(CreateTeam1(PieceColour.White))
        .Concat(CreateTeam2(PieceColour.Black))
        .ToList();

    public IEnumerable<Piece> Pieces => _pieces;

    public Piece? FindPiece(char x, int y)
    {
        return _pieces.FirstOrDefault(p => p.Position.Equals(x, y));
    }

    public static bool IsBlackTile(char x, int y)
    {
        const int zeroChar = 'A' - 1;
        var xIsEven = (x - zeroChar) % 2 == 0;
        var yIsEven =  y % 2 == 0;
        return (xIsEven && yIsEven) || (!xIsEven && !yIsEven);
    }

    public static implicit operator Board(char[] tiles)
    {
        if (tiles.Length != 64)
        {
            throw new ArgumentException("Invalid tile set. Expecting 64 tiles, the chessboard is 8 x 8.", nameof(tiles));
        }

        var board = new Board();
        board._pieces.Clear();

        var x = Position.MinX;
        var y = Position.MaxY;
        PieceColour colour;

        foreach (var tile in tiles)
        {
            colour = y > 4 ? PieceColour.Black : PieceColour.White;
            
            switch (tile)
            {
                case (char)PieceType.Bishop:
                    board._pieces.Add(new Bishop(colour, x, y));
                    break;
                case (char)PieceType.King:
                    board._pieces.Add(new King(colour, x, y));
                    break;
                case (char)PieceType.Knight:
                    board._pieces.Add(new Knight(colour, x, y));
                    break;
                case (char)PieceType.Pawn:
                    board._pieces.Add(new Pawn(colour, x, y));
                    break;
                case (char)PieceType.Queen:
                    board._pieces.Add(new Queen(colour, x, y));
                    break;
                case (char)PieceType.Rook:
                    board._pieces.Add(new Rook(colour, x, y));
                    break;
                default: break;
            }

            if (x == Position.MaxX)
            {
                x = Position.MinX;
                y--;
            }
            else
            {
                x++;
            }
        }

        return board;
    }

    private static IEnumerable<Piece> CreateTeam1(PieceColour colour)
    {
        return new Piece[]
        {
            new Rook(colour, 'A', 1),
            new Knight(colour, 'B', 1),
            new Bishop(colour, 'C', 1),
            new King(colour, 'D', 1),
            new Queen(colour, 'E', 1),
            new Bishop(colour, 'F', 1),
            new Knight(colour, 'G', 1),
            new Rook(colour, 'H', 1),
            new Pawn(colour, 'A', 2),
            new Pawn(colour, 'B', 2),
            new Pawn(colour, 'C', 2),
            new Pawn(colour, 'D', 2),
            new Pawn(colour, 'E', 2),
            new Pawn(colour, 'F', 2),
            new Pawn(colour, 'G', 2),
            new Pawn(colour, 'H', 2)
        };
    }

    private static IEnumerable<Piece> CreateTeam2(PieceColour colour)
    {
        return new Piece[]
        {
            new Pawn(colour, 'A', 7),
            new Pawn(colour, 'B', 7),
            new Pawn(colour, 'C', 7),
            new Pawn(colour, 'D', 7),
            new Pawn(colour, 'E', 7),
            new Pawn(colour, 'F', 7),
            new Pawn(colour, 'G', 7),
            new Pawn(colour, 'H', 7),
            new Rook(colour, 'A', 8),
            new Knight(colour, 'B', 8),
            new Bishop(colour, 'C', 8),
            new King(colour, 'D', 8),
            new Queen(colour, 'E', 8),
            new Bishop(colour, 'F', 8),
            new Knight(colour, 'G', 8),
            new Rook(colour, 'H', 8)
        };
    }
}