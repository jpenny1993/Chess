// See https://aka.ms/new-console-template for more information

using Chess;
using ChessConsole;

var chessSet = new ChessSet();

chessSet.DrawBoard();

Console.WriteLine();
Console.Write(":>");

var notation = new Notation();
while (true)
{
    var input = Console.ReadLine();
    var move = notation.ReadRound(input!);
    Console.WriteLine();
    Console.Write(":>");
}