using System;
using System.IO;
using System.Linq;
using Chess;
using Chess.Notation;
using Xunit;
using Xunit.Abstractions;

namespace Chess.IntegrationTests;

/// <summary>
/// Integration tests using real games from Lichess.org
/// These tests validate that the chess engine can successfully parse and apply
/// complete games from real matches, ensuring the notation reader and move
/// application logic work correctly with real-world data.
/// </summary>
public class LichessGameTests
{
    private readonly ITestOutputHelper _output;

    public LichessGameTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData("game1.txt", "Gvein vs DrNykterstein (Black wins)", 48, 6, 7)]
    [InlineData("game2.txt", "WowFlow vs DrNykterstein (Black wins)", 30, 11, 12)]
    [InlineData("game3.txt", "DrNykterstein vs Think_Fast_Move_Fast (White wins)", 58, 5, 3)]
    public void RealLichessGame_ShouldParseAndApplyAllMoves(
        string filename,
        string gameName,
        int expectedTurns,
        int expectedWhitePieces,
        int expectedBlackPieces)
    {
        // Arrange
        _output.WriteLine($"Testing: {gameName}");

        var testDataPath = Path.Combine(
            Path.GetDirectoryName(typeof(LichessGameTests).Assembly.Location)!,
            "TestData",
            filename);

        var notation = File.ReadAllText(testDataPath).Trim();
        _output.WriteLine($"Notation length: {notation.Length} characters");

        var reader = new AlgebraicNotationReader();
        var board = new Board();

        // Act - Parse notation
        var turns = ParseNotation(notation, reader);

        // Assert - Verify parsing
        Assert.Equal(expectedTurns, turns.Count);
        _output.WriteLine($"✓ Successfully parsed {turns.Count} turns");

        // Act - Apply all moves
        int successfulTurns = 0;
        for (int i = 0; i < turns.Count; i++)
        {
            var turn = turns[i];

            try
            {
                board.ApplyTurn(turn);
                successfulTurns++;

                if ((i + 1) % 10 == 0)
                {
                    _output.WriteLine($"  Applied turn {i + 1}...");
                }
            }
            catch (Exception ex)
            {
                _output.WriteLine($"✗ Failed at turn {i + 1}: {ex.Message}");
                throw new Exception($"Failed to apply turn {i + 1}", ex);
            }
        }

        // Assert - Verify all moves applied successfully
        Assert.Equal(expectedTurns, successfulTurns);
        _output.WriteLine($"✓ Successfully applied all {successfulTurns} turns");

        // Assert - Verify final board state
        var whitePieces = board.Pieces.Where(p => p.Colour == PieceColour.White).ToList();
        var blackPieces = board.Pieces.Where(p => p.Colour == PieceColour.Black).ToList();

        Assert.Equal(expectedWhitePieces, whitePieces.Count);
        Assert.Equal(expectedBlackPieces, blackPieces.Count);

        _output.WriteLine($"Final board state:");
        _output.WriteLine($"  White pieces: {whitePieces.Count}");
        _output.WriteLine($"  Black pieces: {blackPieces.Count}");

        // Verify kings are present
        var whiteKing = board.Pieces.FirstOrDefault(p => p.IsKing && p.IsWhite);
        var blackKing = board.Pieces.FirstOrDefault(p => p.IsKing && p.IsBlack);

        Assert.NotNull(whiteKing);
        Assert.NotNull(blackKing);

        _output.WriteLine($"  White King: {whiteKing.Position}");
        _output.WriteLine($"  Black King: {blackKing.Position}");

        // Check for check/checkmate
        var analysis = new BoardAnalysis(board);
        bool whiteInCheck = analysis.IsKingInCheck(PieceColour.White);
        bool blackInCheck = analysis.IsKingInCheck(PieceColour.Black);

        if (whiteInCheck)
        {
            _output.WriteLine("  ⚠ White king is in check!");
        }
        if (blackInCheck)
        {
            _output.WriteLine("  ⚠ Black king is in check!");
        }
    }

    [Fact]
    public void AllLichessGames_ShouldComplete100Percent()
    {
        // This meta-test verifies that all game files can be fully processed
        var testDataPath = Path.Combine(
            Path.GetDirectoryName(typeof(LichessGameTests).Assembly.Location)!,
            "TestData");

        var gameFiles = Directory.GetFiles(testDataPath, "game*.txt");

        Assert.NotEmpty(gameFiles);
        _output.WriteLine($"Found {gameFiles.Length} game files to test");

        foreach (var gameFile in gameFiles)
        {
            var notation = File.ReadAllText(gameFile).Trim();
            var reader = new AlgebraicNotationReader();
            var board = new Board();

            var turns = ParseNotation(notation, reader);

            foreach (var turn in turns)
            {
                board.ApplyTurn(turn);
            }

            _output.WriteLine($"✓ {Path.GetFileName(gameFile)}: {turns.Count} turns applied successfully");
        }
    }

    private System.Collections.Generic.List<NotedTurn> ParseNotation(string notation, AlgebraicNotationReader reader)
    {
        var turns = new System.Collections.Generic.List<NotedTurn>();

        // Parse turns in Lichess format: "1. e4 e5" (one turn per line)
        // The AlgebraicNotationReader now handles this format natively
        var lines = notation.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var turn = reader.ReadTurn(line);
            turns.Add(turn);
        }

        return turns;
    }
}
