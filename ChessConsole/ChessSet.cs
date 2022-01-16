namespace ChessConsole;

public sealed class ChessSet
{
    public readonly List<Piece> Board = new()
    {
        new Piece('A', 1, PieceColour.White, PieceType.Rook),
        new Piece('B', 1, PieceColour.White, PieceType.Knight),
        new Piece('C', 1, PieceColour.White, PieceType.Bishop),
        new Piece('D', 1, PieceColour.White, PieceType.King),
        new Piece('E', 1, PieceColour.White, PieceType.Queen),
        new Piece('F', 1, PieceColour.White, PieceType.Bishop),
        new Piece('G', 1, PieceColour.White, PieceType.Knight),
        new Piece('H', 1, PieceColour.White, PieceType.Rook),
        new Piece('A', 2, PieceColour.White, PieceType.Pawn),
        new Piece('B', 2, PieceColour.White, PieceType.Pawn),
        new Piece('C', 2, PieceColour.White, PieceType.Pawn),
        new Piece('D', 2, PieceColour.White, PieceType.Pawn),
        new Piece('E', 2, PieceColour.White, PieceType.Pawn),
        new Piece('F', 2, PieceColour.White, PieceType.Pawn),
        new Piece('G', 2, PieceColour.White, PieceType.Pawn),
        new Piece('H', 2, PieceColour.White, PieceType.Pawn),
        new Piece('A', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('B', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('C', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('D', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('E', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('F', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('G', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('H', 7, PieceColour.Black, PieceType.Pawn),
        new Piece('A', 8, PieceColour.Black, PieceType.Rook),
        new Piece('B', 8, PieceColour.Black, PieceType.Knight),
        new Piece('C', 8, PieceColour.Black, PieceType.Bishop),
        new Piece('D', 8, PieceColour.Black, PieceType.King),
        new Piece('E', 8, PieceColour.Black, PieceType.Queen),
        new Piece('F', 8, PieceColour.Black, PieceType.Bishop),
        new Piece('G', 8, PieceColour.Black, PieceType.Knight),
        new Piece('H', 8, PieceColour.Black, PieceType.Rook)
    };

    public void DrawBoard()
    {
        Console.Clear();
        Console.WriteLine();

        for (var y = 8; y > 0; y--)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            
            Console.Write($" {y} ");
            Console.Write($" |");
            for (var x = 'A'; x < 'I'; x++)
            {
                var isBlackTile = IsBlackTile(x, y);
                Console.BackgroundColor = isBlackTile ? ConsoleColor.Black : ConsoleColor.Gray;
                var piece = Board.FirstOrDefault(p => p.IsAtLocation(x, y));
                if (piece != null)
                {
                    if (piece.IsBlack)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (piece.IsWhite)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    Console.Write(" ");
                    Console.Write((char)piece.Type);
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("   ");
                }
            }

            Console.Write(Environment.NewLine);
        }

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("    -------------------------\r\n");
        Console.Write("     ");
        for (var x = 'A'; x < 'I'; x++)
        {
            Console.Write($" {x} ");
        }
    }

    private static bool IsBlackTile(char x, int y)
    {
        const int zeroChar = 'A' - 1;
        var xIsEven = (x - zeroChar) % 2 == 0;
        var yIsEven =  y % 2 == 0;
        return (xIsEven && yIsEven) || (!xIsEven && !yIsEven);
    }
}