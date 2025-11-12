using System.Collections.Generic;
using System.Linq;
using Chess.Tests.Builders;
using Chess.Actions;
using Chess.Pieces;
using FluentAssertions;
using Xunit;

namespace Chess.Tests.Pieces;

public class PawnTests
{
    [Fact]
    public void White_Pawn_Can_Move_Forward_One_Square()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D4", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void White_Pawn_Can_Move_Two_Squares_From_Starting_Position()
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
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D2", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Black_Pawn_Can_Move_Forward_One_Square()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D5", PieceColour.Black)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Black_Pawn_Can_Move_Two_Squares_From_Starting_Position()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D7", PieceColour.Black)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void White_Pawn_Can_Capture_Diagonally()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', 'P', ' ', 'P', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D4", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', 'P', 'P', 'P', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void White_Pawn_Cannot_Capture_Forward()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D4", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void White_Pawn_Cannot_Capture_Friendly_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', 'P', 'P', 'P', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D4", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void White_Pawn_Cannot_Jump_Over_Pieces()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D2", PieceColour.White)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void White_Pawn_On_Edge_Can_Only_Capture_One_Direction()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                'P', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', 'P', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("A4", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                'P', 'P', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        // Pawn on A-file can only capture right (B5), not left (off board)
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Black_Pawn_Can_Capture_Diagonally()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', 'P', ' ', 'P', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D5", PieceColour.Black)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', 'P', 'P', 'P', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void Black_Pawn_Cannot_Capture_Forward()
    {
        var possibleMoves = new ChessBoardBuilder()
            .WithBlackPieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D5", PieceColour.Black)
            .BuildPossibleMoves();

        possibleMoves.Should().BeEmpty();
    }

    [Fact]
    public void Pawn_At_Seventh_Rank_Cannot_Move_Two_Squares()
    {
        // White pawn at 7th rank (near promotion)
        var possibleMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .SetPawnAt("D7", PieceColour.White)
            .BuildPossibleMoves();

        var expectedMoves = new ChessBoardBuilder()
            .WithWhitePieces(
            //   A    B    C    D    E    F    G    H
                ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', // 8
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 7
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 6
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 5
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 4
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 3
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', // 2
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ') // 1
            .BuildCoordinates();

        // Can only move one square forward, not two
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void White_Pawn_Can_Capture_En_Passant_Left()
    {
        // Setup: White pawn at E5, black pawn just moved from E7 to E5 (moved 2 squares)
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.White, 'E', 5),
            new Pawn(PieceColour.Black, 'D', 5)
        };
        var board = new Board(pieces);

        // Simulate black pawn's last move (D7 -> D5)
        var blackPawn = board.FindPiece(new Position('D', 5))!;
        board.LastMove = new Movement(blackPawn, new Position('D', 7), new Position('D', 5));

        var whitePawn = board.FindPiece(new Position('E', 5));
        var possibleMoves = whitePawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // White pawn should be able to move to E6 and capture en passant at D6
        var expectedMoves = new[] { "D6", "E6" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);

        // Verify en passant move has the EnPassant action
        var enPassantMove = whitePawn.PossibleMoves(board)
            .First(m => m.Destination.ToString() == "D6");
        enPassantMove.Actions.Should().ContainSingle(a => a is EnPassant);
    }

    [Fact]
    public void White_Pawn_Can_Capture_En_Passant_Right()
    {
        // Setup: White pawn at E5, black pawn just moved from F7 to F5 (moved 2 squares)
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.White, 'E', 5),
            new Pawn(PieceColour.Black, 'F', 5)
        };
        var board = new Board(pieces);

        // Simulate black pawn's last move (F7 -> F5)
        var blackPawn = board.FindPiece(new Position('F', 5))!;
        board.LastMove = new Movement(blackPawn, new Position('F', 7), new Position('F', 5));

        var whitePawn = board.FindPiece(new Position('E', 5));
        var possibleMoves = whitePawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // White pawn should be able to move to E6 and capture en passant at F6
        var expectedMoves = new[] { "E6", "F6" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);

        // Verify en passant move has the EnPassant action
        var enPassantMove = whitePawn.PossibleMoves(board)
            .First(m => m.Destination.ToString() == "F6");
        enPassantMove.Actions.Should().ContainSingle(a => a is EnPassant);
    }

    [Fact]
    public void Black_Pawn_Can_Capture_En_Passant_Left()
    {
        // Setup: Black pawn at D4, white pawn just moved from C2 to C4 (moved 2 squares)
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.Black, 'D', 4),
            new Pawn(PieceColour.White, 'C', 4)
        };
        var board = new Board(pieces);

        // Simulate white pawn's last move (C2 -> C4)
        var whitePawn = board.FindPiece(new Position('C', 4))!;
        board.LastMove = new Movement(whitePawn, new Position('C', 2), new Position('C', 4));

        var blackPawn = board.FindPiece(new Position('D', 4));
        var possibleMoves = blackPawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // Black pawn should be able to move to D3 and capture en passant at C3
        var expectedMoves = new[] { "C3", "D3" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);

        // Verify en passant move has the EnPassant action
        var enPassantMove = blackPawn.PossibleMoves(board)
            .First(m => m.Destination.ToString() == "C3");
        enPassantMove.Actions.Should().ContainSingle(a => a is EnPassant);
    }

    [Fact]
    public void Black_Pawn_Can_Capture_En_Passant_Right()
    {
        // Setup: Black pawn at D4, white pawn just moved from E2 to E4 (moved 2 squares)
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.Black, 'D', 4),
            new Pawn(PieceColour.White, 'E', 4)
        };
        var board = new Board(pieces);

        // Simulate white pawn's last move (E2 -> E4)
        var whitePawn = board.FindPiece(new Position('E', 4))!;
        board.LastMove = new Movement(whitePawn, new Position('E', 2), new Position('E', 4));

        var blackPawn = board.FindPiece(new Position('D', 4));
        var possibleMoves = blackPawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // Black pawn should be able to move to D3 and capture en passant at E3
        var expectedMoves = new[] { "D3", "E3" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);

        // Verify en passant move has the EnPassant action
        var enPassantMove = blackPawn.PossibleMoves(board)
            .First(m => m.Destination.ToString() == "E3");
        enPassantMove.Actions.Should().ContainSingle(a => a is EnPassant);
    }

    [Fact]
    public void En_Passant_Not_Available_If_No_Last_Move()
    {
        // Setup: White pawn at E5, black pawn at D5, but no last move recorded
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.White, 'E', 5),
            new Pawn(PieceColour.Black, 'D', 5)
        };
        var board = new Board(pieces);
        // No LastMove set

        var whitePawn = board.FindPiece(new Position('E', 5));
        var possibleMoves = whitePawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // White pawn should only be able to move forward, not capture en passant
        var expectedMoves = new[] { "E6" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void En_Passant_Not_Available_If_Enemy_Pawn_Moved_One_Square()
    {
        // Setup: White pawn at E5, black pawn moved from D6 to D5 (only 1 square)
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.White, 'E', 5),
            new Pawn(PieceColour.Black, 'D', 5)
        };
        var board = new Board(pieces);

        // Black pawn moved only 1 square
        var blackPawn = board.FindPiece(new Position('D', 5))!;
        board.LastMove = new Movement(blackPawn, new Position('D', 6), new Position('D', 5));

        var whitePawn = board.FindPiece(new Position('E', 5));
        var possibleMoves = whitePawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // White pawn should only be able to move forward, not capture en passant
        var expectedMoves = new[] { "E6" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void En_Passant_Not_Available_If_Last_Move_Was_Not_By_Pawn()
    {
        // Setup: White pawn at E5, black knight at D5, knight just moved
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.White, 'E', 5),
            new Knight(PieceColour.Black, 'D', 5)
        };
        var board = new Board(pieces);

        // Last move was by a knight, not a pawn
        var knight = board.FindPiece(new Position('D', 5))!;
        board.LastMove = new Movement(knight, new Position('C', 3), new Position('D', 5));

        var whitePawn = board.FindPiece(new Position('E', 5));
        var possibleMoves = whitePawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // White pawn should only be able to move forward, not capture en passant
        var expectedMoves = new[] { "E6" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Fact]
    public void En_Passant_Not_Available_If_Pawn_Not_On_Correct_Rank()
    {
        // Setup: White pawn at E4 (should be at E5), black pawn just moved from D7 to D5
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.White, 'E', 4),
            new Pawn(PieceColour.Black, 'D', 5)
        };
        var board = new Board(pieces);

        // Black pawn moved 2 squares
        var blackPawn = board.FindPiece(new Position('D', 5))!;
        board.LastMove = new Movement(blackPawn, new Position('D', 7), new Position('D', 5));

        var whitePawn = board.FindPiece(new Position('E', 4));
        var possibleMoves = whitePawn!.PossibleMoves(board)
            .Select(m => m.Destination.ToString())
            .OrderBy(m => m)
            .ToList();

        // White pawn at E4 can move forward to E5 and capture normally at D5
        // But NOT via en passant since it's not on rank 5
        var expectedMoves = new[] { "D5", "E5" };
        possibleMoves.Should().BeEquivalentTo(expectedMoves);

        // Verify the D5 capture is NOT en passant (should be regular Capture)
        var d5Move = whitePawn.PossibleMoves(board).First(m => m.Destination.ToString() == "D5");
        d5Move.Actions.Should().NotContain(a => a is EnPassant);
    }

    [Fact]
    public void En_Passant_Captures_The_Correct_Pawn_Position()
    {
        // Verify that the EnPassant action contains the correct captured pawn position
        var pieces = new List<Piece>
        {
            new Pawn(PieceColour.White, 'E', 5),
            new Pawn(PieceColour.Black, 'D', 5)
        };
        var board = new Board(pieces);

        // Simulate black pawn's last move (D7 -> D5)
        var blackPawn = board.FindPiece(new Position('D', 5))!;
        board.LastMove = new Movement(blackPawn, new Position('D', 7), new Position('D', 5));

        var whitePawn = board.FindPiece(new Position('E', 5));
        var enPassantMove = whitePawn!.PossibleMoves(board)
            .First(m => m.Destination.ToString() == "D6");

        var enPassantAction = enPassantMove.Actions.OfType<EnPassant>().Single();
        enPassantAction.CapturedPawnPosition.Should().Be(new Position('D', 5));
    }
}
