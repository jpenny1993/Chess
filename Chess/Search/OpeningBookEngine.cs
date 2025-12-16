namespace Chess.Search;

/// <summary>
/// Decorator engine that consults an opening book before falling back to search.
/// Provides instant opening moves (0 nodes evaluated) for known positions.
/// Falls through to wrapped engine when out of book or for middlegame/endgame positions.
/// </summary>
public sealed class OpeningBookEngine : ISearchEngine
{
    private readonly ISearchEngine _fallbackEngine;
    private readonly OpeningBook _book;

    /// <summary>
    /// Creates an OpeningBookEngine wrapping the given fallback search engine.
    /// Uses the default OpeningBook with hardcoded opening theory.
    /// </summary>
    /// <param name="fallbackEngine">Engine to use when move is not in opening book</param>
    /// <exception cref="ArgumentNullException">If fallbackEngine is null</exception>
    public OpeningBookEngine(ISearchEngine fallbackEngine)
        : this(fallbackEngine, new OpeningBook()) { }

    /// <summary>
    /// Creates an OpeningBookEngine wrapping the given search engine with a custom opening book.
    /// Useful for testing with different opening theories.
    /// </summary>
    /// <param name="fallbackEngine">Engine to use when move is not in opening book</param>
    /// <param name="book">Custom opening book (uses default if null)</param>
    /// <exception cref="ArgumentNullException">If fallbackEngine is null</exception>
    public OpeningBookEngine(ISearchEngine fallbackEngine, OpeningBook? book)
    {
        _fallbackEngine = fallbackEngine ?? throw new ArgumentNullException(nameof(fallbackEngine));
        _book = book ?? new OpeningBook();
    }

    /// <summary>
    /// Finds the best move by consulting the opening book first.
    /// If position is in book, returns book move with 0 nodes evaluated (instant).
    /// Otherwise delegates to fallback search engine for full analysis.
    /// </summary>
    /// <param name="board">Current board position</param>
    /// <param name="colour">Colour of the side to move</param>
    /// <param name="depth">Maximum search depth (ignored for book hits)</param>
    /// <returns>Search result with best move and evaluation</returns>
    public SearchResult FindBestMove(Board board, PieceColour colour, int depth)
    {
        // Try to find a book move for this position
        var bookMove = _book.SelectBookMove(board, colour);

        if (bookMove != null)
        {
            try
            {
                // Convert the book move to an actual Movement on the board
                var movement = bookMove.ToMovement(board, colour);

                // Return as SearchResult with book characteristics:
                // - 0 nodes evaluated (instant lookup)
                // - 0 depth reached (no search performed)
                // - Score of 0 (book moves considered balanced)
                return new SearchResult
                {
                    BestMove = movement,
                    Score = 0,
                    NodesEvaluated = 0,
                    DepthReached = 0,
                    MateInMoves = null,
                    PrincipalVariation = new List<Movement> { movement },
                    WasPruned = false
                };
            }
            catch (InvalidOperationException)
            {
                // Book move is no longer legal in this position (defensive coding)
                // This shouldn't happen with a well-formed book, but fall through to search
            }
        }

        // Position not in book or book move failed
        // Delegate to fallback search engine for full analysis
        return _fallbackEngine.FindBestMove(board, colour, depth);
    }
}
