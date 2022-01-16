namespace Chess.Pieces;

public class King : Piece
{
    private const PieceType ChessPiece = PieceType.King;

    public King(PieceColour colour)
        : base(colour, ChessPiece)
    {
    }
    
    public King(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        SetPosition(x, y);
    }
}