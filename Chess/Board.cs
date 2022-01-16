using Chess.Pieces;

namespace Chess;

public class Board
{
    private readonly List<Piece> _pieces = Array.Empty<Piece>()
        .Concat(CreateTeam1(PieceColour.White))
        .Concat(CreateTeam2(PieceColour.Black))
        .ToList();

    public Piece? FindPiece(char x, int y)
    {
        return _pieces.FirstOrDefault(p => p.IsAtLocation(x, y));
    }

    public static bool IsBlackTile(char x, int y)
    {
        const int zeroChar = 'A' - 1;
        var xIsEven = (x - zeroChar) % 2 == 0;
        var yIsEven =  y % 2 == 0;
        return (xIsEven && yIsEven) || (!xIsEven && !yIsEven);
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