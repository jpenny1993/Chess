namespace Chess.Pieces;

public class Queen : Piece
{
    private const PieceType ChessPiece = PieceType.Queen;

    public Queen(PieceColour colour)
        : base(colour, ChessPiece)
    {
    }
    
    public Queen(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        SetPosition(x, y);
    }
}