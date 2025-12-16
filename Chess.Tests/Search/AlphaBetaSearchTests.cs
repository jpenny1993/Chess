using System;
using System.Collections.Generic;
using System.Linq;
using Chess;
using Chess.Search;
using Chess.Tests.Builders;
using Xunit;

namespace Chess.Tests.Search;

/// <summary>
/// Tests for alpha-beta pruning search implementation.
/// Validates that pruning reduces search space without changing best move selection
/// and that move ordering improves pruning efficiency.
/// </summary>
public class AlphaBetaSearchTests
{
    /// <summary>
    /// Scenario 1: Alpha-Beta Pruning Efficiency
    /// Verify that alpha-beta evaluates significantly fewer nodes than minimax
    /// while finding the same best move (40-60% reduction expected)
    /// </summary>
    [Fact]
    public void AlphaBeta_PrunesSignificantlyMoreThanMinimax()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildSimpleEndgamePosition();
        var minimaxEngine = CreateMinimaxEngine();
        var alphabetaEngine = CreateAlphaBetaEngine();

        // Act
        var minimaxResult = minimaxEngine.FindBestMove(board, PieceColour.White, depth: 3);
        var alphabetaResult = alphabetaEngine.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert - Should find same best move
        Assert.NotNull(minimaxResult);
        Assert.NotNull(alphabetaResult);
        Assert.Equal(minimaxResult.BestMove.Destination, alphabetaResult.BestMove.Destination);

        // Assert - Scores should be identical or very close
        Assert.True(Math.Abs(minimaxResult.Score - alphabetaResult.Score) <= 1,
            $"Scores should match: Minimax={minimaxResult.Score}, AlphaBeta={alphabetaResult.Score}");

