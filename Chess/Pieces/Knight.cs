namespace Chess.Pieces;

public sealed class Knight : Piece
{
    private const PieceType ChessPiece = PieceType.Knight;

    public Knight(PieceColour colour, char x, int y)
        : base(colour, ChessPiece)
    {
        Position = new (x, y);
    }
}