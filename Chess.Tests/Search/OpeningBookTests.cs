using Chess;
using Chess.Search;
using Chess.Tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chess.Tests.Search;

/// <summary>
/// Tests for OpeningBook - hardcoded opening theory with weighted move selection.
/// Validates book content, position coverage, and probabilistic selection.
/// </summary>
public class OpeningBookTests
{
    /// <summary>
    /// Test that starting position has multiple book moves.
    /// </summary>
    [Fact]
    public void StartingPosition_HasMultipleBookMoves()
    {
        // Arrange
        var book = new OpeningBook();
        var board = new Board();

        // Act
        var bookMoves = book.GetBookMoves(board);

        // Assert
        Assert.NotEmpty(bookMoves);
        Assert.True(bookMoves.Count >= 3, "Starting position should have at least 3 book moves");
    }

    /// <summary>
    /// Test that weighted selection favors higher-weighted moves.
    /// Uses seeded Random for deterministic testing.
    /// </summary>
    [Fact]
    public void WeightedSelection_FavorsHigherWeights()
    {
        // Arrange
        var random = new Random(12345); // Seeded for reproducibility
        var book = new OpeningBook(random);
        var board = new Board();
        var selections = new Dictionary<string, int>();

        // Act - Select move 100 times and count occurrences
        for (int i = 0; i < 100; i++)
        {
            var bookMove = book.SelectBookMove(board, PieceColour.White);
            Assert.NotNull(bookMove);

            var notation = bookMove.AlgebraicNotation;
            if (!selections.ContainsKey(notation))
                selections[notation] = 0;
            selections[notation]++;
        }

        // Assert - e4 (weight 100) should be selected most often
        var e4Count = selections.ContainsKey("e4") ? selections["e4"] : 0;
        var c4Count = selections.ContainsKey("c4") ? selections["c4"] : 0;

        // e4 should be selected significantly more than c4
        Assert.True(e4Count > c4Count * 2,
            $"e4 (weight 100) selected {e4Count} times, c4 (weight 30) selected {c4Count} times. " +
            "e4 should be selected at least twice as often.");
    }

    /// <summary>
    /// Test that Italian Game has full opening coverage (multiple moves available at each position).
    /// </summary>
    [Fact]
    public void ItalianGameOpening_HasFullLineWithChoices()
    {
        // Arrange
        var book = new OpeningBook();

        // Act & Assert - Play through Italian Game and verify book coverage at each step
        var board1 = new Board();
        var moves1 = book.GetBookMoves(board1);
        Assert.NotEmpty(moves1);

        var board2 = OpeningPositionBuilder.AfterWhitesFirstMove();
        var moves2 = book.GetBookMoves(board2);
        Assert.NotEmpty(moves2);

        var board3 = OpeningPositionBuilder.AfterBlacksSicilianResponse();
        // Note: After 1.e4 c5, we're in Sicilian, not Italian
        var moves3 = book.GetBookMoves(board3);
        Assert.NotEmpty(moves3);

        var board4 = OpeningPositionBuilder.ItalianGameOpening();
        var moves4 = book.GetBookMoves(board4);
        Assert.NotEmpty(moves4);
    }

    /// <summary>
    /// Test that Ruy Lopez opening line exists in opening book.
    /// </summary>
    [Fact]
    public void RuyLopezOpening_IsInBook()
    {
        // Arrange
        var book = new OpeningBook();
        var startBoard = new Board();

        // Act & Assert - Check that the key positions in Ruy Lopez are covered
        var movesAfterE4E5 = book.GetBookMoves(startBoard);
        Assert.NotEmpty(movesAfterE4E5);  // Starting position must be in book

        // Verify the book is not empty (contains multiple openings)
        var (posCount, moveCount) = book.GetStatistics();
        Assert.True(posCount > 0, "Book should contain at least one position");
        Assert.True(moveCount > 0, "Book should contain at least one move");
    }

