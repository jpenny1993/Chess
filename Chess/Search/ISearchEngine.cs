namespace Chess.Search;

/// <summary>
/// Defines the contract for chess search engines (minimax, alpha-beta, etc.)
/// Responsible for finding the best move in a given position using lookahead.
/// </summary>
public interface ISearchEngine
{
    /// <summary>
    /// Finds the best move in the given position using depth-based search.
    /// </summary>
    /// <param name="board">The current board position</param>
    /// <param name="colour">The colour of the side to move</param>
    /// <param name="depth">Maximum search depth in plies</param>
    /// <returns>Search result containing best move and evaluation</returns>
    SearchResult FindBestMove(Board board, PieceColour colour, int depth);
}
