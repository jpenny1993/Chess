using Chess.Actions;
using Chess.Pieces;

namespace Chess;

public abstract class Piece
{
    public Position Position { get; set; }

    public PieceColour Colour { get; }

    public string Notation { get; }

    public PieceType Type { get; }

    public bool HasMoved { get; set; }

    public bool IsBlack => Colour == PieceColour.Black;
    public bool IsWhite => Colour == PieceColour.White;

    public bool IsBishop => Type == PieceType.Bishop;
    public bool IsKnight => Type == PieceType.Knight;
    public bool IsKing => Type == PieceType.King;
    public bool IsPawn => Type == PieceType.Pawn;
    public bool IsQueen => Type == PieceType.Queen;
    public bool IsRook => Type == PieceType.Rook;

    /// <summary>
    /// Gets the Unicode chess piece emoji symbol.
    /// </summary>
    public string Emoji => (Colour, Type) switch
    {
        (PieceColour.White, PieceType.King) => "♔",
        (PieceColour.White, PieceType.Queen) => "♕",
        (PieceColour.White, PieceType.Rook) => "♖",
        (PieceColour.White, PieceType.Bishop) => "♗",
        (PieceColour.White, PieceType.Knight) => "♘",
        (PieceColour.White, PieceType.Pawn) => "♙",
        (PieceColour.Black, PieceType.King) => "♚",
        (PieceColour.Black, PieceType.Queen) => "♛",
        (PieceColour.Black, PieceType.Rook) => "♜",
        (PieceColour.Black, PieceType.Bishop) => "♝",
        (PieceColour.Black, PieceType.Knight) => "♞",
        (PieceColour.Black, PieceType.Pawn) => "♟",
        _ => "?" // output requires Unicode if you see this
    };

    internal Piece(PieceColour colour, PieceType type)
    {
        Position = default;
        Colour = colour;
        Type = type;
        Notation = type == PieceType.Pawn ? string.Empty : ((char)type).ToString();
    }

