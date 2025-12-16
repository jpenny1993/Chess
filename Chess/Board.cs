using Chess.Actions;
using Chess.Notation;
using Chess.Pieces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Chess.Tests")]

namespace Chess;

public class Board
{
    private readonly List<Piece> _pieces = new();

    public IEnumerable<Piece> Pieces => _pieces;

    /// <summary>
    /// The last movement that was made on the board.
    /// This is used to determine if en passant is legal (must be immediately after enemy pawn's two-square advance).
    /// </summary>
    public Movement? LastMove { get; set; }

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
        // Apply white's move
        var whitePlayerMove = ConvertNotedPlayerTurnToMovement(turn.WhitePlayerTurn);
        if (whitePlayerMove != null)
        {
            ApplyMovement(whitePlayerMove);
        }

        // Apply black's move (if present - white might have checkmated)
        var blackPlayerMove = ConvertNotedPlayerTurnToMovement(turn.BlackPlayerTurn);
        if (blackPlayerMove != null)
        {
            ApplyMovement(blackPlayerMove);
        }
    }

    /// <summary>
    /// Applies a single player's turn (move) to the board.
    /// This allows applying white and black moves independently.
    /// </summary>
    /// <param name="playerTurn">The player turn to apply (contains white or black move)</param>
    public void ApplyPlayerTurn(NotedPlayerTurn playerTurn)
    {
        var movement = ConvertNotedPlayerTurnToMovement(playerTurn);
        if (movement != null)
        {
            ApplyMovement(movement);
        }
    }

    /// <summary>
    /// Applies a movement to the board, updating piece positions and handling special moves.
    /// </summary>
    internal void ApplyMovement(Movement movement)
    {
        var piece = movement.MovingPiece;

        // Validate the move is legal
        if (!piece.CanMoveTo(this, movement.Destination))
        {
            throw new InvalidOperationException($"Illegal move: {piece.Type} at {piece.Position} cannot move to {movement.Destination}");
        }

        // Handle castling
        if (movement.IsCastling)
        {
            ApplyCastling(piece, movement);
            LastMove = movement;
            return;
        }

        if (movement.IsCapture)
        {
            RemovePiece(movement.Destination);
        }

        // Move the piece
        piece.Position = movement.Destination;
        piece.HasMoved = true;

        // Handle pawn promotion
        if (movement.IsPromotion)
        {
            var promotionAction = movement.Actions.OfType<Promotion>().First();
            RemovePiece(movement.Destination);

            // Create new promoted piece at destination
            Piece promotedPiece = promotionAction.Piece switch
            {
                PieceType.Queen => new Queen(piece.Colour, movement.Destination.X, movement.Destination.Y),
                PieceType.Rook => new Rook(piece.Colour, movement.Destination.X, movement.Destination.Y),
                PieceType.Bishop => new Bishop(piece.Colour, movement.Destination.X, movement.Destination.Y),
                PieceType.Knight => new Knight(piece.Colour, movement.Destination.X, movement.Destination.Y),
                _ => throw new InvalidOperationException($"Invalid promotion piece type: {promotionAction.Piece}")
            };

            promotedPiece.HasMoved = true;
            AddPiece(promotedPiece);
        }

        // Update last move
        LastMove = movement;
    }

    /// <summary>
    /// Handles castling by moving both the king and rook.
    /// </summary>
    private void ApplyCastling(Piece king, Movement movement)
    {
        var isKingside = movement.Actions.OfType<Castle>().First().IsKingSide;

        // Move king
        king.Position = movement.Destination;
        king.HasMoved = true;

        // Find and move rook
        if (isKingside)
        {
            // Kingside: rook moves from H to F
            var rookOrigin = new Position((char)('H'), king.Position.Y);
            var rookDestination = new Position((char)('F'), king.Position.Y);
            var rook = FindPiece(rookOrigin);

            if (rook != null)
            {
                rook.Position = rookDestination;
                rook.HasMoved = true;
            }
        }
        else
        {
            // Queenside: rook moves from A to D
            var rookOrigin = new Position((char)('A'), king.Position.Y);
            var rookDestination = new Position((char)('D'), king.Position.Y);
            var rook = FindPiece(rookOrigin);

            if (rook != null)
            {
                rook.Position = rookDestination;
                rook.HasMoved = true;
            }
        }
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

    /// <summary>
    /// Temporarily removes a piece from the board. Used for move simulation.
    /// Returns the removed piece so it can be restored.
    /// </summary>
    internal Piece? RemovePiece(Position position)
    {
        var piece = FindPiece(position);
        if (piece != null)
        {
            _pieces.Remove(piece);
        }
        return piece;
    }

    /// <summary>
    /// Adds a piece back to the board. Used for undoing move simulation.
    /// </summary>
    internal void AddPiece(Piece piece)
    {
        if (!_pieces.Contains(piece))
        {
            _pieces.Add(piece);
        }
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

        return new (playerOriginPiece, playerOriginPiece.Position, playerMove.Destination, actions);
    }

    /// <summary>
    /// Determines if the specified color is in stalemate (no legal moves but not in check).
    /// </summary>
    /// <param name="colour">The color to check for stalemate</param>
    /// <returns>True if the color is in stalemate</returns>
    public bool IsStalemate(PieceColour colour)
    {
        // First check: King must NOT be in check for stalemate
        var analysis = new BoardAnalysis(this);
        if (analysis.IsKingInCheck(colour))
        {
            return false; // In check = not stalemate (could be checkmate)
        }

        // Second check: Player must have no legal moves
        var playerPieces = _pieces.Where(p => p.Colour == colour);

        foreach (var piece in playerPieces)
        {
            // If any piece has at least one legal move, it's not stalemate
            if (piece.PossibleMoves(this).Any())
            {
                return false;
            }
        }

        // Not in check and no legal moves = stalemate
        return true;
    }

    public static bool IsBlackTile(int x, int y)
    {
        const int zeroChar = 'A' - 1;
        var xIsEven = (x - zeroChar) % 2 == 0;
        var yIsEven =  y % 2 == 0;
        return (xIsEven && yIsEven) || (!xIsEven && !yIsEven);
    }

    private static Piece[] CreateWhiteTeam()
    {
        const PieceColour colour = PieceColour.White;
        return
        [
            new Rook(colour, 'A', 1),
            new Knight(colour, 'B', 1),
            new Bishop(colour, 'C', 1),
            new Queen(colour, 'D', 1),
            new King(colour, 'E', 1),
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
        ];
    }

    private static Piece[] CreateBlackTeam()
    {
        const PieceColour colour = PieceColour.Black;
        return
        [
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
            new Queen(colour, 'D', 8),
            new King(colour, 'E', 8),
            new Bishop(colour, 'F', 8),
            new Knight(colour, 'G', 8),
            new Rook(colour, 'H', 8)
        ];
    }
}