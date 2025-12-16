using Chess;
using Chess.Notation;
using ChessConsole;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var chessSet = new ChessSet();

chessSet.DrawBoard();

var notation = new AlgebraicNotationReader();
var currentPlayer = PieceColour.White;

while (true)
{
    Console.WriteLine();
    // Show suggested moves for the current player before they move
    chessSet.DisplaySuggestedMoves(currentPlayer);

    Console.Write(":>");
    var input = Console.ReadLine();
    if (currentPlayer == PieceColour.White)
    {
        var move = notation.WhiteTurn(input!);
        chessSet.Board.ApplyPlayerTurn(move);
        currentPlayer = PieceColour.Black;
    }
    else
    {
        var move = notation.BlackTurn(input!);
        chessSet.Board.ApplyPlayerTurn(move);
        currentPlayer = PieceColour.White;
    }

    chessSet.DrawBoard();
}