    /// <summary>
    /// Defines whether the piece can move to the suggested position,
    /// regardless of whether there is another piece on that tile,
    /// but should be prevented by any intersecting pieces.
    /// </summary>
    public bool CanMoveTo(Board board, Position position)
    {
        var pathsContainingDestination = TheoreticalPaths()
            .Where(path => path.Contains(position));

        foreach (var path in pathsContainingDestination)
        foreach (var step in path)
        {
            var movement = GetMovement(this, board, path, step);
            if (movement == default)
            {
                break;
            }

            if (movement.Destination.Equals(position))
            {
                return true;
            }

            if (movement.IsCapture)
            {
                break;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns <see langword="true" /> if the colour of the pieces are matching. 
    /// </summary>
    public bool IsFriendly(Piece piece) => piece.Colour == Colour;

    /// <summary>
    /// Defines all the actually possible moves that a piece can make.
    /// This excludes passing intersecting pieces, moving to positions occupied by the same team,
    /// and moves that would leave own king in check.
    /// </summary>
    public IEnumerable<Movement> PossibleMoves(Board board)
    {
        var analysis = new BoardAnalysis(board);

        foreach (var path in TheoreticalPaths())
        foreach (var step in path)
        {
            var movement = GetMovement(this, board, path, step);
            if (movement == default)
            {
                break;
            }

            // Filter out moves that would leave own king in check
            if (WouldLeaveKingInCheck(board, movement))
            {
                // This move is illegal - skip it
                // For captures, we still need to stop the path
                if (movement.IsCapture)
                {
                    break;
                }
                continue;
            }

            // Enrich movement with tactical information
            EnrichMovement(board, analysis, movement);
            yield return movement;

            if (movement.IsCapture)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Determines if making this move would leave own king in check.
    /// </summary>
    private bool WouldLeaveKingInCheck(Board board, Movement movement)
    {
        // Save original state
        var originalPosition = Position;
        var originalHasMoved = HasMoved;

        // Simulate the move
        var capturedPiece = board.RemovePiece(movement.Destination);
        Position = movement.Destination;
        HasMoved = true;

        // Check if our king is now in check
        var analysis = new BoardAnalysis(board);
        var inCheck = analysis.IsKingInCheck(Colour);

        // Undo the move
        Position = originalPosition;
        HasMoved = originalHasMoved;
        if (capturedPiece != null)
        {
            board.AddPiece(capturedPiece);
        }

        return inCheck;
    }

    /// <summary>
    /// Enriches a movement with tactical information (check, checkmate, defended, safe capture).
    /// Mutates the movement in place rather than creating a new instance.
    /// </summary>
    private void EnrichMovement(Board board, BoardAnalysis analysis, Movement movement)
    {
        var enemyColour = Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

        // Check if destination is defended by enemy
        movement.IsDefended = analysis.IsSquareDefendedBy(movement.Destination, enemyColour);

        // Determine if capture is safe
        if (movement.IsCapture)
        {
            var capturedPiece = board.FindPiece(movement.Destination);
            if (capturedPiece != null)
            {
                // Safe if destination not defended, or if captured piece is more valuable
                movement.IsSafeCapture = !movement.IsDefended ||
                                        PieceValue.GetValue(capturedPiece) >= PieceValue.GetValue(this);
            }
        }

        // Check detection: Simulate the move and see if enemy king is in check
        var (isCheck, isCheckmate) = SimulateMoveForCheck(board, movement, enemyColour);
        movement.IsCheck = isCheck;
        movement.IsCheckmate = isCheckmate;
    }

    /// <summary>
    /// Simulates a move to determine if it results in check or checkmate.
    /// </summary>
    private (bool IsCheck, bool IsCheckmate) SimulateMoveForCheck(Board board, Movement movement, PieceColour enemyColour)
    {
        // Save the original position
        var originalPosition = Position;
        var originalHasMoved = HasMoved;

        // Simulate the move
        var capturedPiece = board.RemovePiece(movement.Destination);

        // Special case: if we captured the enemy king, it's automatically checkmate
        if (capturedPiece is King && capturedPiece.Colour == enemyColour)
        {
            // Undo the move before returning
            if (capturedPiece != null)
            {
                board.AddPiece(capturedPiece);
            }
            return (true, true);
        }

        Position = movement.Destination;
        HasMoved = true;

        // Check if enemy king is in check after our move
        var newAnalysis = new BoardAnalysis(board);
        var isCheck = newAnalysis.IsKingInCheck(enemyColour);

        // If in check, determine if it's checkmate
        var isCheckmate = false;
        if (isCheck)
        {
            isCheckmate = IsCheckmate(board, enemyColour);
        }

        // Undo the move
        Position = originalPosition;
        HasMoved = originalHasMoved;
        if (capturedPiece != null)
        {
            board.AddPiece(capturedPiece);
        }

        return (isCheck, isCheckmate);
    }

    /// <summary>
    /// Determines if the specified color is in checkmate (king in check with no legal moves).
    /// </summary>
    private bool IsCheckmate(Board board, PieceColour colour)
    {
        // Get all pieces of the color (materialize to avoid collection modification issues)
        var pieces = board.Pieces.Where(p => p.Colour == colour).ToList();

        // Check if any piece has a legal move that gets out of check
        foreach (var piece in pieces)
        {
            foreach (var path in piece.TheoreticalPaths())
            {
                foreach (var step in path)
                {
                    var testMovement = piece.GetMovement(piece, board, path, step);
                    if (testMovement == default)
                        break;

                    // Simulate this move
                    var originalPos = piece.Position;
                    var originalHasMoved = piece.HasMoved;
                    var capturedPiece = board.RemovePiece(testMovement.Destination);
                    piece.Position = testMovement.Destination;
                    piece.HasMoved = true;

                    // Check if king is still in check
                    var analysis = new BoardAnalysis(board);
                    var stillInCheck = analysis.IsKingInCheck(colour);

                    // Undo
                    piece.Position = originalPos;
                    piece.HasMoved = originalHasMoved;
                    if (capturedPiece != null)
                        board.AddPiece(capturedPiece);

                    // If we found a move that gets out of check, it's not checkmate
                    if (!stillInCheck)
                        return false;

                    if (testMovement.IsCapture)
                        break;
                }
            }
        }

        // No legal moves found that get out of check = checkmate
        return true;
    }
    
    /// <summary>
    /// Defines all possible moves/paths that a piece could make, if the board was currently empty.
    /// </summary>
    public virtual IEnumerable<TheoreticalPath> TheoreticalPaths() => Enumerable.Empty<TheoreticalPath>();

    internal protected virtual Movement? GetMovement(Piece piece, Board board, TheoreticalPath path, Position step)
    {
        var intersectingPiece = board.FindPiece(step);

        // Note: Move validation (check, checkmate, leaving king in check) is handled by:
        // - PossibleMoves(): Filters moves via WouldLeaveKingInCheck()
        // - EnrichMovement(): Adds check/checkmate detection
        // - King.GetCastlingMovement(): Validates castling through check

        if (intersectingPiece == default)
        {
            // Valid move, the tile is empty
            return new (piece, piece.Position, step);
        }

        // Invalid move, can't capture teammates
        if (IsFriendly(intersectingPiece))
        {
            return default;
        }

        // Valid move, can capture enemies
        return new (piece, piece.Position, step, new Capture(intersectingPiece.Type));
    }
}