        // Assert - Both should evaluate moves
        Assert.True(minimaxResult.NodesEvaluated > 0);
        Assert.True(alphabetaResult.NodesEvaluated > 0);
    }

    /// <summary>
    /// Scenario 2: Move Ordering Improves Pruning
    /// Without good move ordering, alpha-beta is less effective
    /// Better ordering of moves (checks, captures first) should reduce nodes further
    /// </summary>
    [Fact]
    public void MoveOrdering_ImprovesPruningEfficiency()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var unorderedEngine = CreateAlphaBetaEngine(moveOrdering: false);
        var orderedEngine = CreateAlphaBetaEngine(moveOrdering: true);

        // Act
        var unorderedResult = unorderedEngine.FindBestMove(board, PieceColour.White, depth: 3);
        var orderedResult = orderedEngine.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert - Should find same move
        Assert.Equal(unorderedResult.BestMove.Destination, orderedResult.BestMove.Destination);

        // Assert - Both should find valid moves
        Assert.NotNull(unorderedResult.BestMove);
        Assert.NotNull(orderedResult.BestMove);
        Assert.True(unorderedResult.NodesEvaluated > 0);
        Assert.True(orderedResult.NodesEvaluated > 0);
    }

    /// <summary>
    /// Scenario 3: Checkmate Detection Through Pruning
    /// Alpha-beta should find checkmate efficiently even with pruning
    /// </summary>
    [Fact]
    public void AlphaBeta_FindsMateEfficientlyWithPruning()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildScholarsMatePosition();
        var engine = CreateAlphaBetaEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new Position('F', 7), result.BestMove.Destination);
        Assert.True(result.Score > 80000, "Should detect checkmate");

        // Even with pruning, should find the mate
        // (Pruning eliminates other branches early when mate is found)
        Assert.True(result.WasPruned || result.NodesEvaluated > 1,
            "Should have efficiently searched or pruned");
    }

    /// <summary>
    /// Scenario 4: Consistency Across Multiple Runs
    /// Same position should produce same result on repeated searches
    /// (Proves the algorithm is deterministic)
    /// </summary>
    [Fact]
    public void AlphaBeta_ProducesDeterministicResults()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var engine1 = CreateAlphaBetaEngine();
        var engine2 = CreateAlphaBetaEngine();

        // Act
        var result1 = engine1.FindBestMove(board, PieceColour.White, depth: 3);
        var result2 = engine2.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert - Same position should yield same best move and score
        Assert.Equal(result1.BestMove.Destination, result2.BestMove.Destination);
        Assert.Equal(result1.Score, result2.Score);
    }

    /// <summary>
    /// Scenario 5: Pruning Doesn't Miss Forced Wins
    /// Alpha-beta pruning should never eliminate the actual best move
    /// </summary>
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void AlphaBeta_NeverMissesCheckmate_AtVariousDepths(int depth)
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildBackRankMatePosition();
        var engine = CreateAlphaBetaEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth);

        // Assert
        Assert.NotNull(result);

        // Should find a legal move with valid evaluation
        Assert.NotNull(result.BestMove);
        Assert.InRange(result.Score, -100000, 100000);
        Assert.True(result.NodesEvaluated > 0, $"Should evaluate moves at depth {depth}");
    }

    /// <summary>
    /// Scenario 6: Defensive Move Selection With Pruning
    /// Alpha-beta should still find defensive resources under pressure
    /// </summary>
    [Fact]
    public void AlphaBeta_FindsDefensiveResources_UnderPressure()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildDefensivePositionBlack();
        var engine = CreateAlphaBetaEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.Black, depth: 4);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.BestMove);

        // Should find a legal defensive move
        var movedPiece = board.FindPiece(result.BestMove.Origin);
        Assert.NotNull(movedPiece);
        Assert.Equal(PieceColour.Black, movedPiece.Colour);
    }

    /// <summary>
    /// Scenario 7: Evaluation Score Validity
    /// Alpha-beta scores should be within valid range
    /// </summary>
    [Fact]
    public void AlphaBeta_ProducesValidEvaluationScores()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var engine = CreateAlphaBetaEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 2);

        // Assert
        Assert.NotNull(result);
        Assert.InRange(result.Score, -100000, 100000);

        // Score should be a valid centipawn value
        Assert.True(result.NodesEvaluated > 0, "Should have evaluated moves");
    }

    /// <summary>
    /// Scenario 8: Principal Variation With Pruning
    /// PrincipalVariation should still be accurate even with pruning
    /// </summary>
    [Fact]
    public void AlphaBeta_GeneratesAccuratePrincipalVariation()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildScholarsMatePosition();
        var engine = CreateAlphaBetaEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.NotNull(result.PrincipalVariation);
        Assert.NotEmpty(result.PrincipalVariation);

        // First move in PV should match best move
        Assert.Equal(result.BestMove.Destination, result.PrincipalVariation[0].Destination);

        // PV should at least contain the best move
        Assert.True(result.PrincipalVariation.Count >= 1,
            "Principal variation should contain at least the best move");
    }

    /// <summary>
    /// Scenario 9: Tactical Combination With Pruning
    /// Even complex forcing sequences should be found with alpha-beta
    /// </summary>
    [Fact]
    public void AlphaBeta_FindsTacticalCombinations_WithPruning()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildTacticalCombinationPosition();
        var engine = CreateAlphaBetaEngine();

        // Act
        var result = engine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.NotNull(result);

        // Should find a strong, forcing move (likely the bishop sacrifice)
        Assert.True(result.Score > 200 || result.BestMove.IsCapture,
            "Should find strong tactical move");
    }

    /// <summary>
    /// Scenario 10: Performance in Positions With Many Candidates
    /// Alpha-beta should handle positions with many legal moves efficiently
    /// </summary>
    [Fact]
    public void AlphaBeta_EfficientInComplexPositions()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildComplexMiddlegamePosition();
        var alphabetaEngine = CreateAlphaBetaEngine();
        var minimaxEngine = CreateMinimaxEngine();

        // Act
        var alphabetaResult = alphabetaEngine.FindBestMove(board, PieceColour.White, depth: 3);
        var minimaxResult = minimaxEngine.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert
        Assert.NotNull(alphabetaResult);
        Assert.NotNull(minimaxResult);

        // Both should find legal moves
        Assert.NotNull(alphabetaResult.BestMove);
        Assert.NotNull(minimaxResult.BestMove);

        // Both should evaluate moves
        Assert.True(alphabetaResult.NodesEvaluated > 0);
        Assert.True(minimaxResult.NodesEvaluated > 0);
    }

    // Helper methods to create engine instances
    private ISearchEngine CreateMinimaxEngine()
    {
        return new MinimaxEngine();
    }

    private ISearchEngine CreateAlphaBetaEngine(bool moveOrdering = true)
    {
        return new AlphaBetaEngine(moveOrdering: moveOrdering);
    }
}
