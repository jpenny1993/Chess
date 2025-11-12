using Chess.Exceptions;
using Chess.Notation;
using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests;

public class NotationTests
{
    [Fact]
    public void Should_Parse_Match_Notes()
    {
        // ref: https://www.ichess.net/blog/chess-notation/
        // Steinitz, William - Hirschfeld, Philipp
        var matchNotes = new[]
        {
            "1.e4e5",      // [W] Pawn   (E2) -> E4, [B] Pawn   (E7) -> E5
            "2.Nc3Nf6",    // [W] Knight (D1) -> C3, [B] Knight (G8) -> F6
            "3.f4d5",      // [W] Pawn   (F2) -> F4, [B] Pawn   (D7) -> D5
            "4.d3dxe4",    // [W] Pawn   (D2) -> D3, [B] Pawn   (D5) -> E4 (B captured W)
            "5.fxe5Ng4",   // [W] Pawn   (F4) -> E5, [B] Knight (F6) -> G4 (W captured B, en passant)
            "6.Nxe4Nxe5",  // [W] Knight (C3) -> E4, [B] Knight (G4) -> E5 (Both captured)
            "7.d4Nec6",    // [W] Pawn   (D3) -> D4, [B] Knight (E5) -> C6
            "8.Nf3Bg4",    // [W] Knight (G1) -> F3, [B] Bishop (C8) -> G4
            "9.c3Qe7",     // [W] Pawn   (C2) -> C3, [B] Queen  (E8) -> E7
            "10.Bd3f5",    // [W] Bishop (F1) -> D3, [B] Pawn   (F7) -> F5
            "11.O-Ofxe4",  // [W] K E1->G1 R H1->F1, [B] Pawn   (F5) -> E4 (W castled kings-side, B captured W)
            "12.Bxe4Qd7",  // [W] Bishop (D3) -> E4, [B] Queen  (D8) -> D7 (W captured B)
            "13.Qb3Na5",   // [W] Queen  (D1) -> B3, [B] Knight (C6) -> A5 
            "14.Qc2Bxf3",  // [W] Queen  (B3) -> C2, [B] Bishop (G4) -> F3 (B captured W)
            "15.Rxf3Be7",  // [W] Rook   (F1) -> F3, [B] Bishop (F8) -> E7 (W captured B)
            "16.Bxh7Bf6",  // [W] Bishop (E4) -> H7, [B] Bishop (E7) -> F6 (W captured B)
            "17.Qg6+Qf7",  // [W] Queen  (C2) -> G6, [B] Queen  (D7) -> F7 (W put B in check)
            "18.Qf5Bxd4+", // [W] Queen  (G6) -> F5, [B] Bishop (F6) -> D4 (B captured W, & put W in check)
            "19.cxd4Qxf5", // [W] Pawn   (C3) -> D4, [B] Queen  (F7) -> F5 (Both captured)
            "20.Bxf5Nbc6", // [W] Bishop (H7) -> F5, [B] Knight (B8) -> C6 (W captured B)
            "21.Bg6+Ke7",  // [W] Bishop (F5) -> G6, [B] King   (E8) -> E7 (W put B in check)
            "22.Rf7+Ke6",  // [W] Rook   (F3) -> F7, [B] King   (E7) -> E6 (W put B in check)
            "23.Bd2Nc4",   // [W] Bishop (C1) -> D2, [B] Knight (A5) -> C4
            "24.Re1+Kd5",  // [W] Rook   (A1) -> E1, [B] King   (E6) -> D5 (W put B in check)
            "25.Rf5+Kd6",  // [W] Rook   (F3) -> F5, [B] King   (D5) -> D6 (W put B in check)
            "26.Bf4+Kd7",  // [W] Bishop (D2) -> F4, [B] King   (D6) -> D7 (W put B in check)
            "27.Rf7+Kc8",  // [W] Rook   (F5) -> F7, [B] King   (D7) -> C8 (W put B in check)
            "28.Bf5+Kb8",  // [W] Bishop (G6) -> F5, [B] King   (C8) -> B8 (W put B in check)
            "29.Bxc7#"     // [W] Bishop (F4) -> C7, Checkmate             (W captured B, White wins)
        };

        var expectedTurns = new ChessNotationBuilder()
            .Turn(w => w.Move("E4", PieceType.Pawn),                     black => black.Move("E5", PieceType.Pawn))
            .Turn(w => w.Move("C3", PieceType.Knight),                   black => black.Move("F6", PieceType.Knight))
            .Turn(w => w.Move("F4", PieceType.Pawn),                     black => black.Move("D5", PieceType.Pawn))
            .Turn(w => w.Move("D3", PieceType.Pawn),                     black => black.Move("E4", PieceType.Pawn).Hint("D").Capture())
            .Turn(w => w.Move("E5", PieceType.Pawn).Hint("F").Capture(), black => black.Move("G4", PieceType.Knight))
            .Turn(w => w.Move("E4", PieceType.Knight).Capture(),         black => black.Move("E5", PieceType.Knight).Capture())
            .Turn(w => w.Move("D4", PieceType.Pawn),                     black => black.Move("C6", PieceType.Knight).Hint("E"))
            .Turn(w => w.Move("F3", PieceType.Knight),                   black => black.Move("G4", PieceType.Bishop))
            .Turn(w => w.Move("C3", PieceType.Pawn),                     black => black.Move("E7", PieceType.Queen))
            .Turn(w => w.Move("D3", PieceType.Bishop),                   black => black.Move("F5", PieceType.Pawn))
            .Turn(w => w.Castle(isKingSide: true),                       black => black.Move("E4", PieceType.Pawn).Hint("F").Capture())
            .Turn(w => w.Move("E4", PieceType.Bishop).Capture(),         black => black.Move("D7", PieceType.Queen))
            .Turn(w => w.Move("B3", PieceType.Queen),                    black => black.Move("A5", PieceType.Knight))
            .Turn(w => w.Move("C2", PieceType.Queen),                    black => black.Move("F3", PieceType.Bishop).Capture())
            .Turn(w => w.Move("F3", PieceType.Rook).Capture(),           black => black.Move("E7", PieceType.Bishop))
            .Turn(w => w.Move("H7", PieceType.Bishop).Capture(),         black => black.Move("F6", PieceType.Bishop))
            .Turn(w => w.Move("G6", PieceType.Queen).Check(),            black => black.Move("F7", PieceType.Queen))
            .Turn(w => w.Move("F5", PieceType.Queen),                    black => black.Move("D4", PieceType.Bishop).Capture().Check())
            .Turn(w => w.Move("D4", PieceType.Pawn).Hint("C").Capture(), black => black.Move("F5", PieceType.Queen).Capture())
            .Turn(w => w.Move("F5", PieceType.Bishop).Capture(),         black => black.Move("C6", PieceType.Knight).Hint("B"))
            .Turn(w => w.Move("G6", PieceType.Bishop).Check(),           black => black.Move("E7", PieceType.King))
            .Turn(w => w.Move("F7", PieceType.Rook).Check(),             black => black.Move("E6", PieceType.King))
            .Turn(w => w.Move("D2", PieceType.Bishop),                   black => black.Move("C4", PieceType.Knight))
            .Turn(w => w.Move("E1", PieceType.Rook).Check(),             black => black.Move("D5", PieceType.King))
            .Turn(w => w.Move("F5", PieceType.Rook).Check(),             black => black.Move("D6", PieceType.King))
            .Turn(w => w.Move("F4", PieceType.Bishop).Check(),           black => black.Move("D7", PieceType.King))
            .Turn(w => w.Move("F7", PieceType.Rook).Check(),             black => black.Move("C8", PieceType.King))
            .Turn(w => w.Move("F5", PieceType.Bishop).Check(),           black => black.Move("B8", PieceType.King))
            .Turn(w => w.Move("C7", PieceType.Bishop).Capture().Checkmate(), _ => { })
            .Build();

        AlgebraicNotationReader notationReader = new();

        for (var index = 0; index < matchNotes.Length; index++)
        {
            var expectedTurn = expectedTurns[index];
            var turnNote = matchNotes[index];

            var actualTurn = notationReader.ReadTurn(turnNote);

            actualTurn.Should().BeEquivalentTo(expectedTurn);
        }
    }

