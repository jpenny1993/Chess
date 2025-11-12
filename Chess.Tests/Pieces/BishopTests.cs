using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests.Pieces;

public class BishopTests
{
    [Fact]
    public void Move_Diagonally_Left_Up()
    {
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
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B') // 1
            .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            'B', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', 'B', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', 'B', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', 'B', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', 'B', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Move_Diagonally_Left_Down()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
        .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', 'B', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', 'B', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', 'B', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', 'B', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            'B', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }
    
    [Fact]
    public void Move_Diagonally_Right_Down()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            'B', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
        .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', 'B', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', 'B', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', 'B', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', 'B', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Move_Diagonally_Right_Up()
    {
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
            'B', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
        .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', 'B', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', 'B', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', 'B', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', 'B', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Move_Diagonally_Both_Ways()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
        .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', 'B', ' ', ' ', ' ', ' ', ' ', 'B', // 8
            ' ', ' ', 'B', ' ', ' ', ' ', 'B', ' ', // 7
            ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 4
            ' ', ' ', 'B', ' ', ' ', ' ', 'B', ' ', // 3
            ' ', 'B', ' ', ' ', ' ', ' ', ' ', 'B', // 2
            'B', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Take_Diagonally_Both_Ways()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetBishopAt("E5", PieceColour.White)
            .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'B', ' ', ' ', ' ', 'B', ' ', // 7
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 4
                ' ', ' ', 'B', ' ', ' ', ' ', 'B', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Cannot_Take_Friendly_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetBishopAt("E5", PieceColour.White)
            .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Bishop_Cannot_Jump_Over_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetBishopAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Bishop_Stops_At_First_Capture_In_Each_Direction()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', 'P', ' ', ' ', ' ', ' ', ' ', 'P', // 8
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', 'P', ' ', ' ', ' ', 'P', ' ', // 3
                ' ', 'P', ' ', ' ', ' ', ' ', ' ', 'P', // 2
                'P', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetBishopAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', 'B', ' ', ' ', ' ', 'B', ' ', // 7
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'B', ' ', 'B', ' ', ' ', // 4
                ' ', ' ', 'B', ' ', ' ', ' ', 'B', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Bishop_In_Corner_Has_Limited_Moves()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                'B', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetBishopAt("A8", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', 'B', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'B', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', 'B', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', 'B', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Bishop_On_Same_Color_Square_As_Starting_Position()
    {
        // This test verifies that a bishop on C1 (light square) can reach all light squares
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
                ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ') // 1
            .SetBishopAt("C1", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', 'B', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', 'B', ' ', ' ', // 4
                'B', ' ', ' ', ' ', 'B', ' ', ' ', ' ', // 3
                ' ', 'B', ' ', 'B', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }
}