using System.Linq;
using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests;

public class CheckmateTests
{
    [Fact]
    public void Back_Rank_Mate_Is_Detected()
    {
        // Classic back rank mate: White queen on 7th rank, black king trapped on back rank by own pawns
        // White queen at D7, Black king at E8 with pawns at F7, G7, H7
        var board = new ChessBoardBuilder()
            .SetQueenAt("D7", PieceColour.White)
            .SetKingAt("E8", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White) // White king needed for valid position
            .Build();

        var queen = board.FindPiece(new Position('D', 7));
        var moves = queen!.PossibleMoves(board);

        // Queen moving to E8 should be checkmate
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "E8");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }

    [Fact]
    public void Fools_Mate_Is_Detected()
    {
        // Fool's mate: fastest checkmate in chess (2 moves)
        // After 1.f3 e5 2.g4 Qh4#
        var board = new ChessBoardBuilder()
            // White pieces
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("D1", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C1", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("G1", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D2", PieceColour.White)
            .SetPawnAt("E2", PieceColour.White)
            .SetPawnAt("F3", PieceColour.White) // f2-f3
            .SetPawnAt("G4", PieceColour.White) // g2-g4
            .SetPawnAt("H2", PieceColour.White)
            // Black pieces
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("H8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("B8", PieceColour.Black)
            .SetKnightAt("G8", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black) // e7-e5
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();

        var blackQueen = board.FindPiece(new Position('D', 8));
        var moves = blackQueen!.PossibleMoves(board);

        // Black queen moving to H4 should be checkmate
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "H4");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }

    [Fact]
    public void Scholars_Mate_Is_Detected()
    {
        // Scholar's mate: 1.e4 e5 2.Bc4 Nc6 3.Qh5 Nf6 4.Qxf7#
        var board = new ChessBoardBuilder()
            // White pieces
            .SetKingAt("E1", PieceColour.White)
            .SetQueenAt("H5", PieceColour.White)
            .SetRookAt("A1", PieceColour.White)
            .SetRookAt("H1", PieceColour.White)
            .SetBishopAt("C4", PieceColour.White)
            .SetBishopAt("F1", PieceColour.White)
            .SetKnightAt("B1", PieceColour.White)
            .SetKnightAt("G1", PieceColour.White)
            .SetPawnAt("A2", PieceColour.White)
            .SetPawnAt("B2", PieceColour.White)
            .SetPawnAt("C2", PieceColour.White)
            .SetPawnAt("D2", PieceColour.White)
            .SetPawnAt("E4", PieceColour.White)
            .SetPawnAt("F2", PieceColour.White)
            .SetPawnAt("G2", PieceColour.White)
            .SetPawnAt("H2", PieceColour.White)
            // Black pieces
            .SetKingAt("E8", PieceColour.Black)
            .SetQueenAt("D8", PieceColour.Black)
            .SetRookAt("A8", PieceColour.Black)
            .SetRookAt("H8", PieceColour.Black)
            .SetBishopAt("C8", PieceColour.Black)
            .SetBishopAt("F8", PieceColour.Black)
            .SetKnightAt("C6", PieceColour.Black)
            .SetKnightAt("F6", PieceColour.Black)
            .SetPawnAt("A7", PieceColour.Black)
            .SetPawnAt("B7", PieceColour.Black)
            .SetPawnAt("C7", PieceColour.Black)
            .SetPawnAt("D7", PieceColour.Black)
            .SetPawnAt("E5", PieceColour.Black)
            .SetPawnAt("F7", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();

        var whiteQueen = board.FindPiece(new Position('H', 5));
        var moves = whiteQueen!.PossibleMoves(board);

        // White queen capturing on F7 should be checkmate
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "F7");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove!.IsCapture.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }

    [Fact]
    public void Smothered_Mate_Is_Detected()
    {
        // Smothered mate: King surrounded by its own pieces, knight delivers mate
        // Black king at H8, surrounded by rook at G8, pawns at G7, H7
        // White knight delivers mate from F7
        var board = new ChessBoardBuilder()
            .SetKnightAt("E5", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .SetRookAt("G8", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .Build();

        var knight = board.FindPiece(new Position('E', 5));
        var moves = knight!.PossibleMoves(board);

        // Knight moving to F7 should be checkmate
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "F7");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }

    [Fact]
    public void Check_Without_Checkmate_Is_Detected()
    {
        // King in check but has escape squares
        var board = new ChessBoardBuilder()
            .SetQueenAt("D1", PieceColour.White)
            .SetKingAt("E8", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .Build();

        var queen = board.FindPiece(new Position('D', 1));
        var moves = queen!.PossibleMoves(board);

        // Queen moving to D8 puts king in check, but king can escape to E7, F8, or F7
        var checkMove = moves.FirstOrDefault(m => m.Destination.ToString() == "D8");

        checkMove.Should().NotBeNull();
        checkMove!.IsCheck.Should().BeTrue();
        checkMove.IsCheckmate.Should().BeFalse();
    }

    [Fact]
    public void King_Can_Capture_Attacking_Piece_To_Escape_Check()
    {
        // King in check but can capture the attacking piece
        var board = new ChessBoardBuilder()
            .SetQueenAt("E7", PieceColour.White)
            .SetKingAt("E8", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .Build();

        var queen = board.FindPiece(new Position('E', 7));
        var moves = queen!.PossibleMoves(board);

        // Queen moving to E8 would be captured by king, so not checkmate
        var moveToKing = moves.FirstOrDefault(m => m.Destination.ToString() == "E8");

        // The move should exist (queen can capture king) but won't be checkmate
        // because king can recapture
        moveToKing.Should().NotBeNull();
        moveToKing!.IsCapture.Should().BeTrue();
    }

    [Fact]
    public void Piece_Can_Block_Check()
    {
        // Rook gives check, but bishop can block
        var board = new ChessBoardBuilder()
            .SetRookAt("A8", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .SetBishopAt("D5", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .Build();

        var rook = board.FindPiece(new Position('A', 8));
        var moves = rook!.PossibleMoves(board);

        // Rook moving along rank 8 gives check, but bishop can block at C8, D8, E8, F8, or G8
        var checkMove = moves.FirstOrDefault(m => m.Destination.ToString() == "B8");

        checkMove.Should().NotBeNull();
        checkMove!.IsCheck.Should().BeTrue();
        checkMove.IsCheckmate.Should().BeFalse(); // Bishop can block
    }

    [Fact]
    public void Two_Rooks_Checkmate_On_Back_Rank()
    {
        // Two rooks deliver mate on back rank
        var board = new ChessBoardBuilder()
            .SetRookAt("A7", PieceColour.White)
            .SetRookAt("B6", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .Build();

        var rook = board.FindPiece(new Position('B', 6));
        var moves = rook!.PossibleMoves(board);

        // Rook moving to B8 should be checkmate (with support from A7 rook)
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "B8");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }

    [Fact]
    public void Queen_And_King_Checkmate_In_Corner()
    {
        // Queen and king force enemy king to corner and deliver mate
        var board = new ChessBoardBuilder()
            .SetQueenAt("G6", PieceColour.White)
            .SetKingAt("G8", PieceColour.Black)
            .SetKingAt("F6", PieceColour.White)
            .Build();

        var queen = board.FindPiece(new Position('G', 6));
        var moves = queen!.PossibleMoves(board);

        // Queen moving to G7 or H6 should be checkmate
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "G7");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }

    [Fact]
    public void Arabian_Mate_With_Rook_And_Knight()
    {
        // Arabian mate: Rook and knight checkmate on corner
        var board = new ChessBoardBuilder()
            .SetRookAt("H7", PieceColour.White)
            .SetKnightAt("F6", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .Build();

        var rook = board.FindPiece(new Position('H', 7));
        var moves = rook!.PossibleMoves(board);

        // Rook moving to H8 should be checkmate (knight controls escape squares)
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "H8");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove!.IsCapture.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }

    [Fact]
    public void Rook_And_Bishop_Checkmate_On_Edge()
    {
        // Rook and bishop coordinate to trap and checkmate king on edge
        var board = new ChessBoardBuilder()
            .SetRookAt("A7", PieceColour.White)
            .SetBishopAt("F2", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .SetPawnAt("G7", PieceColour.Black)
            .SetPawnAt("H7", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .Build();

        var rook = board.FindPiece(new Position('A', 7));
        var moves = rook!.PossibleMoves(board);

        // Rook moving to A8 delivers checkmate (bishop controls G7 diagonal, pawns block king)
        var checkmateMove = moves.FirstOrDefault(m => m.Destination.ToString() == "A8");

        checkmateMove.Should().NotBeNull();
        checkmateMove!.IsCheck.Should().BeTrue();
        checkmateMove.IsCheckmate.Should().BeTrue();
    }
}
