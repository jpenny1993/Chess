using Chess.Actions;
using Chess.Notation;
using Chess.Pieces;

namespace Chess;

public class Board
{
    private readonly List<Piece> _pieces = new();

    public IEnumerable<Piece> Pieces => _pieces;

    public Board()
    {
        _pieces.AddRange(CreateWhiteTeam());
        _pieces.AddRange(CreateBlackTeam());
    }

    public Board(IEnumerable<Piece> pieces)
    {
        _pieces.AddRange(pieces);
    }

    public void ApplyTurn(NotedTurn turn)
    {
        // TODO: validate and apply turn movements
        var whitePlayerMove = ConvertNotedPlayerTurnToMovement(turn.WhitePlayerTurn);

        var blackPlayerMove = ConvertNotedPlayerTurnToMovement(turn.BlackPlayerTurn);
    }

    public Piece? FindPiece(Position position)
    {
        return _pieces.FirstOrDefault(p => p.Position.Equals(position));
    }
    
    public Piece? FindPiece(int x, int y)
    {
        return _pieces.FirstOrDefault(p => p.Position.Equals(x, y));
    }
    
    public Piece? FindPiece(PieceColour colour, PieceType pieceType, string? hint, Position destination)
    {
        // ReSharper disable once ReplaceWithSingleCallToSingleOrDefault
        return _pieces
            .Where(p => p.Colour == colour)
            .Where(p => p.Type == pieceType)
            .Where(p => p.Position.Contains(hint))
            .Where(p => p.CanMoveTo(this, destination)) // This is going to overlap for 3 pawns on start if you don't specify captures
            .SingleOrDefault(); // Expect only one
    }

    private Movement? ConvertNotedPlayerTurnToMovement(NotedPlayerTurn turn)
    {
        if (turn?.Moves == default || turn.Moves.Count == 0)
        {
            return default;
        }

        var playerMove = turn.Moves.First();
        var playerOriginPiece = FindPiece(turn.Colour, playerMove.Piece, playerMove.Hint, playerMove.Destination);
        if (playerOriginPiece == default)
        {
            throw new ApplicationException("Unable to find piece to move.");
        }

        var actions = new List<IAction>();

        if (turn.IsCapture)
        {
            var intersectingPiece = FindPiece(playerMove.Destination);
            if (intersectingPiece == default)
            {
                throw new ApplicationException("Expected capture but piece is missing.");
            }

            actions.Add(new Capture(intersectingPiece.Type));
        }

        if (turn.IsCheck)
        {
            actions.Add(new Check());
        }
        else if (turn.IsCheckmate)
        {
            actions.Add(new Checkmate());
        }

        if (turn.Promotion != default)
        {
            actions.Add(new Promotion(turn.Promotion.Value));
        }
        else if (turn.IsCastling)
        {
            actions.Add(new Castle(turn.IsKingSide));
        }

        return new (playerOriginPiece.Position, playerMove.Destination, actions);
    }

    public static bool IsBlackTile(int x, int y)
    {
        const int zeroChar = 'A' - 1;
        var xIsEven = (x - zeroChar) % 2 == 0;
        var yIsEven =  y % 2 == 0;
        return (xIsEven && yIsEven) || (!xIsEven && !yIsEven);
    }

    private static IEnumerable<Piece> CreateWhiteTeam()
    {
        const PieceColour colour = PieceColour.White;
        return new Piece[]
        {
            new Rook(colour, 'A', 1),
            new Knight(colour, 'B', 1),
            new Bishop(colour, 'C', 1),
            new King(colour, 'D', 1),
            new Queen(colour, 'E', 1),
            new Bishop(colour, 'F', 1),
            new Knight(colour, 'G', 1),
            new Rook(colour, 'H', 1),
            new Pawn(colour, 'A', 2),
            new Pawn(colour, 'B', 2),
            new Pawn(colour, 'C', 2),
            new Pawn(colour, 'D', 2),
            new Pawn(colour, 'E', 2),
            new Pawn(colour, 'F', 2),
            new Pawn(colour, 'G', 2),
            new Pawn(colour, 'H', 2)
        };
    }

    private static IEnumerable<Piece> CreateBlackTeam()
    {
        const PieceColour colour = PieceColour.Black;
        return new Piece[]
        {
            new Pawn(colour, 'A', 7),
            new Pawn(colour, 'B', 7),
            new Pawn(colour, 'C', 7),
            new Pawn(colour, 'D', 7),
            new Pawn(colour, 'E', 7),
            new Pawn(colour, 'F', 7),
            new Pawn(colour, 'G', 7),
            new Pawn(colour, 'H', 7),
            new Rook(colour, 'A', 8),
            new Knight(colour, 'B', 8),
            new Bishop(colour, 'C', 8),
            new King(colour, 'D', 8),
            new Queen(colour, 'E', 8),
            new Bishop(colour, 'F', 8),
            new Knight(colour, 'G', 8),
            new Rook(colour, 'H', 8)
        };
    }
}