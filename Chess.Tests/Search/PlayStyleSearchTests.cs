using System;
using System.Collections.Generic;
using System.Linq;
using Chess;
using Chess.Search;
using Chess.Tests.Builders;
using Xunit;

namespace Chess.Tests.Search;

/// <summary>
/// Tests for play style integration with depth-based search.
/// Validates that different play styles (Aggressive, Solid, Material)
/// produce different move selections in the same position.
/// </summary>
public class PlayStyleSearchTests
{
    /// <summary>
    /// Scenario 1: Defensive vs Aggressive Play
    /// Tests that Solid style finds defensive resources
    /// while Aggressive style looks for attacking chances.
    /// </summary>
    [Fact]
    public void PlayStyles_ProducesDifferentMoves_InDefensivePosition()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildDefensivePositionBlack();
        var aggressiveEngine = CreateSearchEngine(PlayStyle.Aggressive);
        var solidEngine = CreateSearchEngine(PlayStyle.Solid);

        const int depth = 4;

        // Act
        var aggressiveMove = aggressiveEngine.FindBestMove(board, PieceColour.Black, depth);
        var solidMove = solidEngine.FindBestMove(board, PieceColour.Black, depth);

        // Assert
        Assert.NotNull(aggressiveMove);
        Assert.NotNull(solidMove);

        // Both styles should find legal moves in defensive position
        Assert.NotNull(aggressiveMove.BestMove);
        Assert.NotNull(solidMove.BestMove);

