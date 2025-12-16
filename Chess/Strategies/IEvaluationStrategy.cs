namespace Chess.Strategies;

/// <summary>
/// Defines a strategy for evaluating chess moves based on specific chess principles.
/// Each strategy should be independent and focused on one aspect of move evaluation.
/// </summary>
public interface IEvaluationStrategy
{
    /// <summary>
    /// Evaluates a move and returns a score in centipawns.
    /// Positive scores favor the moving player, negative scores favor the opponent.
    /// </summary>
    /// <param name="board">The current board state</param>
    /// <param name="movement">The move to evaluate</param>
    /// <returns>Score in centipawns (-100000 to +100000)</returns>
    int Evaluate(Board board, Movement movement);

    /// <summary>
    /// Gets the name of this evaluation strategy for logging/debugging purposes.
    /// </summary>
    string Name { get; }
}
