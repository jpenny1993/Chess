namespace Chess;

/// <summary>
/// Provides tactical analysis of the chess board including attack detection,
/// piece safety evaluation, and square control information.
/// </summary>
public sealed class BoardAnalysis
{
    private readonly Board _board;
    private Dictionary<Position, HashSet<Piece>>? _attackMapWhite;
    private Dictionary<Position, HashSet<Piece>>? _attackMapBlack;

    public BoardAnalysis(Board board)
    {
        _board = board ?? throw new ArgumentNullException(nameof(board));
    }

    /// <summary>
    /// Determines if a square is attacked by pieces of the specified color.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <param name="attackerColour">The color of the attacking pieces</param>
    /// <returns>True if the square is attacked by the specified color</returns>
    public bool IsSquareAttackedBy(Position position, PieceColour attackerColour)
    {
        EnsureAttackMapsBuilt();
        var map = attackerColour == PieceColour.White ? _attackMapWhite : _attackMapBlack;
        return map!.ContainsKey(position) && map[position].Count > 0;
    }

    /// <summary>
    /// Gets all pieces of the specified color that are attacking a square.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <param name="attackerColour">The color of the attacking pieces</param>
    /// <returns>Collection of pieces attacking the square</returns>
    public IEnumerable<Piece> GetAttackers(Position position, PieceColour attackerColour)
    {
        EnsureAttackMapsBuilt();
        var map = attackerColour == PieceColour.White ? _attackMapWhite : _attackMapBlack;
        return map!.TryGetValue(position, out var attackers)
            ? attackers
            : Enumerable.Empty<Piece>();
    }

    /// <summary>
    /// Determines if the king of the specified color is currently in check.
    /// </summary>
    /// <param name="kingColour">The color of the king to check</param>
    /// <returns>True if the king is in check</returns>
    public bool IsKingInCheck(PieceColour kingColour)
    {
        var king = _board.Pieces.FirstOrDefault(p => p.IsKing && p.Colour == kingColour);
        if (king == null) return false;

        var enemyColour = kingColour == PieceColour.White ? PieceColour.Black : PieceColour.White;
        return IsSquareAttackedBy(king.Position, enemyColour);
    }

    /// <summary>
    /// Counts how many pieces of the specified color are attacking a square.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <param name="attackerColour">The color of the attacking pieces</param>
    /// <returns>Number of attackers</returns>
    public int CountAttackers(Position position, PieceColour attackerColour)
    {
        return GetAttackers(position, attackerColour).Count();
    }

    /// <summary>
    /// Determines if a square is defended by pieces of the specified color.
    /// A square is defended if it's occupied by a friendly piece and other friendly pieces can reach it.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <param name="defenderColour">The color of the defending pieces</param>
    /// <returns>True if the square is defended</returns>
    public bool IsSquareDefendedBy(Position position, PieceColour defenderColour)
    {
        // A square is defended if friendly pieces can move there
        return IsSquareAttackedBy(position, defenderColour);
    }

    /// <summary>
    /// Invalidates the cached attack maps, forcing them to be recalculated on next access.
    /// Call this when the board state changes.
    /// </summary>
    public void InvalidateCache()
    {
        _attackMapWhite = null;
        _attackMapBlack = null;
    }

    /// <summary>
    /// Builds attack maps for both colors if they haven't been built yet.
    /// Attack maps show which pieces can move to which squares.
    /// </summary>
    private void EnsureAttackMapsBuilt()
    {
        if (_attackMapWhite != null) return;

        _attackMapWhite = new Dictionary<Position, HashSet<Piece>>();
        _attackMapBlack = new Dictionary<Position, HashSet<Piece>>();

        foreach (var piece in _board.Pieces)
        {
            var map = piece.IsWhite ? _attackMapWhite : _attackMapBlack;

            // For each square this piece can attack/move to
            // Note: We use TheoreticalPaths and check for blocked paths manually
            // because PossibleMoves might filter based on leaving king in check,
            // but for attack maps we want to know what squares are controlled regardless
            foreach (var path in piece.TheoreticalPaths())
            {
                foreach (var step in path)
                {
                    var movement = piece.GetMovement(piece, _board, path, step);
                    if (movement == default) break;

                    if (!map.ContainsKey(movement.Destination))
                        map[movement.Destination] = new HashSet<Piece>();

                    map[movement.Destination].Add(piece);

                    // Stop at captures (can't move through pieces)
                    if (movement.IsCapture)
                        break;
                }
            }
        }
    }
}
