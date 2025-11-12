using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests.Pieces;

public class KnightTests
{
    [Fact]
    public void Knight_Can_Move_In_L_Shape()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'N', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 7
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 4
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Knight_Can_Capture_Enemy_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'N', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 7
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 4
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 7
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 4
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Knight_Cannot_Capture_Friendly_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 7
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 6
                ' ', ' ', ' ', ' ', 'N', ' ', ' ', ' ', // 5
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 4
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void Knight_Can_Jump_Over_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'N', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 7
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 4
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Knight_In_Corner_Has_Limited_Moves()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                'N', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("A8", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'N', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', 'N', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Knight_On_Edge_Has_Fewer_Moves()
    {
        // Knight on edge (row 1)
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', 'N', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("D1", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', 'N', ' ', 'N', ' ', ' ', ' ', // 3
                ' ', 'N', ' ', ' ', ' ', 'N', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Knight_On_Side_Edge_Has_Fewer_Moves()
    {
        // Knight on side edge (column H)
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'N', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("H5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', 'N', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', 'N', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', 'N', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', 'N', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Knight_Completely_Surrounded_Can_Still_Jump()
    {
        // Knight surrounded by pieces can still jump over them
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'N', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKnightAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 7
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', 'N', ' ', ' ', ' ', 'N', ' ', // 4
                ' ', ' ', ' ', 'N', ' ', 'N', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        // Knight can jump over all surrounding pieces
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }
}
