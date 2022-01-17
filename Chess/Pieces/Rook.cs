namespace Chess.Pieces;

public sealed class Rook : Piece
{
    private const PieceType ChessPiece = PieceType.Rook;

    public Rook(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
}