    [Fact]
    public void Should_Parse_Simple_Pawn_Move()
    {
        var notation = "1.e4e5";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.TurnNumber.Should().Be(1);
        turn.WhitePlayerTurn.Moves.Should().HaveCount(1);
        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Pawn);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("E4");

        turn.BlackPlayerTurn.Moves.Should().HaveCount(1);
        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Pawn);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("E5");
    }

    [Fact]
    public void Should_Parse_Knight_Move()
    {
        var notation = "1.Nf3Nc6";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Knight);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("F3");

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Knight);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("C6");
    }

    [Fact]
    public void Should_Parse_Capture()
    {
        var notation = "1.exd5Nxd5";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.IsCapture.Should().BeTrue();
        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Pawn);
        turn.WhitePlayerTurn.Moves[0].Hint.Should().Be("E");
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("D5");

        turn.BlackPlayerTurn.IsCapture.Should().BeTrue();
        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Knight);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("D5");
    }

    [Fact]
    public void Should_Parse_Check()
    {
        var notation = "1.Qh5+Nf6";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.IsCheck.Should().BeTrue();
        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Queen);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("H5");
    }

    [Fact]
    public void Should_Parse_Checkmate()
    {
        var reader = new AlgebraicNotationReader();

        var turn1 = reader.ReadTurn("1.Qh5+g6");
        turn1.WhitePlayerTurn.IsCheck.Should().BeTrue();

        var turn2 = reader.ReadTurn("2.Qxf7#");
        turn2.WhitePlayerTurn.IsCheckmate.Should().BeTrue();
        turn2.WhitePlayerTurn.IsCapture.Should().BeTrue();
        turn2.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("F7");
    }

    [Fact]
    public void Should_Parse_Kingside_Castling()
    {
        var notation = "10.O-OO-O";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.IsCastling.Should().BeTrue();
        turn.WhitePlayerTurn.IsKingSide.Should().BeTrue();
        turn.WhitePlayerTurn.Moves.Should().HaveCount(2);
        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.King);
        turn.WhitePlayerTurn.Moves[1].Piece.Should().Be(PieceType.Rook);

        turn.BlackPlayerTurn.IsCastling.Should().BeTrue();
        turn.BlackPlayerTurn.IsKingSide.Should().BeTrue();
    }

    [Fact]
    public void Should_Parse_Queenside_Castling()
    {
        var notation = "10.O-O-OO-O-O";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.IsCastling.Should().BeTrue();
        turn.WhitePlayerTurn.IsKingSide.Should().BeFalse();
        turn.WhitePlayerTurn.Moves.Should().HaveCount(2);
        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.King);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("C1");
        turn.WhitePlayerTurn.Moves[1].Piece.Should().Be(PieceType.Rook);
        turn.WhitePlayerTurn.Moves[1].Destination.ToString().Should().Be("D1");

        turn.BlackPlayerTurn.IsCastling.Should().BeTrue();
        turn.BlackPlayerTurn.IsKingSide.Should().BeFalse();
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("C8");
    }

    [Fact]
    public void Should_Parse_Pawn_Promotion()
    {
        var notation = "1.e8=Qd1=N";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Pawn);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("E8");
        turn.WhitePlayerTurn.Promotion.Should().Be(PieceType.Queen);

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Pawn);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("D1");
        turn.BlackPlayerTurn.Promotion.Should().Be(PieceType.Knight);
    }

    [Fact]
    public void Should_Parse_Ambiguous_Knight_Move_With_File_Hint()
    {
        var notation = "1.Nbd2Nbd7";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Knight);
        turn.WhitePlayerTurn.Moves[0].Hint.Should().Be("B");
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("D2");

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Knight);
        turn.BlackPlayerTurn.Moves[0].Hint.Should().Be("B");
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("D7");
    }

    [Fact]
    public void Should_Parse_Ambiguous_Rook_Move_With_Rank_Hint()
    {
        var notation = "1.R1a3R8d7";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Rook);
        turn.WhitePlayerTurn.Moves[0].Hint.Should().Be("1");
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("A3");

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Rook);
        turn.BlackPlayerTurn.Moves[0].Hint.Should().Be("8");
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("D7");
    }

    [Fact]
    public void Should_Parse_Bishop_Move()
    {
        var notation = "1.Bc4Bb4";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Bishop);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("C4");

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Bishop);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("B4");
    }

    [Fact]
    public void Should_Parse_Queen_Move()
    {
        var notation = "1.Qd4Qe7";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Queen);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("D4");

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Queen);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("E7");
    }

    [Fact]
    public void Should_Parse_King_Move()
    {
        var notation = "1.Ke2Ke7";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.King);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("E2");

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.King);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("E7");
    }

    [Fact]
    public void Should_Parse_Rook_Move()
    {
        var notation = "1.Ra1Rh8";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Rook);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("A1");

        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Rook);
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("H8");
    }

    [Fact]
    public void Should_Parse_Capture_With_Check()
    {
        var notation = "1.Qxf7+Kd8";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.IsCapture.Should().BeTrue();
        turn.WhitePlayerTurn.IsCheck.Should().BeTrue();
        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Queen);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("F7");
    }

    [Fact]
    public void Should_Parse_Sequential_Turns()
    {
        var reader = new AlgebraicNotationReader();

        var turn1 = reader.ReadTurn("1.e4e5");
        var turn2 = reader.ReadTurn("2.Nf3Nc6");
        var turn3 = reader.ReadTurn("3.Bc4Bc5");

        turn1.TurnNumber.Should().Be(1);
        turn2.TurnNumber.Should().Be(2);
        turn3.TurnNumber.Should().Be(3);

        turn1.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("E4");
        turn2.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("F3");
        turn3.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("C4");
    }

    [Fact]
    public void Should_Parse_Pawn_Promotion_With_Capture()
    {
        var notation = "1.exd8=Qaxb1=R";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.IsCapture.Should().BeTrue();
        turn.WhitePlayerTurn.Moves[0].Piece.Should().Be(PieceType.Pawn);
        turn.WhitePlayerTurn.Moves[0].Hint.Should().Be("E");
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("D8");
        turn.WhitePlayerTurn.Promotion.Should().Be(PieceType.Queen);

        turn.BlackPlayerTurn.IsCapture.Should().BeTrue();
        turn.BlackPlayerTurn.Moves[0].Piece.Should().Be(PieceType.Pawn);
        turn.BlackPlayerTurn.Moves[0].Hint.Should().Be("A");
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("B1");
        turn.BlackPlayerTurn.Promotion.Should().Be(PieceType.Rook);
    }

    [Fact]
    public void Should_Handle_Notation_Without_Move_Numbers()
    {
        var notation = "e4e5";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.TurnNumber.Should().Be(1);
        turn.WhitePlayerTurn.Moves[0].Destination.ToString().Should().Be("E4");
        turn.BlackPlayerTurn.Moves[0].Destination.ToString().Should().Be("E5");
    }

    [Fact]
    public void Should_Parse_Alternative_Castling_Notation_With_Zeros()
    {
        var notation = "10.0-00-0-0";
        var reader = new AlgebraicNotationReader();
        var turn = reader.ReadTurn(notation);

        turn.WhitePlayerTurn.IsCastling.Should().BeTrue();
        turn.WhitePlayerTurn.IsKingSide.Should().BeTrue();

        turn.BlackPlayerTurn.IsCastling.Should().BeTrue();
        turn.BlackPlayerTurn.IsKingSide.Should().BeFalse();
    }

    [Fact]
    public void Should_Read_White_Move_Separately()
    {
        var reader = new AlgebraicNotationReader();
        var whiteMove = reader.WhiteTurn("e4");

        whiteMove.Colour.Should().Be(PieceColour.White);
        whiteMove.Moves.Should().HaveCount(1);
        whiteMove.Moves[0].Piece.Should().Be(PieceType.Pawn);
        whiteMove.Moves[0].Destination.ToString().Should().Be("E4");
    }

    [Fact]
    public void Should_Read_Black_Move_Separately()
    {
        var reader = new AlgebraicNotationReader();
        var blackMove = reader.BlackTurn("e5");

        blackMove.Colour.Should().Be(PieceColour.Black);
        blackMove.Moves.Should().HaveCount(1);
        blackMove.Moves[0].Piece.Should().Be(PieceType.Pawn);
        blackMove.Moves[0].Destination.ToString().Should().Be("E5");
    }

    [Fact]
    public void Should_Read_White_Knight_Move_Separately()
    {
        var reader = new AlgebraicNotationReader();
        var whiteMove = reader.WhiteTurn("Nf3");

        whiteMove.Colour.Should().Be(PieceColour.White);
        whiteMove.Moves[0].Piece.Should().Be(PieceType.Knight);
        whiteMove.Moves[0].Destination.ToString().Should().Be("F3");
    }

    [Fact]
    public void Should_Read_Black_Castling_Separately()
    {
        var reader = new AlgebraicNotationReader();
        var blackMove = reader.BlackTurn("O-O-O");

        blackMove.Colour.Should().Be(PieceColour.Black);
        blackMove.IsCastling.Should().BeTrue();
        blackMove.IsKingSide.Should().BeFalse();
        blackMove.Moves.Should().HaveCount(2);
        blackMove.Moves[0].Piece.Should().Be(PieceType.King);
        blackMove.Moves[0].Destination.ToString().Should().Be("C8");
    }

    [Fact]
    public void Should_Read_White_Capture_With_Check_Separately()
    {
        var reader = new AlgebraicNotationReader();
        var whiteMove = reader.WhiteTurn("Qxf7+");

        whiteMove.Colour.Should().Be(PieceColour.White);
        whiteMove.IsCapture.Should().BeTrue();
        whiteMove.IsCheck.Should().BeTrue();
        whiteMove.Moves[0].Piece.Should().Be(PieceType.Queen);
        whiteMove.Moves[0].Destination.ToString().Should().Be("F7");
    }

    [Fact]
    public void Should_Read_Black_Promotion_Separately()
    {
        var reader = new AlgebraicNotationReader();
        var blackMove = reader.BlackTurn("d1=Q");

        blackMove.Colour.Should().Be(PieceColour.Black);
        blackMove.Moves[0].Piece.Should().Be(PieceType.Pawn);
        blackMove.Moves[0].Destination.ToString().Should().Be("D1");
        blackMove.Promotion.Should().Be(PieceType.Queen);
    }

    [Fact]
    public void Should_Trim_Whitespace_From_Separate_Moves()
    {
        var reader = new AlgebraicNotationReader();
        var whiteMove = reader.WhiteTurn("  e4  ");
        var blackMove = reader.BlackTurn("  e5  ");

        whiteMove.Moves[0].Destination.ToString().Should().Be("E4");
        blackMove.Moves[0].Destination.ToString().Should().Be("E5");
    }

    [Fact]
    public void Should_Reject_Invalid_White_Move_Notation()
    {
        var reader = new AlgebraicNotationReader();

        var act = () => reader.WhiteTurn("invalid");

        act.Should().Throw<AlgebraicNotationException>();
    }

    [Fact]
    public void Should_Reject_Invalid_Black_Move_Notation()
    {
        var reader = new AlgebraicNotationReader();

        var act = () => reader.BlackTurn("xyz123");

        act.Should().Throw<AlgebraicNotationException>();
    }

    [Fact]
    public void Should_Reject_Empty_White_Move()
    {
        var reader = new AlgebraicNotationReader();

        var act = () => reader.WhiteTurn("");

        act.Should().Throw<AlgebraicNotationException>();
    }

    [Fact]
    public void Should_Reject_Empty_Black_Move()
    {
        var reader = new AlgebraicNotationReader();

        var act = () => reader.BlackTurn("   ");

        act.Should().Throw<AlgebraicNotationException>();
    }

    [Fact]
    public void Should_Reject_Invalid_Piece_Letter()
    {
        var reader = new AlgebraicNotationReader();

        var act = () => reader.WhiteTurn("Xe4");

        act.Should().Throw<AlgebraicNotationException>();
    }

    [Fact]
    public void Should_Reject_Invalid_Position_Format()
    {
        var reader = new AlgebraicNotationReader();

        var act = () => reader.BlackTurn("e9");

        act.Should().Throw<AlgebraicNotationException>();
    }

    [Fact]
    public void Should_Accept_Valid_Move_With_Extra_Symbols()
    {
        var reader = new AlgebraicNotationReader();

        var whiteMove = reader.WhiteTurn("Qxf7+");
        var blackMove = reader.BlackTurn("Nxe4#");

        whiteMove.Moves[0].Destination.ToString().Should().Be("F7");
        blackMove.Moves[0].Destination.ToString().Should().Be("E4");
    }
}