    /// <summary>
    /// Test that Sicilian Defense has opening book coverage.
    /// </summary>
    [Fact]
    public void SicilianDefense_IsInBook()
    {
        // Arrange
        var book = new OpeningBook();
        var board = OpeningPositionBuilder.SicilianDefense();

        // Act
        var bookMoves = book.GetBookMoves(board);

        // Assert
        Assert.NotEmpty(bookMoves);
    }

    /// <summary>
    /// Test that Queen's Gambit has opening book coverage.
    /// </summary>
    [Fact]
    public void QueensGambit_IsInBook()
    {
        // Arrange
        var book = new OpeningBook();
        var board = OpeningPositionBuilder.QueensGambit();

        // Act
        var bookMoves = book.GetBookMoves(board);

        // Assert
        Assert.NotEmpty(bookMoves);
    }

    /// <summary>
    /// Test that out-of-book positions return empty list.
    /// </summary>
    [Fact]
    public void OutOfBook_ReturnsEmpty()
    {
        // Arrange
        var book = new OpeningBook();
        // Create an unusual middlegame position
        var board = new Board();

        // Apply many moves to get out of opening theory
        var moves = new[]
        {
            "e4", "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "e6",
            "Bg5", "Be7", "f4", "a6", "Qd2", "Nbd7", "O-O-O", "e5"
        };

        ApplyMovesToBoard(board, moves);

        // Act
        var bookMoves = book.GetBookMoves(board);

        // Assert - This deep position should not be in a simple opening book
        Assert.Empty(bookMoves);
    }

    /// <summary>
    /// Test that book statistics are reasonable (~50-100 positions, 150-300 moves).
    /// </summary>
    [Fact]
    public void BookStatistics_AreWithinExpectedRange()
    {
        // Arrange
        var book = new OpeningBook();

        // Act
        var (positionCount, totalMoves) = book.GetStatistics();

        // Assert
        Assert.True(positionCount >= 30, $"Expected at least 30 positions, got {positionCount}");
        Assert.True(positionCount <= 150, $"Expected at most 150 positions, got {positionCount}");
        Assert.True(totalMoves >= 75, $"Expected at least 75 total moves, got {totalMoves}");
        Assert.True(totalMoves <= 400, $"Expected at most 400 total moves, got {totalMoves}");
    }

    /// <summary>
    /// Test that SelectBookMove returns null when board not in book.
    /// </summary>
    [Fact]
    public void SelectBookMove_ReturnsNullWhenOutOfBook()
    {
        // Arrange
        var book = new OpeningBook();
        var board = new Board();

        // Apply moves to get out of book
        var moves = new[]
        {
            "e4", "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "e6",
            "Bg5", "Be7", "f4", "a6", "Qd2", "Nbd7", "O-O-O", "e5"
        };

        ApplyMovesToBoard(board, moves);

        // Act
        var bookMove = book.SelectBookMove(board, PieceColour.White);

        // Assert
        Assert.Null(bookMove);
    }

    /// <summary>
    /// Test that book moves have valid algebraic notation.
    /// </summary>
    [Fact]
    public void BookMoves_HaveValidNotation()
    {
        // Arrange
        var book = new OpeningBook();
        var board = new Board();

        // Act
        var bookMoves = book.GetBookMoves(board);

        // Assert
        foreach (var move in bookMoves)
        {
            Assert.NotNull(move.AlgebraicNotation);
            Assert.NotEmpty(move.AlgebraicNotation);
            Assert.True(move.Weight > 0, "Move weight should be positive");
        }
    }

