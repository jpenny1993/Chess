namespace Chess.Pieces;

public class Rook : Piece
{
    private const PieceType ChessPiece = PieceType.Rook;

    public Rook(PieceColour colour)
        : base(colour, ChessPiece)
    {
    }
    
    public Rook(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        SetPosition(x, y);
    }
}