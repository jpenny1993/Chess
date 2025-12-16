namespace Chess.Search;

/// <summary>
/// Contains hardcoded chess opening theory with popular variations.
/// Maps position fingerprints to weighted move choices for probabilistic play.
/// Covers 10 popular openings at 4-6 moves deep (~50-100 positions).
/// </summary>
public sealed class OpeningBook
{
    private readonly Dictionary<PositionFingerprint, List<OpeningMove>> _bookMoves;
    private readonly Random _random;

    /// <summary>
    /// Creates an OpeningBook with the default random seed.
    /// </summary>
    public OpeningBook() : this(new Random()) { }

    /// <summary>
    /// Creates an OpeningBook with a specific Random instance.
    /// Useful for deterministic testing with seeded random.
    /// </summary>
    public OpeningBook(Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
        _bookMoves = InitializeOpeningTheory();
    }

    /// <summary>
    /// Looks up available moves for the given position in the opening book.
    /// </summary>
    /// <param name="board">Current board position</param>
    /// <returns>List of book moves if position is found, empty list otherwise</returns>
    public IReadOnlyList<OpeningMove> GetBookMoves(Board board)
    {
        var fingerprint = new PositionFingerprint(board);

        if (_bookMoves.TryGetValue(fingerprint, out var moves))
        {
            return moves.AsReadOnly();
        }

        return Array.Empty<OpeningMove>();
    }

    /// <summary>
    /// Selects a move using weighted random selection.
    /// Higher weight moves have higher probability of selection.
    /// </summary>
    /// <param name="board">Current position</param>
    /// <param name="colour">Side to move</param>
    /// <returns>Selected opening move, or null if position not in book</returns>
    public OpeningMove? SelectBookMove(Board board, PieceColour colour)
    {
        var bookMoves = GetBookMoves(board);

        if (bookMoves.Count == 0)
            return null;

        // Calculate total weight
        var totalWeight = bookMoves.Sum(m => m.Weight);

        // Select random value in range [0, totalWeight)
        var randomValue = _random.Next(totalWeight);

        // Find the move corresponding to this random value
        var cumulativeWeight = 0;
        foreach (var move in bookMoves)
        {
            cumulativeWeight += move.Weight;
            if (randomValue < cumulativeWeight)
                return move;
        }

        // Fallback (shouldn't reach here with correct weights)
        return bookMoves.Count > 0 ? bookMoves[0] : null;
    }

    /// <summary>
    /// Gets statistics about the opening book.
    /// </summary>
    public (int PositionCount, int TotalMoves) GetStatistics()
    {
        return (_bookMoves.Count, _bookMoves.Values.Sum(list => list.Count));
    }

