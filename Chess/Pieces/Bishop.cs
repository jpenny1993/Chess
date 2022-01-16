namespace Chess.Pieces;

public class Bishop : Piece
{
    private const PieceType ChessPiece = PieceType.Bishop;

    public Bishop(PieceColour colour)
        : base(colour, ChessPiece)
    {
    }
    
    public Bishop(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        SetPosition(x, y);
    }
}