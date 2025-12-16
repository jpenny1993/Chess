namespace Chess.Search;

/// <summary>
/// Result of a depth-based search operation.
/// Contains the best move found and associated evaluation data.
/// </summary>
public sealed class SearchResult
{
    /// <summary>
    /// The best move found by the search engine.
    /// </summary>
    public required Movement BestMove { get; init; }

    /// <summary>
    /// The evaluation score of the best move (-100000 to +100000 centipawns).
    /// Positive = White advantage, Negative = Black advantage.
    /// Scores above 80000 indicate forced checkmate.
    /// </summary>
    public required int Score { get; init; }

    /// <summary>
    /// Total number of leaf nodes evaluated during the search.
    /// Used to measure search efficiency and pruning effectiveness.
    /// </summary>
    public int NodesEvaluated { get; init; }

    /// <summary>
    /// Actual search depth reached (in plies).
    /// May be less than requested depth if search terminated early.
    /// </summary>
    public int DepthReached { get; init; }

    /// <summary>
    /// If the position is a forced checkmate, this indicates mate in N moves.
    /// Null if position is not a forced checkmate.
    /// </summary>
    public int? MateInMoves { get; init; }

    /// <summary>
    /// Principal variation - the expected sequence of moves.
    /// Primarily used for debugging and displaying search result.
    /// </summary>
    public IReadOnlyList<Movement> PrincipalVariation { get; init; } = new List<Movement>();

    /// <summary>
    /// Whether this search result was affected by alpha-beta pruning.
    /// (Used for testing and diagnostics)
    /// </summary>
    public bool WasPruned { get; init; }
}