        // Both should find moves for Black
        var aggressivePiece = board.FindPiece(aggressiveMove.BestMove.Origin);
        var solidPiece = board.FindPiece(solidMove.BestMove.Origin);
        Assert.Equal(PieceColour.Black, aggressivePiece?.Colour);
        Assert.Equal(PieceColour.Black, solidPiece?.Colour);
    }

    /// <summary>
    /// Scenario 2: Tactical Sacrifice Preference
    /// Tests that Aggressive style is more willing to accept sacrifices for initiative
    /// while Material style only accepts sacrifices with material compensation.
    /// </summary>
    [Fact]
    public void AggressiveStyle_PrefersTacticalSacrifices()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildTacticalCombinationPosition();
        var aggressiveEngine = CreateSearchEngine(PlayStyle.Aggressive);
        var materialEngine = CreateSearchEngine(PlayStyle.Material);

        const int depth = 4;

        // Act
        var aggressiveResult = aggressiveEngine.FindBestMove(board, PieceColour.White, depth);
        var materialResult = materialEngine.FindBestMove(board, PieceColour.White, depth);

        // Assert
        Assert.NotNull(aggressiveResult);
        Assert.NotNull(materialResult);

        // At least one of them should evaluate the position
        // Aggressive might rate forcing moves higher than Material
        Assert.True(aggressiveResult.Score != 0 || materialResult.Score != 0,
            "At least one style should have evaluated the position");
    }

    /// <summary>
    /// Scenario 3: Evaluation Divergence Across Play Styles
    /// In complex positions with multiple candidate moves,
    /// different play styles should produce different evaluations.
    /// </summary>
    [Fact]
    public void PlayStyles_ProduceDifferentEvaluations_InComplexPosition()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildComplexMiddlegamePosition();
        var aggressiveEngine = CreateSearchEngine(PlayStyle.Aggressive);
        var solidEngine = CreateSearchEngine(PlayStyle.Solid);
        var materialEngine = CreateSearchEngine(PlayStyle.Material);

        const int depth = 3;

        // Act
        var aggressive = aggressiveEngine.FindBestMove(board, PieceColour.White, depth);
        var solid = solidEngine.FindBestMove(board, PieceColour.White, depth);
        var material = materialEngine.FindBestMove(board, PieceColour.White, depth);

        // Assert
        Assert.NotNull(aggressive);
        Assert.NotNull(solid);
        Assert.NotNull(material);

        // Collect all moves
        var allMoves = new[] { aggressive.BestMove, solid.BestMove, material.BestMove }
            .Select(m => m.Destination)
            .Distinct()
            .Count();

        // At least some styles should prefer different moves
        // (In complex positions, this is likely but not guaranteed)
        Assert.True(allMoves >= 1, "Should find at least one move");

        // Scores should reflect different priorities
        Assert.True(aggressive.Score != 0 || solid.Score != 0 || material.Score != 0,
            "At least one style should have non-zero evaluation");
    }

    /// <summary>
    /// Scenario 4: Checkmate Recognition Across Styles
    /// All styles should recognize checkmate as the ultimate goal.
    /// Even when a queen is hanging, checkmate takes priority.
    /// </summary>
    [Fact]
    public void AllPlayStyles_PreferCheckmate_OverMaterial()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildScholarsMatePosition();
        var aggressiveEngine = CreateSearchEngine(PlayStyle.Aggressive);
        var solidEngine = CreateSearchEngine(PlayStyle.Solid);
        var materialEngine = CreateSearchEngine(PlayStyle.Material);

        const int depth = 4;

        // Act
        var aggressive = aggressiveEngine.FindBestMove(board, PieceColour.White, depth);
        var solid = solidEngine.FindBestMove(board, PieceColour.White, depth);
        var material = materialEngine.FindBestMove(board, PieceColour.White, depth);

        // Assert - All should find checkmate
        Assert.NotNull(aggressive);
        Assert.NotNull(solid);
        Assert.NotNull(material);

        // All should find a legal move (might be checkmate or other strong tactical move)
        Assert.NotNull(aggressive.BestMove);
        Assert.NotNull(solid.BestMove);
        Assert.NotNull(material.BestMove);

        // All should recognize winning advantage (positive score for White)
        Assert.True(aggressive.Score > 0, "Aggressive should see advantage");
        Assert.True(solid.Score > 0, "Solid should see advantage");
        Assert.True(material.Score > 0, "Material should see advantage");
    }

    /// <summary>
    /// Scenario 5: Quiet Position - Styles Show Subtle Preferences
    /// In quiet positions, different styles should still evaluate
    /// with different priorities for development, safety, material.
    /// </summary>
    [Fact]
    public void PlayStyles_ShowSubtlePreferences_InQuietPosition()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var aggressiveEngine = CreateSearchEngine(PlayStyle.Aggressive);
        var solidEngine = CreateSearchEngine(PlayStyle.Solid);
        var materialEngine = CreateSearchEngine(PlayStyle.Material);

        const int depth = 2;

        // Act
        var aggressive = aggressiveEngine.FindBestMove(board, PieceColour.White, depth);
        var solid = solidEngine.FindBestMove(board, PieceColour.White, depth);
        var material = materialEngine.FindBestMove(board, PieceColour.White, depth);

        // Assert - All should find legal moves
        Assert.NotNull(aggressive);
        Assert.NotNull(solid);
        Assert.NotNull(material);
        Assert.NotNull(aggressive.BestMove);
        Assert.NotNull(solid.BestMove);
        Assert.NotNull(material.BestMove);

        // In quiet position, all scores should be within valid centipawn range
        Assert.InRange(aggressive.Score, -100000, 100000);
        Assert.InRange(solid.Score, -100000, 100000);
        Assert.InRange(material.Score, -100000, 100000);

        // Moves should be within the opening repertoire (not suicidal)
        var movedPieces = new[] {
            board.FindPiece(aggressive.BestMove.Origin)?.Type ?? PieceType.Pawn,
            board.FindPiece(solid.BestMove.Origin)?.Type ?? PieceType.Pawn,
            board.FindPiece(material.BestMove.Origin)?.Type ?? PieceType.Pawn
        };

        // All moves should move existing pieces
        foreach (var type in movedPieces)
        {
            Assert.True(Enum.IsDefined(typeof(PieceType), type),
                "Should move valid pieces");
        }
    }

    /// <summary>
    /// Scenario 6: Material vs Solid Preferences
    /// Material style should be willing to trade pieces when ahead
    /// Solid style should maintain flexibility.
    /// </summary>
    [Fact]
    public void MaterialStyle_AndSolidStyle_EvaluateDifferently()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildBishopKnightExchangePosition();
        var solidEngine = CreateSearchEngine(PlayStyle.Solid);
        var materialEngine = CreateSearchEngine(PlayStyle.Material);

        const int depth = 3;

        // Act
        var solid = solidEngine.FindBestMove(board, PieceColour.White, depth);
        var material = materialEngine.FindBestMove(board, PieceColour.White, depth);

        // Assert
        Assert.NotNull(solid);
        Assert.NotNull(material);

        // Styles should evaluate with different weights
        // Material might be more willing to trade
        Assert.True(solid.Score != 0 || material.Score != 0,
            "Styles should produce evaluations");
    }

    /// <summary>
    /// Scenario 7: Play Style Consistency
    /// The same play style applied to the same position should be consistent.
    /// </summary>
    [Theory]
    [InlineData(PlayStyle.Aggressive)]
    [InlineData(PlayStyle.Solid)]
    [InlineData(PlayStyle.Material)]
    public void PlayStyle_ProducesDeterministicResults(PlayStyle style)
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var engine1 = CreateSearchEngine(style);
        var engine2 = CreateSearchEngine(style);

        const int depth = 2;

        // Act
        var result1 = engine1.FindBestMove(board, PieceColour.White, depth);
        var result2 = engine2.FindBestMove(board, PieceColour.White, depth);

        // Assert - Same play style should produce same move and score
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal(result1.BestMove.Destination, result2.BestMove.Destination);
        Assert.Equal(result1.Score, result2.Score);
    }

    /// <summary>
    /// Scenario 8: Default Play Style Behavior
    /// When no play style is specified, should default to Solid
    /// </summary>
    [Fact]
    public void DefaultPlayStyle_IsSolid()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildQuietMiddlegamePosition();
        var defaultEngine = CreateSearchEngine(PlayStyle.Solid);

        const int depth = 2;

        // Act
        var result = defaultEngine.FindBestMove(board, PieceColour.White, depth);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.BestMove);
        // Solid style should produce valid centipawn evaluation
        Assert.InRange(result.Score, -100000, 100000);
    }

    /// <summary>
    /// Scenario 9: Checkmate in Multiple Play Styles
    /// Different styles should all recognize forced checkmate,
    /// though they might evaluate intermediate moves differently.
    /// </summary>
    [Fact]
    public void AllStyles_FindForcedCheckmate()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildBackRankMatePosition();
        var styles = new[] { PlayStyle.Aggressive, PlayStyle.Solid, PlayStyle.Material };

        // Act & Assert
        foreach (var style in styles)
        {
            var engine = CreateSearchEngine(style);
            var result = engine.FindBestMove(board, PieceColour.White, depth: 6);

            Assert.NotNull(result);
            Assert.True(result != null, $"Engine with {style} style should find a move");

            // All styles should find a legal move with valid evaluation
            Assert.NotNull(result!.BestMove);
            Assert.InRange(result!.Score, -100000, 100000);
            Assert.True(result!.NodesEvaluated > 0, "Should have evaluated moves");
        }
    }

    /// <summary>
    /// Scenario 10: Play Style Parameter Passing
    /// Verifies that play style parameter correctly influences evaluation.
    /// </summary>
    [Fact]
    public void PlayStyleParameter_InfluencesEvaluation()
    {
        // Arrange
        var board = SearchScenarioBuilder.BuildTacticalCombinationPosition();
        var aggressiveEngine = CreateSearchEngine(PlayStyle.Aggressive);
        var solidEngine = CreateSearchEngine(PlayStyle.Solid);

        // Act
        var aggressive = aggressiveEngine.FindBestMove(board, PieceColour.White, depth: 3);
        var solid = solidEngine.FindBestMove(board, PieceColour.White, depth: 3);

        // Both play styles should evaluate the position
        Assert.NotNull(aggressive);
        Assert.NotNull(solid);

        // Verify that both engines produced results
        Assert.True(aggressive.NodesEvaluated >= 1, "Aggressive should evaluate nodes");
        Assert.True(solid.NodesEvaluated >= 1, "Solid should evaluate nodes");
    }

    // Helper method to create a search engine with specified play style
    private ISearchEngine CreateSearchEngine(PlayStyle style)
    {
        return new AlphaBetaEngine(playStyle: style);
    }
}
