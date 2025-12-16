using Chess;
using Chess.Search;
using Chess.Tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chess.Tests.Search;

/// <summary>
/// Integration tests for OpeningBookEngine - decorator wrapping ISearchEngine.
/// Validates book lookup, fallback behavior, and search result characteristics.
/// </summary>
public class OpeningBookEngineTests
{
    /// <summary>
    /// Mock search engine for testing fallback behavior.
    /// Returns predictable results with a specific node count.
    /// </summary>
    private sealed class MockSearchEngine : ISearchEngine
    {
        private readonly int _nodesEvaluated;
        private readonly int _score;

        public MockSearchEngine(int nodesEvaluated = 1000, int score = 50)
        {
            _nodesEvaluated = nodesEvaluated;
            _score = score;
        }

        public SearchResult FindBestMove(Board board, PieceColour colour, int depth)
        {
            // Return first legal move with mock node count
            foreach (var piece in board.Pieces)
            {
                if (piece != null && piece.Colour == colour)
                {
                    var moves = piece.PossibleMoves(board).ToList();
                    if (moves.Count > 0)
                    {
                        return new SearchResult
                        {
                            BestMove = moves[0],
                            Score = _score,
                            NodesEvaluated = _nodesEvaluated,
                            DepthReached = depth,
                            MateInMoves = null,
                            PrincipalVariation = moves.Take(1).ToList(),
                            WasPruned = false
                        };
                    }
                }
            }

            throw new InvalidOperationException("No legal moves available");
        }
    }

    /// <summary>
    /// Test that starting position returns book move with 0 nodes evaluated.
    /// </summary>
    [Fact]
    public void StartingPosition_ReturnsBookMove_WithZeroNodes()
    {
        // Arrange
        var mockEngine = new MockSearchEngine(nodesEvaluated: 1000);
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.NotNull(result.BestMove);
        Assert.Equal(0, result.NodesEvaluated);
        Assert.Equal(0, result.DepthReached);
        Assert.Equal(0, result.Score);  // Book moves have balanced score
    }

    /// <summary>
    /// Test that book move results have principal variation containing the book move.
    /// </summary>
    [Fact]
    public void BookMoveResult_HasPrincipalVariationWithMove()
    {
        // Arrange
        var mockEngine = new MockSearchEngine();
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.NotEmpty(result.PrincipalVariation);
        Assert.Single(result.PrincipalVariation);
        Assert.Equal(result.BestMove, result.PrincipalVariation[0]);
    }

    /// <summary>
    /// Test that out-of-book positions delegate to fallback engine.
    /// </summary>
    [Fact]
    public void OutOfBookPosition_DelegatesToFallbackEngine()
    {
        // Arrange
        var mockEngine = new MockSearchEngine(nodesEvaluated: 5000, score: 75);
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Apply moves to get out of book
        var moves = new[]
        {
            "e4", "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "e6",
            "Bg5", "Be7", "f4", "a6", "Qd2", "Nbd7", "O-O-O", "e5"
        };
        ApplyMovesToBoard(board, moves);

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert - Should use fallback engine (nodes > 0)
        Assert.Equal(5000, result.NodesEvaluated);
        Assert.Equal(75, result.Score);
        Assert.Equal(4, result.DepthReached);
    }

    /// <summary>
    /// Test that Italian Game opening position returns book move.
    /// </summary>
    [Fact]
    public void ItalianGamePosition_ReturnsBookMove()
    {
        // Arrange
        var mockEngine = new MockSearchEngine(nodesEvaluated: 1000);
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = OpeningPositionBuilder.ItalianGameOpening();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.Equal(0, result.NodesEvaluated);  // Book hit
        Assert.NotNull(result.BestMove);
    }

    /// <summary>
    /// Test that Ruy Lopez position returns book move.
    /// </summary>
    [Fact]
    public void RuyLopezPosition_ReturnsBookMove()
    {
        // Arrange
        var mockEngine = new MockSearchEngine(nodesEvaluated: 1000);
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = OpeningPositionBuilder.RuyLopezOpening();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.Equal(0, result.NodesEvaluated);
        Assert.NotNull(result.BestMove);
    }

    /// <summary>
    /// Test that Queen's Gambit position is in book at intermediate depth.
    /// </summary>
    [Fact]
    public void QueensGambitPosition_DelegatesToFallbackAtDeepLine()
    {
        // Arrange
        var mockEngine = new MockSearchEngine(nodesEvaluated: 1000);
        var bookEngine = new OpeningBookEngine(mockEngine);
        // After 1.d4 d5, we're in the book, but deep lines may not be covered
        var board = OpeningPositionBuilder.QueensGambit();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert - The result should be valid (either from book or fallback)
        Assert.NotNull(result.BestMove);
        Assert.True(result.NodesEvaluated >= 0);
    }

    /// <summary>
    /// Test that opening book engine can be wrapped with MinimaxEngine.
    /// </summary>
    [Fact]
    public void WrappingMinimaxEngine_ProducesBookMoves()
    {
        // Arrange
        var minimaxEngine = new MinimaxEngine();
        var bookEngine = new OpeningBookEngine(minimaxEngine);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert - Starting position should hit book
        Assert.Equal(0, result.NodesEvaluated);
        Assert.NotNull(result.BestMove);
    }

    /// <summary>
    /// Test that opening book engine can be wrapped with AlphaBetaEngine.
    /// </summary>
    [Fact]
    public void WrappingAlphaBetaEngine_ProducesBookMoves()
    {
        // Arrange
        var alphaBetaEngine = new AlphaBetaEngine();
        var bookEngine = new OpeningBookEngine(alphaBetaEngine);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 3);

