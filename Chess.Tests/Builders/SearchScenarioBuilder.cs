using Chess;
using Chess.Tests.Builders;

namespace Chess.Tests.Search;

/// <summary>
/// Provides specific chess positions for depth-based search testing.
/// Each position is designed to test a particular aspect of minimax/alpha-beta search.
/// </summary>
public static class SearchScenarioBuilder
{
    /// <summary>
    /// Scenario 1: Scholar's Mate - White can deliver checkmate in 2 moves with Qxf7#
    /// Position from move 3 (White to move on move 4)
    /// Previous: 1.e4 e5 2.Bc4 Nc6 3.Qh5 Nf6
    /// </summary>
    public static Board BuildScholarsMatePosition()
    {
        return new ChessBoardBuilder()
            // White pieces
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("H5", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C4", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("G1", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D2", PieceColour.White)
            .SetPawnAt("E4", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black pieces
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("H8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("B8", PieceColour.Black)
            .SetKnightAt("F6", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 2: Simple Endgame - King + Queen + Rooks vs King
    /// Used for alpha-beta pruning efficiency tests
    /// </summary>
    public static Board BuildSimpleEndgamePosition()
    {
        return new ChessBoardBuilder()
            // White: K + Q + 2R
            .SetKingAt("G2", PieceColour.White)
            .SetQueenAt("D2", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            // Black: K only
            .SetKingAt("F5", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 3: Bishop-Knight Exchange - Tests horizon effect detection
    /// White can capture knight on c6, but recapture neutralizes the gain
    /// </summary>
    public static Board BuildBishopKnightExchangePosition()
    {
        return new ChessBoardBuilder()
            // White
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("D1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C4", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("F3", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D4", PieceColour.White)
            .SetPawnAt("E4", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("H8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("C6", PieceColour.Black)
            .SetKnightAt("F6", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 4: Back Rank Mate - Checkmate in 3 moves
    /// King trapped on g8 with pawns on f7, g7, h7
    /// White can execute: 1.Qh8+ Rxh8 2.Rxf8+ Rxf8 3.Rxf8#
    /// </summary>
    public static Board BuildBackRankMatePosition()
    {
        return new ChessBoardBuilder()
            // White
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("H1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("F1", PieceColour.White)
            // Black
            .SetKingAt("G8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("F8", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E7", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 5: Pinned Knight - Knight on f3 is pinned to king by bishop on g5
    /// Tests that search avoids moving pinned pieces or breaks the pin
    /// </summary>
    public static Board BuildPinnedKnightPosition()
    {
        return new ChessBoardBuilder()
            // White
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("D1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C1", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("F3", PieceColour.White) // This knight is pinned
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D2", PieceColour.White)
            .SetPawnAt("E4", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("H8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("G5", PieceColour.Black) // Pins the knight
            .SetKnightAt("B8", PieceColour.Black)
            .SetKnightAt("F6", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 6: Defensive Position - Black to move, must defend against threats
    /// Tests that Solid style finds defensive moves (e.g., Re8)
    /// </summary>
    public static Board BuildDefensivePositionBlack()
    {
        return new ChessBoardBuilder()
            // White
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("D1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C1", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("C3", PieceColour.White)
            .SetKnightAt("F3", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D4", PieceColour.White)
            .SetPawnAt("E4", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("E8", PieceColour.Black) // Can defend with Re-file
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("B8", PieceColour.Black)
            .SetKnightAt("F6", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 7: Tactical Combination - Bxc6 sacrifice with discovered attack on queen
    /// Tests that Aggressive style finds forcing sacrifices
    /// </summary>
    public static Board BuildTacticalCombinationPosition()
    {
        return new ChessBoardBuilder()
            // White
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("D1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C4", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("F3", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D4", PieceColour.White)
            .SetPawnAt("E4", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("H8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("B8", PieceColour.Black)
            .SetKnightAt("C6", PieceColour.Black) // Can be sacrificed
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 8: Standard Opening Position - Used for move ordering tests
    /// Starting position after 1.e4 e5
    /// </summary>
    public static Board BuildStandardOpeningPosition()
    {
        return new Board();
    }

    /// <summary>
    /// Scenario 9: Quiet Middlegame Position - Italian Game setup
    /// Used for evaluation consistency tests across depths
    /// </summary>
    public static Board BuildQuietMiddlegamePosition()
    {
        return new ChessBoardBuilder()
            // White
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("D1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C4", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("F3", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D4", PieceColour.White)
            .SetPawnAt("E4", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("H8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("B8", PieceColour.Black)
            .SetKnightAt("F6", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }

    /// <summary>
    /// Scenario 10: Complex Middlegame - Multiple candidate moves with different strategic goals
    /// Tests play style divergence in realistic positions
    /// </summary>
    public static Board BuildComplexMiddlegamePosition()
    {
        return new ChessBoardBuilder()
            // White
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("D1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("F1", PieceColour.White)
            .SetBishopAt("C1", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("F3", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D4", PieceColour.White)
            .SetPawnAt("E5", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("F8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("B8", PieceColour.Black)
            .SetKnightAt("E6", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E7", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();
    }
}
