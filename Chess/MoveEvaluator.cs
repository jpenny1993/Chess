using Chess.Evaluation;

namespace Chess;

/// <summary>
/// Evaluates and scores chess moves using a strategy-based approach.
/// Combines multiple evaluation strategies (material, piece activity, position, etc.)
/// with game-phase-aware weighting for informed move selection.
/// </summary>
public class MoveEvaluator
{
    private WeightedEvaluationComposer _composer;

    public MoveEvaluator() : this(new EvaluationWeights())
    {
    }

    public MoveEvaluator(EvaluationWeights weights)
    {
        _composer = new WeightedEvaluationComposer(weights);
    }

    /// <summary>
    /// Represents a single evaluated move with its score.
    /// </summary>
    public class EvaluatedMove
    {
        public Piece MovingPiece { get; }
        public Position SourcePosition { get; }
        public Position TargetPosition { get; }
        public int Score { get; }
        public string Description { get; }

        internal EvaluatedMove(Piece piece, Position source, Position target, int score, string description)
        {
            MovingPiece = piece;
            SourcePosition = source;
            TargetPosition = target;
            Score = score;
            Description = description;
        }
    }

    /// <summary>
    /// Sets custom evaluation weights for different play styles or preferences.
    /// </summary>
    public void SetWeights(EvaluationWeights weights)
    {
        _composer.SetWeights(weights ?? throw new ArgumentNullException(nameof(weights)));
    }

    /// <summary>
    /// Configures the evaluator to use a specific play style.
    /// </summary>
    public void ConfigurePlayStyle(PlayStyle style)
    {
        _composer.ConfigurePlayStyle(style);
    }

    /// <summary>
    /// Evaluates all possible moves for pieces of a given color and returns the best ones.
    /// Uses strategy-based evaluation with game-phase-aware weighting.
    /// </summary>
    /// <param name="board">The current board state</param>
    /// <param name="color">The piece color to evaluate moves for</param>
    /// <param name="topCount">Number of top moves to return</param>
    /// <returns>List of evaluated moves sorted by score (highest first)</returns>
    public List<EvaluatedMove> GetBestMoves(Board board, PieceColour color, int topCount = 5)
    {
        var allMoves = new List<EvaluatedMove>();

        // Materialize the pieces list to avoid collection modification during iteration
        var pieces = board.Pieces.Where(p => p.Colour == color).ToList();

        // Evaluate all possible moves for all pieces of the given color
        foreach (var piece in pieces)
        {
            var possibleMoves = piece.PossibleMoves(board);

            foreach (var movement in possibleMoves)
            {
                // Use strategy-based evaluation
                var score = _composer.Evaluate(board, movement);
                var description = movement.ToString();

                allMoves.Add(new EvaluatedMove(piece, movement.Origin, movement.Destination, score, description));
            }
        }

        // Sort by score descending and return top moves
        return allMoves
            .OrderByDescending(m => m.Score)
            .Take(topCount)
            .ToList();
    }

}
