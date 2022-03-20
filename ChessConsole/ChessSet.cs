using Chess;

namespace ChessConsole;

public sealed class ChessSet
{
    public Board Board { get; } = new();

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
                var isBlackTile = Board.IsBlackTile(x, y);
                Console.BackgroundColor = isBlackTile ? ConsoleColor.Black : ConsoleColor.Gray;
                var piece = Board.FindPiece(x, y);
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
}