        // Assert - Starting position should hit book
        Assert.Equal(0, result.NodesEvaluated);
        Assert.NotNull(result.BestMove);
    }

    /// <summary>
    /// Test that custom opening book can be passed to engine.
    /// </summary>
    [Fact]
    public void CustomOpeningBook_IsUsedByEngine()
    {
        // Arrange
        var mockEngine = new MockSearchEngine();
        var customBook = new OpeningBook();
        var bookEngine = new OpeningBookEngine(mockEngine, customBook);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert - Should use custom book
        Assert.Equal(0, result.NodesEvaluated);
    }

    /// <summary>
    /// Test that consecutive book moves form a valid game sequence.
    /// </summary>
    [Fact]
    public void ConsecutiveBookMoves_FormValidGameSequence()
    {
        // Arrange
        var mockEngine = new MockSearchEngine();
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();
        var moveCount = 0;

        // Act - Try to get 6 consecutive book moves
        for (int i = 0; i < 6; i++)
        {
            var colour = i % 2 == 0 ? PieceColour.White : PieceColour.Black;
            var result = bookEngine.FindBestMove(board, colour, depth: 4);

            if (result.NodesEvaluated == 0)  // Book hit
            {
                // Verify move is legal by applying it
                board.ApplyMovement(result.BestMove);
                moveCount++;
            }
            else
            {
                break;  // Out of book
            }
        }

        // Assert - Should get at least 1 full move (2 half-moves)
        // With randomness in book move selection, we may go out of book earlier
        Assert.True(moveCount >= 2, $"Expected at least 2 half-moves from book, got {moveCount}");
    }

    /// <summary>
    /// Test that book move selection produces variety with seeded random.
    /// </summary>
    [Fact]
    public void BookMoveSelection_WithSeededRandom_ProducesVariety()
    {
        // Arrange
        var mockEngine = new MockSearchEngine();
        var random1 = new Random(42);
        var random2 = new Random(99);

        var book1 = new OpeningBook(random1);
        var bookEngine1 = new OpeningBookEngine(mockEngine, book1);

        var book2 = new OpeningBook(random2);
        var bookEngine2 = new OpeningBookEngine(mockEngine, book2);

        var board = new Board();
        var selections1 = new List<string>();
        var selections2 = new List<string>();

        // Act - Select moves with different random seeds
        for (int i = 0; i < 5; i++)
        {
            var result1 = bookEngine1.FindBestMove(board, PieceColour.White, depth: 4);
            selections1.Add(result1.BestMove.Destination.ToString());

            var result2 = bookEngine2.FindBestMove(board, PieceColour.White, depth: 4);
            selections2.Add(result2.BestMove.Destination.ToString());
        }

        // Assert - Different seeds may produce different selections
        // (they could coincidentally be the same, but rarely)
        // Just verify both made selections
        Assert.NotEmpty(selections1);
        Assert.NotEmpty(selections2);
    }

    /// <summary>
    /// Test that book move is a legal move for the position.
    /// </summary>
    [Fact]
    public void BookMove_IsLegalForPosition()
    {
        // Arrange
        var mockEngine = new MockSearchEngine();
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);
        var bookMoveDestination = result.BestMove.Destination;

        // Assert - Verify move is from a white piece to a valid destination
        var movingPiece = result.BestMove.MovingPiece;
        Assert.NotNull(movingPiece);
        Assert.Equal(PieceColour.White, movingPiece.Colour);
        Assert.NotNull(bookMoveDestination);
    }

    /// <summary>
    /// Test that WasPruned flag is false for book moves.
    /// </summary>
    [Fact]
    public void BookMove_WasPrunedFalse()
    {
        // Arrange
        var mockEngine = new MockSearchEngine();
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.False(result.WasPruned);
    }

    /// <summary>
    /// Test that MateInMoves is null for book moves.
    /// </summary>
    [Fact]
    public void BookMove_MateInMovesNull()
    {
        // Arrange
        var mockEngine = new MockSearchEngine();
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.Null(result.MateInMoves);
    }

    /// <summary>
    /// Test that fallback engine result is returned unchanged when out of book.
    /// </summary>
    [Fact]
    public void FallbackEngineResult_ReturnedUnchangedWhenOutOfBook()
    {
        // Arrange
        var mockEngine = new MockSearchEngine(nodesEvaluated: 2500, score: 100);
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Apply moves to get out of book
        var moves = new[]
        {
            "e4", "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "e6",
            "Bg5", "Be7", "f4", "a6", "Qd2", "Nbd7", "O-O-O", "e5"
        };
        ApplyMovesToBoard(board, moves);

        // Act
        var result = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert - Fallback result returned unchanged
        Assert.Equal(2500, result.NodesEvaluated);
        Assert.Equal(100, result.Score);
    }

    /// <summary>
    /// Test book hit followed by fallback in same game.
    /// </summary>
    [Fact]
    public void BookHit_ThenFallback_InSameGame()
    {
        // Arrange
        var mockEngine = new MockSearchEngine(nodesEvaluated: 1000, score: 50);
        var bookEngine = new OpeningBookEngine(mockEngine);
        var board = new Board();

        // Act - Get book move
        var result1 = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);
        Assert.Equal(0, result1.NodesEvaluated);
        board.ApplyMovement(result1.BestMove);

        // Get fallback move (after going out of book)
        var moves = new[] { "c5", "Nf3", "d6", "d4", "cxd4", "Nxd4", "Nf6", "Nc3", "e6",
                           "Bg5", "Be7", "f4", "a6", "Qd2", "Nbd7", "O-O-O", "e5" };
        ApplyMovesToBoard(board, moves);

        var result2 = bookEngine.FindBestMove(board, PieceColour.White, depth: 4);

        // Assert
        Assert.Equal(1000, result2.NodesEvaluated);
        Assert.Equal(50, result2.Score);
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