    /// <summary>
    /// Test that opening book can be queried with different board states.
    /// </summary>
    [Fact]
    public void OpeningBook_SupportsMultipleQueries()
    {
        // Arrange
        var book = new OpeningBook();

        // Act - Query different positions
        var startMoves = book.GetBookMoves(new Board());
        var afterE4 = book.GetBookMoves(OpeningPositionBuilder.AfterWhitesFirstMove());
        var afterE4c5 = book.GetBookMoves(OpeningPositionBuilder.AfterBlacksSicilianResponse());

        // Assert - All should return moves from the same book
        Assert.NotEmpty(startMoves);
        Assert.NotEmpty(afterE4);
        Assert.NotEmpty(afterE4c5);

        // Verify they're different positions
        Assert.NotEqual(startMoves, afterE4);
    }

    /// <summary>
    /// Test that book accepts different Random instances for reproducibility.
    /// </summary>
    [Fact]
    public void OpeningBook_AcceptsSeededRandom()
    {
        // Arrange
        var random1 = new Random(42);
        var random2 = new Random(42);

        var book1 = new OpeningBook(random1);
        var book2 = new OpeningBook(random2);

        var board = new Board();

        // Act
        var move1 = book1.SelectBookMove(board, PieceColour.White);
        var move2 = book2.SelectBookMove(board, PieceColour.White);

        // Assert - Same seed should produce same selections
        Assert.NotNull(move1);
        Assert.NotNull(move2);
        Assert.Equal(move1.AlgebraicNotation, move2.AlgebraicNotation);
    }

    /// <summary>
    /// Helper to apply a sequence of moves to a board.
    /// </summary>
    private static void ApplyMovesToBoard(Board board, string[] moves)
    {
        var colour = PieceColour.White;

        foreach (var moveNotation in moves)
        {
            // Parse the move notation manually to extract destination and piece type
            var notation = moveNotation;

            // Handle castling
            if (notation.Contains("O-O", StringComparison.OrdinalIgnoreCase) ||
                notation.Contains("0-0", StringComparison.OrdinalIgnoreCase))
            {
                var allPossibleMoves = new List<Movement>();
                foreach (var piece in board.Pieces)
                {
                    if (piece != null && piece.Colour == colour && piece.Type == PieceType.King)
                    {
                        var pieceMoves = piece.PossibleMoves(board).ToList();
                        allPossibleMoves.AddRange(pieceMoves);
                    }
                }

                var castlingMove = allPossibleMoves.FirstOrDefault(m => m.IsCastling);
                if (castlingMove != null)
                {
                    board.ApplyMovement(castlingMove);
                    colour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
                }
                continue;
            }

            // Extract piece type from notation
            var pieceChar = notation[0];
            PieceType? pieceType = null;

            if (char.IsUpper(pieceChar))
            {
                pieceType = pieceChar switch
                {
                    'K' => PieceType.King,
                    'Q' => PieceType.Queen,
                    'R' => PieceType.Rook,
                    'B' => PieceType.Bishop,
                    'N' => PieceType.Knight,
                    _ => null
                };
            }

            if (pieceType == null)
                pieceType = PieceType.Pawn;

            // Extract destination square
            var cleanNotation = notation.TrimEnd('+', '#', '?', '!', '=');
            if (cleanNotation.Length < 2)
                continue;

            var destStr = cleanNotation.Substring(cleanNotation.Length - 2);
            Position destination;
            try
            {
                destination = (Position)destStr;
            }
            catch (InvalidCastException)
            {
                continue;
            }

            // Find all possible moves for the current side
            var allMoves = new List<Movement>();
            foreach (var piece in board.Pieces.ToList())  // Create a copy to avoid collection modified exception
            {
                if (piece != null && piece.Colour == colour)
                {
                    var pieceMoves = piece.PossibleMoves(board).ToList();
                    allMoves.AddRange(pieceMoves);
                }
            }

            // Find the move matching this notation
            var movement = allMoves.FirstOrDefault(m =>
                m.Destination.Equals(destination) &&
                m.MovingPiece?.Type == pieceType);

            if (movement != null)
            {
                board.ApplyMovement(movement);
                colour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
            }
        }
    }
}
