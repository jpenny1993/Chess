using System.Text;
using Chess.Actions;

namespace Chess.Notation;

public sealed class NotedPlayerTurn
{
    public NotedPlayerTurn(PieceColour colour)
    {
        Colour = colour;
        Moves = new List<NotedPlayerMove>();
    }
    
    public PieceColour Colour { get; }

    public PieceType? Promotion { get; set; }

    public IList<NotedPlayerMove> Moves { get; }
    
    public bool IsCapture { get; set; }
    
    public bool IsCastling { get; set; }
    
    public bool IsKingSide { get; set; }
    
    public bool IsCheck { get; set; }
    
    public bool IsCheckmate { get; set; }

    public override string ToString()
    {
        if (IsCastling)
        {
            return IsKingSide ? "O-O" : "O-O-O";
        }

        var firstMove = Moves.FirstOrDefault();
        var isPawn = firstMove?.Piece == PieceType.Pawn;
        var sb = new StringBuilder();

        if (!isPawn)
        {
            sb.Append((char)Moves[0].Piece);
        }

        if (!string.IsNullOrEmpty(firstMove?.Hint))
        {
            sb.Append(firstMove?.Hint);
        }

        if (IsCapture)
        {
            sb.Append('x');
        }

        if (firstMove != default)
        {
            sb.Append(firstMove.MoveTo);
        }

        if (IsCheck)
        {
            sb.Append('+');
        }
        else if (IsCheckmate)
        {
            sb.Append('#');
        }

        return sb.ToString();
    }
}