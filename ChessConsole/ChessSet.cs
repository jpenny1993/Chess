using Chess;

namespace ChessConsole;

public sealed class ChessSet
{
    public Board Board { get; } = new();
    private readonly MoveEvaluator _evaluator = new();
    public PlayStyle CurrentPlayStyle { get; private set; } = PlayStyle.Solid;

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

    /// <summary>
    /// Displays the top 5 suggested best moves for the specified player color.
    /// </summary>
    public void DisplaySuggestedMoves(PieceColour playerColor)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine();
        Console.WriteLine("Suggested best moves:");
        Console.ForegroundColor = ConsoleColor.Gray;

        var suggestedMoves = _evaluator.GetBestMoves(Board, playerColor, 5);

        if (!suggestedMoves.Any())
        {
            Console.WriteLine("  No moves available");
            return;
        }

        for (var i = 0; i < suggestedMoves.Count; i++)
        {
            var move = suggestedMoves[i];
            var fromSquare = PositionToAlgebraic(move.SourcePosition);
            var toSquare = PositionToAlgebraic(move.TargetPosition);
            Console.WriteLine($"  {i + 1}. {move.Description} ({fromSquare} → {toSquare}) [Score: {move.Score}]");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Sets the play style for move evaluation and suggestions.
    /// </summary>
    public void SetPlayStyle(PlayStyle style)
    {
        CurrentPlayStyle = style;
        _evaluator.ConfigurePlayStyle(style);
    }

    private static string PositionToAlgebraic(Position position)
    {
        return $"{position.X}{position.Y}";
    }
}