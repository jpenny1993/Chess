using Chess.Strategies;

namespace Chess.Evaluation;

/// <summary>
/// Composes multiple evaluation strategies with game-phase-aware weighting.
/// Combines independent strategy evaluations into a final move score.
/// </summary>
public class WeightedEvaluationComposer
{
    private readonly MaterialGainStrategy _materialGainStrategy = new();
    private readonly CheckmateStrategy _checkmateStrategy = new();
    private readonly PieceActivityStrategy _pieceActivityStrategy = new();
    private readonly CenterControlStrategy _centerControlStrategy = new();
    private readonly PawnStructureStrategy _pawnStructureStrategy = new();
    private readonly PieceDevelopmentStrategy _pieceDevelopmentStrategy = new();
    private readonly KingSafetyStrategy _kingSafetyStrategy = new();
    private readonly PieceSelfPreservationStrategy _selfPreservationStrategy = new();

    private EvaluationWeights _weights;

    public WeightedEvaluationComposer() : this(new EvaluationWeights())
    {
    }

    public WeightedEvaluationComposer(EvaluationWeights weights)
    {
        _weights = weights ?? throw new ArgumentNullException(nameof(weights));
    }

    /// <summary>
    /// Evaluates a move by composing all strategies with phase-appropriate weights.
    /// </summary>
    public int Evaluate(Board board, Movement movement)
    {
        // Detect game phase
        var gamePhase = GamePhaseDetector.DetectPhase(board);

        // Get phase-specific weights
        var phaseWeights = _weights.GetWeightsForPhase(gamePhase);

        // Normalize weights to sum to 10000 (100%)
        var normalizedWeights = phaseWeights.Normalize();

        // Evaluate each strategy
        int materialScore = _materialGainStrategy.Evaluate(board, movement);
        int checkmateScore = _checkmateStrategy.Evaluate(board, movement);
        int activityScore = _pieceActivityStrategy.Evaluate(board, movement);
        int centerScore = _centerControlStrategy.Evaluate(board, movement);
        int pawnScore = _pawnStructureStrategy.Evaluate(board, movement);
        int developmentScore = _pieceDevelopmentStrategy.Evaluate(board, movement);
        int safetyScore = _kingSafetyStrategy.Evaluate(board, movement);
        int selfPreservationScore = _selfPreservationStrategy.Evaluate(board, movement);

        // Combine with normalized weights
        int totalScore = 0;
        totalScore += (materialScore * normalizedWeights.MaterialGain) / 10000;
        totalScore += (checkmateScore * normalizedWeights.Checkmate) / 10000;
        totalScore += (activityScore * normalizedWeights.PieceActivity) / 10000;
        totalScore += (centerScore * normalizedWeights.CenterControl) / 10000;
        totalScore += (pawnScore * normalizedWeights.PawnStructure) / 10000;
        totalScore += (developmentScore * normalizedWeights.PieceDevelopment) / 10000;
        totalScore += (safetyScore * normalizedWeights.KingSafety) / 10000;
        totalScore += (selfPreservationScore * normalizedWeights.SelfPreservation) / 10000;

        return totalScore;
    }

    /// <summary>
    /// Sets custom evaluation weights for different play styles.
    /// </summary>
    public void SetWeights(EvaluationWeights weights)
    {
        _weights = weights ?? throw new ArgumentNullException(nameof(weights));
    }

    /// <summary>
    /// Creates an aggressive play style (prioritizes checks and material).
    /// </summary>
    public static EvaluationWeights CreateAggressiveWeights()
    {
        var weights = new EvaluationWeights();
        // Increase checkmate weight in all phases
        weights.Checkmate_Opening = 1500;
        weights.Checkmate_Middlegame = 2000;
        weights.Checkmate_Endgame = 1500;
        return weights;
    }

    /// <summary>
    /// Creates a solid, positional play style (balanced weights).
    /// </summary>
    public static EvaluationWeights CreateSolidWeights()
    {
        return new EvaluationWeights(); // Default is solid/balanced
    }

    /// <summary>
    /// Creates a material-focused play style (prioritizes capturing pieces).
    /// </summary>
    public static EvaluationWeights CreateMaterialWeights()
    {
        var weights = new EvaluationWeights();
        // Increase material gain weight in all phases
        weights.MaterialGain_Opening = 1500;
        weights.MaterialGain_Middlegame = 3500;
        weights.MaterialGain_Endgame = 3000;
        return weights;
    }

    /// <summary>
    /// Configures the evaluator for a specific play style.
    /// </summary>
    public void ConfigurePlayStyle(PlayStyle style)
    {
        var weights = style switch
        {
            PlayStyle.Aggressive => CreateAggressiveWeights(),
            PlayStyle.Solid => CreateSolidWeights(),
            PlayStyle.Material => CreateMaterialWeights(),
            _ => throw new ArgumentException($"Unknown play style: {style}", nameof(style))
        };

        SetWeights(weights);
    }
}
