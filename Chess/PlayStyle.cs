namespace Chess;

/// <summary>
/// Defines different strategic play styles for move evaluation and selection.
/// Each style emphasizes different aspects of chess strategy.
/// </summary>
public enum PlayStyle
{
    /// <summary>
    /// Aggressive style prioritizes:
    /// - Checks and forcing moves
    /// - Sacrifices that expose enemy king
    /// - Maintaining initiative and tension
    /// - Risk-taking for attacking chances
    /// </summary>
    Aggressive = 0,

    /// <summary>
    /// Solid/Balanced style prioritizes:
    /// - Sound, balanced development
    /// - King safety and piece defense
    /// - Flexibility and maintaining options
    /// - Avoiding unnecessary risks
    /// - Positional advantages
    /// </summary>
    Solid = 1,

    /// <summary>
    /// Material-focused style prioritizes:
    /// - Winning material (pieces and pawns)
    /// - Safe captures with favorable exchanges
    /// - Simplifying the position
    /// - Concrete material advantage
    /// </summary>
    Material = 2,
}
