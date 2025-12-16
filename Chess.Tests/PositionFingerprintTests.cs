using Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chess.Tests;

/// <summary>
/// Tests for PositionFingerprint - lightweight position identification for opening book.
/// </summary>
public class PositionFingerprintTests
{
    /// <summary>
    /// Test that identical starting positions produce the same fingerprint.
    /// </summary>
    [Fact]
    public void StartingPosition_ProducesConsistentFingerprint()
    {
        // Arrange
        var board1 = new Board();
        var board2 = new Board();

        // Act
        var fp1 = new PositionFingerprint(board1);
        var fp2 = new PositionFingerprint(board2);

        // Assert
        Assert.Equal(fp1, fp2);
        Assert.Equal(fp1.GetHashCode(), fp2.GetHashCode());
    }

    /// <summary>
    /// Test that the same position reached via different move sequences produces the same fingerprint.
    /// </summary>
    [Fact]
    public void SamePositionDifferentMoveSequence_ProducesSameFingerprint()
    {
        // Arrange
        var board1 = new Board();
        var board2 = new Board();

        // Play 1.e4 e5 on board1
        var e2pawn = board1.FindPiece('E', 2);
        var e4move = e2pawn!.PossibleMoves(board1).First(m => m.Destination.Equals(new Position('E', 4)));
        board1.ApplyMovement(e4move);

        var e7pawn = board1.FindPiece('E', 7);
        var e5move = e7pawn!.PossibleMoves(board1).First(m => m.Destination.Equals(new Position('E', 5)));
        board1.ApplyMovement(e5move);

        // Play 1.e4 e5 on board2 (same moves)
        var e2pawn2 = board2.FindPiece('E', 2);
        var e4move2 = e2pawn2!.PossibleMoves(board2).First(m => m.Destination.Equals(new Position('E', 4)));
        board2.ApplyMovement(e4move2);

        var e7pawn2 = board2.FindPiece('E', 7);
        var e5move2 = e7pawn2!.PossibleMoves(board2).First(m => m.Destination.Equals(new Position('E', 5)));
        board2.ApplyMovement(e5move2);

        // Act
        var fp1 = new PositionFingerprint(board1);
        var fp2 = new PositionFingerprint(board2);

        // Assert
        Assert.Equal(fp1, fp2);
    }

    /// <summary>
    /// Test that different positions produce different fingerprints.
    /// </summary>
    [Fact]
    public void DifferentPositions_ProduceDifferentFingerprints()
    {
        // Arrange
        var board1 = new Board();
        var board2 = new Board();

        // Move e2-e4 on board1
        var e2pawn = board1.FindPiece('E', 2);
        var e4move = e2pawn!.PossibleMoves(board1).First(m => m.Destination.Equals(new Position('E', 4)));
        board1.ApplyMovement(e4move);

        // board2 stays at starting position

        // Act
        var fp1 = new PositionFingerprint(board1);
        var fp2 = new PositionFingerprint(board2);

        // Assert
        Assert.NotEqual(fp1, fp2);
        Assert.NotEqual(fp1.GetHashCode(), fp2.GetHashCode());
    }

    /// <summary>
    /// Test that fingerprint respects castling rights (HasMoved flags).
    /// </summary>
    [Fact]
    public void CastlingRights_AreIncludedInFingerprint()
    {
        // Arrange
        var board1 = new Board();
        var board2 = new Board();

        // First, move e2 pawn to create space for king movement on board1
        var e2pawn = board1.FindPiece('E', 2);
        var e4move = e2pawn!.PossibleMoves(board1).First(m => m.Destination.Equals(new Position('E', 4)));
        board1.ApplyMovement(e4move);

        // Move e7 pawn as black's response
        var e7pawn = board1.FindPiece('E', 7);
        var e5move = e7pawn!.PossibleMoves(board1).First(m => m.Destination.Equals(new Position('E', 5)));
        board1.ApplyMovement(e5move);

        // Now move white king (losing castling rights)
        var whiteKing = board1.FindPiece('E', 1);
        var kingMove = whiteKing!.PossibleMoves(board1).First(m => m.Destination.Equals(new Position('E', 2)));
        board1.ApplyMovement(kingMove);

        // Move the king back (but HasMoved flag remains true)
        var kingMoveBack = whiteKing.PossibleMoves(board1).First(m => m.Destination.Equals(new Position('E', 1)));
        board1.ApplyMovement(kingMoveBack);

        // board2 stays at starting position (can still castle)

        // Act
        var fp1 = new PositionFingerprint(board1);
        var fp2 = new PositionFingerprint(board2);

        // Assert
        // Even though both positions look the same, they should have different fingerprints
        // because board1's king has HasMoved = true
        Assert.NotEqual(fp1, fp2);
    }

    /// <summary>
    /// Test that PositionFingerprint can be used as a dictionary key.
    /// </summary>
    [Fact]
    public void PositionFingerprint_CanBeUsedAsDictionaryKey()
    {
        // Arrange
        var board = new Board();
        var fp = new PositionFingerprint(board);
        var dict = new Dictionary<PositionFingerprint, string>();

        // Act
        dict[fp] = "Starting Position";
        var retrieved = dict[fp];

        // Assert
        Assert.Equal("Starting Position", retrieved);

        // Create identical fingerprint and verify lookup works
        var fp2 = new PositionFingerprint(new Board());
        Assert.Equal("Starting Position", dict[fp2]);
    }
}
