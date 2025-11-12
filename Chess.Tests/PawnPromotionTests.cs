using System.Linq;
using Chess.Actions;
using Chess.Pieces;
using Chess.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace Chess.Tests;

public class PawnPromotionTests
{
    [Fact]
    public void White_Pawn_On_Rank_7_Moving_To_Rank_8_Promotes_To_Queen_By_Default()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("E7", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = board.FindPiece(new Position('E', 7));
        var moves = pawn!.PossibleMoves(board).ToList();

        // Pawn should be able to move to E8 and promote
        var promotionMove = moves.FirstOrDefault(m => m.Destination.ToString() == "E8");

        promotionMove.Should().NotBeNull();
        promotionMove!.IsPromotion.Should().BeTrue();

        var promotionAction = promotionMove.Actions.OfType<Promotion>().FirstOrDefault();
        promotionAction.Should().NotBeNull();
        promotionAction!.Piece.Should().Be(PieceType.Queen);
    }

    [Fact]
    public void Black_Pawn_On_Rank_2_Moving_To_Rank_1_Promotes_To_Queen_By_Default()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("D2", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = board.FindPiece(new Position('D', 2));
        var moves = pawn!.PossibleMoves(board).ToList();

        // Pawn should be able to move to D1 and promote
        var promotionMove = moves.FirstOrDefault(m => m.Destination.ToString() == "D1");

        promotionMove.Should().NotBeNull();
        promotionMove!.IsPromotion.Should().BeTrue();

        var promotionAction = promotionMove.Actions.OfType<Promotion>().FirstOrDefault();
        promotionAction.Should().NotBeNull();
        promotionAction!.Piece.Should().Be(PieceType.Queen);
    }

    [Fact]
    public void GetPromotionMovements_Returns_All_Four_Promotion_Options()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("E7", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('E', 7))!;
        var destination = new Position('E', 8);

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        // Should have 4 promotion options
        promotionMoves.Should().HaveCount(4);

        // Check each promotion option
        var pieceTypes = promotionMoves
            .Select(m => m.Actions.OfType<Promotion>().First().Piece)
            .ToList();

        pieceTypes.Should().Contain(PieceType.Queen);
        pieceTypes.Should().Contain(PieceType.Rook);
        pieceTypes.Should().Contain(PieceType.Bishop);
        pieceTypes.Should().Contain(PieceType.Knight);
    }

    [Fact]
    public void Promotion_With_Capture_Includes_Both_Actions()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("E7", PieceColour.White)
            .SetKnightAt("F8", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('E', 7))!;
        var destination = new Position('F', 8);

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        // Should have 4 promotion options, all with capture
        promotionMoves.Should().HaveCount(4);

        foreach (var move in promotionMoves)
        {
            move.IsCapture.Should().BeTrue();
            move.IsPromotion.Should().BeTrue();

            var captureAction = move.Actions.OfType<Capture>().FirstOrDefault();
            captureAction.Should().NotBeNull();
            captureAction!.Piece.Should().Be(PieceType.Knight);
        }
    }

    [Fact]
    public void Underpromotion_To_Knight_Is_Available()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("E7", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('E', 7))!;
        var destination = new Position('E', 8);

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        var knightPromotion = promotionMoves
            .FirstOrDefault(m => m.Actions.OfType<Promotion>().First().Piece == PieceType.Knight);

        knightPromotion.Should().NotBeNull();
    }

    [Fact]
    public void Underpromotion_To_Rook_Is_Available()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("A7", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('A', 7))!;
        var destination = new Position('A', 8);

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        var rookPromotion = promotionMoves
            .FirstOrDefault(m => m.Actions.OfType<Promotion>().First().Piece == PieceType.Rook);

        rookPromotion.Should().NotBeNull();
    }

    [Fact]
    public void Underpromotion_To_Bishop_Is_Available()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("H7", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('H', 7))!;
        var destination = new Position('H', 8);

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        var bishopPromotion = promotionMoves
            .FirstOrDefault(m => m.Actions.OfType<Promotion>().First().Piece == PieceType.Bishop);

        bishopPromotion.Should().NotBeNull();
    }

    [Fact]
    public void Non_Promotion_Move_Returns_No_Promotion_Movements()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("E2", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('E', 2))!;
        var destination = new Position('E', 3); // Not promotion rank

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        // Should be empty - not a promotion move
        promotionMoves.Should().BeEmpty();
    }

    [Fact]
    public void Black_Pawn_Promotion_Has_All_Four_Options()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("D2", PieceColour.Black)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('D', 2))!;
        var destination = new Position('D', 1);

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        promotionMoves.Should().HaveCount(4);

        var pieceTypes = promotionMoves
            .Select(m => m.Actions.OfType<Promotion>().First().Piece)
            .ToList();

        pieceTypes.Should().Contain(PieceType.Queen);
        pieceTypes.Should().Contain(PieceType.Rook);
        pieceTypes.Should().Contain(PieceType.Bishop);
        pieceTypes.Should().Contain(PieceType.Knight);
    }

    [Fact]
    public void Promotion_Move_Destination_Is_Correct()
    {
        var board = new ChessBoardBuilder()
            .SetPawnAt("C7", PieceColour.White)
            .SetKingAt("A1", PieceColour.White)
            .SetKingAt("H8", PieceColour.Black)
            .Build();

        var pawn = (Pawn)board.FindPiece(new Position('C', 7))!;
        var destination = new Position('C', 8);

        var promotionMoves = pawn.GetPromotionMovements(board, destination).ToList();

        // All promotion moves should have the same destination
        foreach (var move in promotionMoves)
        {
            move.Destination.Should().Be(destination);
            move.Origin.Should().Be(new Position('C', 7));
        }
    }
}
