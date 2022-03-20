// See https://aka.ms/new-console-template for more information

using Chess.Notation;
using ChessConsole;

var chessSet = new ChessSet();

chessSet.DrawBoard();

Console.WriteLine();
Console.Write(":>");

var notation = new AlgebraicNotationReader();
while (true)
{
    var input = Console.ReadLine();
    var move = notation.ReadTurn(input!);
    chessSet.Board.ApplyTurn(move);
    Console.WriteLine();
    Console.Write(":>");
}