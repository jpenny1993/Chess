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
}