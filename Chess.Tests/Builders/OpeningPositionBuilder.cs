using Chess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Tests.Builders;

/// <summary>
/// Builder for creating standard opening positions for testing.
/// Reduces boilerplate in test setup by providing pre-built opening positions.
/// </summary>
public static class OpeningPositionBuilder
{
    /// <summary>
    /// Returns the standard starting position.
    /// </summary>
    public static Board StartingPosition() => new Board();

    /// <summary>
    /// Returns position after Italian Game opening: 1.e4 e5 2.Nf3 Nc6 3.Bc4 Bc5
    /// </summary>
    public static Board ItalianGameOpening()
    {
        var board = new Board();
        ApplyMoves(board, "e4", "e5", "Nf3", "Nc6", "Bc4", "Bc5");
        return board;
    }

    /// <summary>
    /// Returns position after Ruy Lopez opening: 1.e4 e5 2.Nf3 Nc6 3.Bb5 a6
    /// </summary>
    public static Board RuyLopezOpening()
    {
        var board = new Board();
        ApplyMoves(board, "e4", "e5", "Nf3", "Nc6", "Bb5", "a6");
        return board;
    }

    /// <summary>
    /// Returns position after Sicilian Defense: 1.e4 c5 2.Nf3 d6 3.d4 cxd4
    /// </summary>
    public static Board SicilianDefense()
    {
        var board = new Board();
        ApplyMoves(board, "e4", "c5", "Nf3", "d6", "d4", "cxd4");
        return board;
    }

    /// <summary>
    /// Returns position after French Defense: 1.e4 e6 2.d4 d5
    /// </summary>
    public static Board FrenchDefense()
    {
        var board = new Board();
        ApplyMoves(board, "e4", "e6", "d4", "d5");
        return board;
    }

    /// <summary>
    /// Returns position after Queen's Gambit: 1.d4 d5 2.c4
    /// </summary>
    public static Board QueensGambit()
    {
        var board = new Board();
        ApplyMoves(board, "d4", "d5", "c4");
        return board;
    }

    /// <summary>
    /// Returns position after King's Indian Defense: 1.d4 Nf6 2.c4 g6 3.Nc3 Bg7
    /// </summary>
    public static Board KingsIndianDefense()
    {
        var board = new Board();
        ApplyMoves(board, "d4", "Nf6", "c4", "g6", "Nc3", "Bg7");
        return board;
    }

    /// <summary>
    /// Returns position after Nimzo-Indian: 1.d4 Nf6 2.c4 e6 3.Nc3 Bb4
    /// </summary>
    public static Board NimzoIndian()
    {
        var board = new Board();
        ApplyMoves(board, "d4", "Nf6", "c4", "e6", "Nc3", "Bb4");
        return board;
    }

    /// <summary>
    /// Returns position after Giuoco Piano: 1.e4 e5 2.Nf3 Nc6 3.Bc4 Bc5 4.c3
    /// </summary>
    public static Board GiuocoPiano()
    {
        var board = new Board();
        ApplyMoves(board, "e4", "e5", "Nf3", "Nc6", "Bc4", "Bc5", "c3");
        return board;
    }

    /// <summary>
    /// Returns position after Caro-Kann Defense: 1.e4 c6 2.d4 d5
    /// </summary>
    public static Board CaroKann()
    {
        var board = new Board();
        ApplyMoves(board, "e4", "c6", "d4", "d5");
        return board;
    }

    /// <summary>
    /// Returns position after one move: 1.e4
    /// </summary>
    public static Board AfterWhitesFirstMove()
    {
        var board = new Board();
        ApplyMoves(board, "e4");
        return board;
    }

    /// <summary>
    /// Returns position after two moves: 1.e4 c5
    /// </summary>
    public static Board AfterBlacksSicilianResponse()
    {
        var board = new Board();
        ApplyMoves(board, "e4", "c5");
        return board;
    }

    /// <summary>
    /// Helper method to apply a sequence of moves to a board.
    /// </summary>
    private static void ApplyMoves(Board board, params string[] moves)
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
                if (castlingMove == null)
                {
                    throw new InvalidOperationException(
                        $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
                }

                board.ApplyMovement(castlingMove);
                colour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
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
            {
                throw new InvalidOperationException(
                    $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
            }

            var destStr = cleanNotation.Substring(cleanNotation.Length - 2);
            Position destination;
            try
            {
                destination = (Position)destStr;
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException(
                    $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
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

            if (movement == null)
            {
                throw new InvalidOperationException(
                    $"Cannot apply move '{moveNotation}' in sequence: {string.Join(" ", moves)}");
            }

            // Apply the move to the board
            board.ApplyMovement(movement);

            // Toggle to the other colour
            colour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
        }
    }
}
