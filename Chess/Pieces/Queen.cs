namespace Chess.Pieces;

public sealed class Queen : Piece
{
    private const PieceType ChessPiece = PieceType.Queen;

    public Queen(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
}