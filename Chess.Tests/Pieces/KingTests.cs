using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests.Pieces;

public class KingTests
{
    [Fact]
    public void Move_Left_Or_Down()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'K', // 8
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
            ' ', ' ', ' ', ' ', ' ', ' ', 'K', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', 'K', 'K', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }
    
    [Fact]
    public void Move_Left_Or_Up()
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
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'K') // 1
            .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', 'K', 'K', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', 'K', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Move_Right_Or_Down()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            'K', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
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
            ' ', 'K', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            'K', 'K', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Move_Right_Or_Up()
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
            'K', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
        .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            'K', 'K', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', 'K', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Move_In_Any_Direction()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
            ' ', ' ', ' ', ' ', 'K', ' ', ' ', ' ', // 5
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
        .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
        //   A    B    C    D    E    F    G    H
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
            ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 6
            ' ', ' ', ' ', 'K', ' ', 'K', ' ', ' ', // 5
            ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 4
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Take_Left_Or_Right_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'K', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 6
                ' ', ' ', ' ', 'K', ' ', 'K', ' ', ' ', // 5
                ' ', ' ', ' ', 'K', ' ', 'K', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        // King can capture most surrounding enemy pieces but not E4
        // E4 is attacked by both D5 and F5 pawns, so moving there would be check
        // Actual legal moves: D4, D5, D6, E6, F4, F5, F6 (7 moves)
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Take_Top_Or_Bottom_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'K', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        // King can capture all surrounding enemy pawns, but cannot move to D5 or F5
        // D5 would be attacked by D6 pawn, F5 would be attacked by F6 pawn
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Cannot_Take_Friendly_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'K', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("E5", PieceColour.White)
            .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'K', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'K', ' ', 'K', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', 'K', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Cannot_Take_Protected_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
                //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'K', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', 'P', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
                //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'K', ' ', 'K', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        // King cannot capture E4 (attacked by D5 and F5), D5 (attacked by E6), or F5 (attacked by E6)
        // Legal moves: D4, D6, E6, F4, F6 (5 moves)
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Can_Perform_Left_Castle_Manoeuvre()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
                //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                'N', ' ', 'Q', 'P', 'B', ' ', ' ', ' ', // 3
                'P', 'P', 'P', ' ', 'P', 'P', 'P', 'P', // 2
                'R', ' ', ' ', ' ', 'K', 'B', 'N', 'R') // 1
            .SetKingAt("E1", PieceColour.White)
            .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
                //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', 'K', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', 'K', 'K', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Can_Perform_Right_Castle_Manoeuvre()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', 'N', 'P', ' ', // 3
                'P', 'P', 'P', 'P', 'P', 'P', 'B', 'P', // 2
                'R', 'N', 'B', 'Q', 'K', ' ', ' ', 'R') // 1
            .SetKingAt("E1", PieceColour.White)
            .BuildPossibleMoves();
        
        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', 'K', 'K', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void King_Completely_Surrounded_By_Friendly_Pieces_Cannot_Move()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', 'K', 'P', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', 'P', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void King_In_Bottom_Corner_Has_Three_Moves()
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
                'K', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("A1", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                'K', 'K', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', 'K', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void King_On_Edge_Has_Five_Moves()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                'K', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("A4", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                'K', 'K', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', 'K', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                'K', 'K', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void King_Can_Move_To_Empty_Squares_And_Capture()
    {
        // This tests that King can both move to empty squares AND capture
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', 'K', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', 'P', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetKingAt("E5", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 6
                ' ', ' ', ' ', 'K', ' ', 'K', ' ', ' ', // 5
                ' ', ' ', ' ', 'K', 'K', 'K', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }
}