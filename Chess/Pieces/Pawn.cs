namespace Chess.Pieces;

public class Pawn : Piece
{
    private const PieceType ChessPiece = PieceType.Pawn;

    public Pawn(PieceColour colour)
        : base(colour, ChessPiece)
    {
    }
    
    public Pawn(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        SetPosition(x, y);
    }
}