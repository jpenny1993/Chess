using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests.Pieces;

public class QueenTests
{
    [Fact]
    public void Queen_Can_Move_In_All_Eight_Directions()
    {
        // Queen at E5 with no obstacles should be able to move to all squares
        // in all 8 directions (4 diagonals + 4 straight lines)
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'Q', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetQueenAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', 'Q', ' ', ' ', 'Q', ' ', ' ', 'Q', // 8
                ' ', ' ', 'Q', ' ', 'Q', ' ', 'Q', ' ', // 7
                ' ', ' ', ' ', 'Q', 'Q', 'Q', ' ', ' ', // 6
                'Q', 'Q', 'Q', 'Q', ' ', 'Q', 'Q', 'Q', // 5
                ' ', ' ', ' ', 'Q', 'Q', 'Q', ' ', ' ', // 4
                ' ', ' ', 'Q', ' ', 'Q', ' ', 'Q', ' ', // 3
                ' ', 'Q', ' ', ' ', 'Q', ' ', ' ', 'Q', // 2
                'Q', ' ', ' ', ' ', 'Q', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Queen_Can_Capture_Enemy_Pieces()
    {
        // Queen should be able to capture enemy pieces in all directions
        // Enemy pieces placed at the edges of the board in all 8 directions
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'Q', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', 'P', ' ', ' ', 'P', ' ', ' ', 'P', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                'P', ' ', ' ', ' ', ' ', ' ', ' ', 'P', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', 'P', ' ', ' ', 'P', ' ', ' ', 'P', // 2
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ') // 1
            .SetQueenAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', 'Q', ' ', ' ', 'Q', ' ', ' ', 'Q', // 8
                ' ', ' ', 'Q', ' ', 'Q', ' ', 'Q', ' ', // 7
                ' ', ' ', ' ', 'Q', 'Q', 'Q', ' ', ' ', // 6
                'Q', 'Q', 'Q', 'Q', ' ', 'Q', 'Q', 'Q', // 5
                ' ', ' ', ' ', 'Q', 'Q', 'Q', ' ', ' ', // 4
                ' ', ' ', 'Q', ' ', 'Q', ' ', 'Q', ' ', // 3
                ' ', 'Q', ' ', ' ', 'Q', ' ', ' ', 'Q', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Queen_Cannot_Capture_Friendly_Pieces()
    {
        // Queen completely surrounded by friendly pieces should have no moves
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'Q', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetQueenAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void Queen_Cannot_Jump_Over_Pieces()
    {
        // Queen blocked by friendly pieces in all 8 directions at distance 1
        // Should have no legal moves
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'Q', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetQueenAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void Queen_Stops_At_First_Capture_In_Each_Direction()
    {
        // Queen should stop at the first enemy piece in each direction
        // Enemy pieces placed at various distances in all 8 directions
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'Q', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'P', ' ', 'P', ' ', 'P', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', 'P', ' ', ' ', ' ', ' ', 'P', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', 'P', ' ', 'P', ' ', 'P', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetQueenAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'Q', ' ', 'Q', ' ', 'Q', ' ', // 7
                ' ', ' ', ' ', 'Q', 'Q', 'Q', ' ', ' ', // 6
                ' ', 'Q', 'Q', 'Q', ' ', 'Q', 'Q', ' ', // 5
                ' ', ' ', ' ', 'Q', 'Q', 'Q', ' ', ' ', // 4
                ' ', ' ', 'Q', ' ', 'Q', ' ', 'Q', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }
}
