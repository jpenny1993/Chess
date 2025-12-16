namespace Chess.Evaluation;

/// <summary>
/// Defines evaluation strategy weights for different game phases.
/// Weights are in basis points (1/100th of a percent) and should sum to 10000 (100%).
/// Based on Grandmaster strategic guidance.
/// </summary>
public class EvaluationWeights
{
    // Opening phase weights (moves 1-12)
    // Emphasis: Development and center control
    public int MaterialGain_Opening { get; set; } = 500;           // 5%
    public int Checkmate_Opening { get; set; } = 0;               // 0% (context dependent)
    public int PieceActivity_Opening { get; set; } = 1500;        // 15%
    public int CenterControl_Opening { get; set; } = 2500;        // 25%
    public int PawnStructure_Opening { get; set; } = 1000;        // 10%
    public int PieceDevelopment_Opening { get; set; } = 2500;     // 25%
    public int KingSafety_Opening { get; set; } = 2000;           // 20%

    // Middlegame phase weights (moves 12-35)
    // Emphasis: Material and piece activity
    public int MaterialGain_Middlegame { get; set; } = 2500;      // 25%
    public int Checkmate_Middlegame { get; set; } = 0;            // 0% (context dependent)
    public int PieceActivity_Middlegame { get; set; } = 2500;     // 25%
    public int CenterControl_Middlegame { get; set; } = 1000;     // 10%
    public int PawnStructure_Middlegame { get; set; } = 1500;     // 15%
    public int PieceDevelopment_Middlegame { get; set; } = 500;   // 5%
    public int KingSafety_Middlegame { get; set; } = 2000;        // 20%

    // Endgame phase weights (queens off or material < 26)
    // Emphasis: Pawn structure and king activity
    public int MaterialGain_Endgame { get; set; } = 2000;         // 20%
    public int Checkmate_Endgame { get; set; } = 0;               // 0% (context dependent)
    public int PieceActivity_Endgame { get; set; } = 1500;        // 15%
    public int CenterControl_Endgame { get; set; } = 500;         // 5%
    public int PawnStructure_Endgame { get; set; } = 3000;        // 30%
    public int PieceDevelopment_Endgame { get; set; } = 0;        // 0% (irrelevant)
    public int KingSafety_Endgame { get; set; } = 1500;           // 15% (king activity)

    /// <summary>
    /// Gets weights for a specific game phase.
    /// </summary>
    public PhaseWeights GetWeightsForPhase(GamePhaseDetector.GamePhase phase)
    {
        return phase switch
        {
            GamePhaseDetector.GamePhase.Opening => new PhaseWeights
            {
                MaterialGain = MaterialGain_Opening,
                Checkmate = Checkmate_Opening,
                PieceActivity = PieceActivity_Opening,
                CenterControl = CenterControl_Opening,
                PawnStructure = PawnStructure_Opening,
                PieceDevelopment = PieceDevelopment_Opening,
                KingSafety = KingSafety_Opening
            },
            GamePhaseDetector.GamePhase.Middlegame => new PhaseWeights
            {
                MaterialGain = MaterialGain_Middlegame,
                Checkmate = Checkmate_Middlegame,
                PieceActivity = PieceActivity_Middlegame,
                CenterControl = CenterControl_Middlegame,
                PawnStructure = PawnStructure_Middlegame,
                PieceDevelopment = PieceDevelopment_Middlegame,
                KingSafety = KingSafety_Middlegame
            },
            GamePhaseDetector.GamePhase.Endgame => new PhaseWeights
            {
                MaterialGain = MaterialGain_Endgame,
                Checkmate = Checkmate_Endgame,
                PieceActivity = PieceActivity_Endgame,
                CenterControl = CenterControl_Endgame,
                PawnStructure = PawnStructure_Endgame,
                PieceDevelopment = PieceDevelopment_Endgame,
                KingSafety = KingSafety_Endgame
            },
            _ => throw new ArgumentException($"Unknown game phase: {phase}")
        };
    }

    /// <summary>
    /// Creates a copy of these weights for modification.
    /// </summary>
    public EvaluationWeights Clone()
    {
        return new EvaluationWeights
        {
            MaterialGain_Opening = MaterialGain_Opening,
            Checkmate_Opening = Checkmate_Opening,
            PieceActivity_Opening = PieceActivity_Opening,
            CenterControl_Opening = CenterControl_Opening,
            PawnStructure_Opening = PawnStructure_Opening,
            PieceDevelopment_Opening = PieceDevelopment_Opening,
            KingSafety_Opening = KingSafety_Opening,
            MaterialGain_Middlegame = MaterialGain_Middlegame,
            Checkmate_Middlegame = Checkmate_Middlegame,
            PieceActivity_Middlegame = PieceActivity_Middlegame,
            CenterControl_Middlegame = CenterControl_Middlegame,
            PawnStructure_Middlegame = PawnStructure_Middlegame,
            PieceDevelopment_Middlegame = PieceDevelopment_Middlegame,
            KingSafety_Middlegame = KingSafety_Middlegame,
            MaterialGain_Endgame = MaterialGain_Endgame,
            Checkmate_Endgame = Checkmate_Endgame,
            PieceActivity_Endgame = PieceActivity_Endgame,
            CenterControl_Endgame = CenterControl_Endgame,
            PawnStructure_Endgame = PawnStructure_Endgame,
            PieceDevelopment_Endgame = PieceDevelopment_Endgame,
            KingSafety_Endgame = KingSafety_Endgame
        };
    }
}

/// <summary>
/// Weights for a single game phase.
/// </summary>
public class PhaseWeights
{
    public int MaterialGain { get; set; }
    public int Checkmate { get; set; }
    public int PieceActivity { get; set; }
    public int CenterControl { get; set; }
    public int PawnStructure { get; set; }
    public int PieceDevelopment { get; set; }
    public int KingSafety { get; set; }

    /// <summary>
    /// Normalizes weights so they sum to 10000 (100%).
    /// </summary>
    public PhaseWeights Normalize()
    {
        int total = MaterialGain + Checkmate + PieceActivity + CenterControl + PawnStructure + PieceDevelopment + KingSafety;

        if (total == 0)
        {
            throw new InvalidOperationException("Cannot normalize weights that sum to zero");
        }

        return new PhaseWeights
        {
            MaterialGain = (MaterialGain * 10000) / total,
            Checkmate = (Checkmate * 10000) / total,
            PieceActivity = (PieceActivity * 10000) / total,
            CenterControl = (CenterControl * 10000) / total,
            PawnStructure = (PawnStructure * 10000) / total,
            PieceDevelopment = (PieceDevelopment * 10000) / total,
            KingSafety = (KingSafety * 10000) / total
        };
    }
}
