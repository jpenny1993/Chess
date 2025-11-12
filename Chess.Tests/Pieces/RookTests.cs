using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests.Pieces;

public class RookTests
{
    [Fact]
    public void Rook_Can_Move_Horizontally_And_Vertically()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetRookAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 6
                'R', 'R', 'R', 'R', ' ', 'R', 'R', 'R', // 5
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Rook_Can_Capture_Enemy_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                'P', ' ', ' ', ' ', ' ', ' ', ' ', 'P', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ') // 1
            .SetRookAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 6
                'R', 'R', 'R', 'R', ' ', 'R', 'R', 'R', // 5
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Rook_Cannot_Capture_Friendly_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'R', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetRookAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void Rook_Cannot_Jump_Over_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', 'P', ' ', 'R', ' ', 'P', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetRookAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'R', ' ', 'R', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Rook_Stops_At_First_Capture()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                'P', ' ', ' ', ' ', ' ', ' ', ' ', 'P', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'P', // 4
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetRookAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 6
                'R', 'R', 'R', 'R', ' ', 'R', 'R', 'R', // 5
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', 'R', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }
}
