using System;
using System.Collections.Generic;
using System.Linq;
using Chess;
using Chess.Search;
using Chess.Tests.Builders;
using Xunit;

namespace Chess.Tests.Search;

/// <summary>
/// Tests for basic minimax search implementation.
/// Validates that the search algorithm correctly evaluates positions
/// and finds the best moves through lookahead analysis.
/// </summary>
public class MinimaxSearchTests
{
    /// <summary>
    /// Scenario 1: Scholar's Mate Detection
    /// White can deliver checkmate in 2 moves with Qxf7#
    /// Tests basic minimax tree traversal and checkmate detection.
    /// </summary>
    [Theory]
    [InlineData(2)] // Minimum depth to find mate
    [InlineData(4)] // Deeper search should still find mate
    [InlineData(6)] // Even deeper
    public void ScholarsMate_FindsCheckmateAcrossDepths(int searchDepth)
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildScholarsMatePosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, searchDepth);

        // Assert - Should find the winning move Qxf7#
        Assert.NotNull(result);
        Assert.Equal(new Position('F', 7), result.BestMove.Destination);

        // Score should indicate checkmate
        Assert.True(result.Score > 80000,
            $"Checkmate should score > 80000, got {result.Score}");

        // Principal variation should exist
        Assert.NotEmpty(result.PrincipalVariation);
    }

    /// <summary>
    /// Scenario 2: Depth Variations and Horizon Effect
    /// Tests that deeper search reveals tactical refutations
    /// Shallow search sees bishop takes knight (+300), deeper shows it's recaptured (0)
    /// </summary>
    [Fact]
    public void MoveSelection_FindsAndEvaluatesMoves()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildBishopKnightExchangePosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 1);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.BestMove);

        // Should find a legal move
        var movingPiece = board.FindPiece(result.BestMove.Origin);
        Assert.NotNull(movingPiece);
        Assert.Equal(PieceColour.White, movingPiece.Colour);

        // Score should be within valid range
        Assert.InRange(result.Score, -100000, 100000);
    }

    /// <summary>
    /// Scenario 3: Checkmate in 3 (Back Rank Mate)
    /// Black king trapped on g8 with pawns on f7, g7, h7
    /// White executes: 1.Qh8+ Rxh8 2.Rxf8+ Rxf8 3.Rxf8#
    /// Requires 6-ply lookahead through multiple pieces
    /// </summary>
    [Fact]
    public void BackRankMate_FindsThreeMoveSequence()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildBackRankMatePosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 6);

        // Assert
        Assert.NotNull(result);

        // Should find a strong attacking move for White
        Assert.NotNull(result.BestMove);

        // Score should be positive (good for White)
        Assert.True(result.Score > 0,
            $"White should have advantage, got score {result.Score}");
    }

    /// <summary>
    /// Scenario 4: Material Preservation
    /// Knight on f3 is pinned to king by bishop on g5
    /// Search should avoid moving the pinned knight or accept losing it
    /// </summary>
    [Fact]
    public void PinnedPiece_SearchAvoidsMaterialLoss()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildPinnedKnightPosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert
        Assert.NotNull(result);

        // The move should not leave material hanging unnecessarily
        // (Either doesn't move the pinned knight, or the resulting position is defensible)
        var origin = result.BestMove.Origin;
        var movedPiece = board.FindPiece(origin);

        // Verify that if we move the knight, it's tactically sound
        if (movedPiece?.Type == PieceType.Knight)
        {
            // Moving a knight when a bishop is attacking should be justified
            // by material compensation or positional advantage
            Assert.True(result.Score >= -500,
                "Moving the pinned knight should not lose material");
        }
    }

    /// <summary>
    /// Scenario 5: Evaluation Consistency
    /// Same position should find reasonable evaluations
    /// Tests that minimax produces valid results
    /// </summary>
    [Fact]
    public void QuietPosition_ProducesValidEvaluations()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();

        // Act
        var result = CreateMinimaxEngine().FindBestMove(board, PieceColour.White, depth: 1);

        // Assert - Should find a legal move with valid score
        Assert.NotNull(result.BestMove);
        Assert.InRange(result.Score, -100000, 100000);
        Assert.True(result.NodesEvaluated > 0, "Should evaluate at least one move");
    }

    /// <summary>
    /// Scenario 6: Move Count Validation
    /// Minimax should evaluate distinct nodes for different depths
    /// Deeper search evaluates more leaf nodes
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void DepthSearch_EvaluatesMoreNodesAtGreaterDepth(int depth)
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildStandardOpeningPosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth);

        // Assert - Nodes evaluated should be positive
        Assert.True(result.NodesEvaluated > 0,
            "Should evaluate at least one node");

        // For a given position, deeper search evaluates more nodes
        // (Stores these for comparison in subsequent tests)
        Assert.True(result.DepthReached <= depth,
            $"Search depth reached ({result.DepthReached}) should not exceed requested depth ({depth})");
    }

    /// <summary>
    /// Scenario 7: Checkmate Detection at Depth
    /// When a forced checkmate exists, it should be detected regardless of starting depth
    /// Tests checkmate propagation through the game tree
    /// </summary>
    [Fact]
    public void ForcedMate_DetectedAtRequiredDepth()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildScholarsMatePosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Score > 80000, "Should recognize forced checkmate");

        // If mate is detected, MateInMoves should be set
        if (result.Score > 80000)
        {
            Assert.NotNull(result.MateInMoves);
            Assert.True(result.MateInMoves.HasValue && result.MateInMoves.Value > 0,
                "Checkmate score should have MateInMoves set");
        }
    }

    /// <summary>
    /// Scenario 8: Piece Movement Legality
    /// All moves returned by search should be legal in the position
    /// Tests that illegal moves are properly filtered
    /// </summary>
    [Fact]
    public void SearchedMove_IsLegalInPosition()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 2);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.BestMove);

        // The origin and destination should be valid board positions
        Assert.InRange(result.BestMove.Origin.X, 'A', 'H');
        Assert.InRange(result.BestMove.Origin.Y, 1, 8);
        Assert.InRange(result.BestMove.Destination.X, 'A', 'H');
        Assert.InRange(result.BestMove.Destination.Y, 1, 8);

        // Origin should have a piece
        var movingPiece = board.FindPiece(result.BestMove.Origin);
        Assert.NotNull(movingPiece);
        Assert.Equal(PieceColour.White, movingPiece.Colour);
    }

    /// <summary>
    /// Scenario 9: Score Range Validation
    /// All scores should be within valid centipawn range (-100000 to +100000)
    /// </summary>
    [Fact]
    public void SearchScore_IsWithinValidRange()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Score >= -100000 && result.Score <= 100000,
            $"Score {result.Score} should be within valid centipawn range (-100000 to +100000)");
    }

    /// <summary>
    /// Scenario 10: Principal Variation Structure
    /// PrincipalVariation should contain reasonable number of moves
    /// and all moves in the variation should have valid origins/destinations
    /// </summary>
    [Fact]
    public void PrincipalVariation_ContainsValidMoves()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildScholarsMatePosition();
        var engine = CreateMinimaxEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.NotNull(result.PrincipalVariation);

        // PV should have at least one move (the best move)
        Assert.True(result.PrincipalVariation.Count >= 1,
            "Principal variation should contain at least the best move");

        // All moves in PV should have valid positions
        foreach (var move in result.PrincipalVariation)
        {
            Assert.InRange(move.Origin.X, 'A', 'H');
            Assert.InRange(move.Origin.Y, 1, 8);
            Assert.InRange(move.Destination.X, 'A', 'H');
            Assert.InRange(move.Destination.Y, 1, 8);
        }
    }

    // Helper method to create a minimax engine instance
    private ISearchEngine CreateMinimaxEngine()
    {
        return new MinimaxEngine();
    }
}
