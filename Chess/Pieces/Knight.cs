namespace Chess.Pieces;

public class Knight : Piece
{
    private const PieceType ChessPiece = PieceType.Knight;

    public Knight(PieceColour colour)
        : base(colour, ChessPiece)
    {
    }
    
    public Knight(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        SetPosition(x, y);
    }
}