    /// <summary>
    /// Initializes the hardcoded opening theory.
    /// Contains 10 popular openings, 4-6 moves deep, ~50-100 positions.
    /// </summary>
    private Dictionary<PositionFingerprint, List<OpeningMove>> InitializeOpeningTheory()
    {
        var builder = new OpeningBookBuilder();

        // === STARTING POSITION (Move 1) ===
        builder.FromStartingPosition()
            .AddMove("e4", weight: 100, "King's Pawn Opening")
            .AddMove("d4", weight: 80, "Queen's Pawn Opening")
            .AddMove("Nf3", weight: 40, "Reti Opening")
            .AddMove("c4", weight: 30, "English Opening");

        // === AFTER 1.e4 (Black's response) ===
        builder.AfterMoves("e4")
            .AddMove("e5", weight: 100, "Open Game")
            .AddMove("c5", weight: 90, "Sicilian Defense")
            .AddMove("e6", weight: 60, "French Defense")
            .AddMove("c6", weight: 50, "Caro-Kann Defense")
            .AddMove("d5", weight: 40, "Scandinavian Defense");

        // === ITALIAN GAME: 1.e4 e5 2.Nf3 ===
        builder.AfterMoves("e4", "e5")
            .AddMove("Nf3", weight: 100, "Standard Knight Move")
            .AddMove("f4", weight: 30, "King's Gambit")
            .AddMove("Bc4", weight: 20, "Bishop's Opening");

        builder.AfterMoves("e4", "e5", "Nf3")
            .AddMove("Nc6", weight: 100, "Standard Response")
            .AddMove("Nf6", weight: 60, "Petrov Defense");

        // Italian Game: 1.e4 e5 2.Nf3 Nc6 3.Bc4
        builder.AfterMoves("e4", "e5", "Nf3", "Nc6")
            .AddMove("Bc4", weight: 70, "Italian Game")
            .AddMove("Bb5", weight: 100, "Ruy Lopez")
            .AddMove("d4", weight: 40, "Scotch Game");

        builder.AfterMoves("e4", "e5", "Nf3", "Nc6", "Bc4")
            .AddMove("Bc5", weight: 100, "Giuoco Piano")
            .AddMove("Nf6", weight: 80, "Two Knights Defense")
            .AddMove("Be7", weight: 50, "Classical");

        builder.AfterMoves("e4", "e5", "Nf3", "Nc6", "Bc4", "Bc5")
            .AddMove("c3", weight: 80, "Giuoco Piano")
            .AddMove("d3", weight: 60, "Giuoco Pianissimo")
            .AddMove("b4", weight: 40, "Evans Gambit");

        // === RUY LOPEZ: 1.e4 e5 2.Nf3 Nc6 3.Bb5 ===
        builder.AfterMoves("e4", "e5", "Nf3", "Nc6", "Bb5")
            .AddMove("a6", weight: 100, "Morphy Defense")
            .AddMove("Nf6", weight: 70, "Berlin Defense")
            .AddMove("Bc5", weight: 40, "Classical Defense");

        builder.AfterMoves("e4", "e5", "Nf3", "Nc6", "Bb5", "a6")
            .AddMove("Ba4", weight: 100, "Main Line")
            .AddMove("Bxc6", weight: 30, "Exchange Variation");

        builder.AfterMoves("e4", "e5", "Nf3", "Nc6", "Bb5", "a6", "Ba4")
            .AddMove("Nf6", weight: 100, "Traditional")
            .AddMove("Be7", weight: 70, "Modern");

        // === SICILIAN DEFENSE: 1.e4 c5 ===
        builder.AfterMoves("e4", "c5")
            .AddMove("Nf3", weight: 100, "Open Sicilian")
            .AddMove("Nc3", weight: 40, "Closed Sicilian")
            .AddMove("c3", weight: 30, "Alapin Variation");

        builder.AfterMoves("e4", "c5", "Nf3")
            .AddMove("d6", weight: 70, "Najdorf/Dragon Setup")
            .AddMove("Nc6", weight: 80, "Old Sicilian")
            .AddMove("e6", weight: 60, "French-Style Sicilian");

        builder.AfterMoves("e4", "c5", "Nf3", "d6")
            .AddMove("d4", weight: 100, "Open Sicilian Main Line");

        builder.AfterMoves("e4", "c5", "Nf3", "d6", "d4")
            .AddMove("cxd4", weight: 100, "Natural Capture");

        builder.AfterMoves("e4", "c5", "Nf3", "d6", "d4", "cxd4")
            .AddMove("Nxd4", weight: 100, "Standard Recapture");

        // === FRENCH DEFENSE: 1.e4 e6 ===
        builder.AfterMoves("e4", "e6")
            .AddMove("d4", weight: 100, "French Defense Main Line")
            .AddMove("d3", weight: 20, "King's Indian Attack");

        builder.AfterMoves("e4", "e6", "d4")
            .AddMove("d5", weight: 100, "French Defense");

        builder.AfterMoves("e4", "e6", "d4", "d5")
            .AddMove("Nc3", weight: 80, "Winawer/Classical")
            .AddMove("Nd2", weight: 70, "Tarrasch Variation")
            .AddMove("exd5", weight: 40, "Exchange Variation");

        // === CARO-KANN DEFENSE: 1.e4 c6 ===
        builder.AfterMoves("e4", "c6")
            .AddMove("d4", weight: 100, "Caro-Kann Main Line");

        builder.AfterMoves("e4", "c6", "d4")
            .AddMove("d5", weight: 100, "Standard Caro-Kann");

        builder.AfterMoves("e4", "c6", "d4", "d5")
            .AddMove("Nc3", weight: 80, "Main Line")
            .AddMove("Nf3", weight: 70, "Positional")
            .AddMove("exd5", weight: 50, "Exchange");

        // === QUEEN'S GAMBIT: 1.d4 d5 2.c4 ===
        builder.AfterMoves("d4")
            .AddMove("d5", weight: 80, "Closed Game")
            .AddMove("Nf6", weight: 100, "Indian Defenses")
            .AddMove("f5", weight: 30, "Dutch Defense");

        builder.AfterMoves("d4", "d5")
            .AddMove("c4", weight: 100, "Queen's Gambit")
            .AddMove("Nf3", weight: 40, "London System");

        builder.AfterMoves("d4", "d5", "c4")
            .AddMove("e6", weight: 80, "Queen's Gambit Declined")
            .AddMove("dxc4", weight: 70, "Queen's Gambit Accepted")
            .AddMove("c6", weight: 60, "Slav Defense")
            .AddMove("Nf6", weight: 50, "Various Lines");

        // === KING'S INDIAN DEFENSE: 1.d4 Nf6 2.c4 g6 ===
        builder.AfterMoves("d4", "Nf6")
            .AddMove("c4", weight: 100, "Standard")
            .AddMove("Nf3", weight: 60, "Various");

        builder.AfterMoves("d4", "Nf6", "c4")
            .AddMove("g6", weight: 70, "King's Indian")
            .AddMove("e6", weight: 100, "Nimzo/Queen's Indian")
            .AddMove("c5", weight: 60, "Benoni");

        builder.AfterMoves("d4", "Nf6", "c4", "g6")
            .AddMove("Nc3", weight: 100, "Standard")
            .AddMove("Nf3", weight: 80, "Flexible");

        builder.AfterMoves("d4", "Nf6", "c4", "g6", "Nc3")
            .AddMove("Bg7", weight: 100, "King's Indian Setup");

        // === NIMZO-INDIAN: 1.d4 Nf6 2.c4 e6 3.Nc3 Bb4 ===
        builder.AfterMoves("d4", "Nf6", "c4", "e6")
            .AddMove("Nc3", weight: 100, "Main Line");

        builder.AfterMoves("d4", "Nf6", "c4", "e6", "Nc3")
            .AddMove("Bb4", weight: 100, "Nimzo-Indian");

        builder.AfterMoves("d4", "Nf6", "c4", "e6", "Nc3", "Bb4")
            .AddMove("Qc2", weight: 80, "Main Line")
            .AddMove("a3", weight: 70, "Classical");

        // === ENGLISH OPENING: 1.c4 ===
        builder.AfterMoves("c4")
            .AddMove("e5", weight: 70, "Symmetrical")
            .AddMove("c5", weight: 80, "Symmetrical")
            .AddMove("Nf6", weight: 60, "Flexible");

        // === RETI OPENING: 1.Nf3 ===
        builder.AfterMoves("Nf3")
            .AddMove("d5", weight: 80, "Central")
            .AddMove("c5", weight: 70, "Flexible")
            .AddMove("Nf6", weight: 60, "Symmetrical");

        return builder.Build();
    }
}
