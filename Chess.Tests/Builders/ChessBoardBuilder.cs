using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;

namespace Chess.Tests.Builders;

public sealed class ChessBoardBuilder
{
    private Piece _movingPiece = new Pawn(PieceColour.White, 'A', 1);
    private readonly List<Piece> _pieces = new();

    public ChessBoardBuilder SetBishopAt(string tile, PieceColour colour = PieceColour.Black)
    {
        AddOrRemovePiece(new Bishop(colour, tile[0], int.Parse(tile.Substring(1, 1))));
        return this;
    }
    
    public ChessBoardBuilder SetKingAt(string tile, PieceColour colour = PieceColour.Black)
    {
        AddOrRemovePiece(new King(colour, tile[0], int.Parse(tile.Substring(1, 1))));
        return this;
    }
    
    public ChessBoardBuilder SetKnightAt(string tile, PieceColour colour = PieceColour.Black)
    {
        AddOrRemovePiece(new Knight(colour, tile[0], int.Parse(tile.Substring(1, 1))));
        return this;
    }
    
    public ChessBoardBuilder SetPawnAt(string tile, PieceColour colour = PieceColour.Black)
    {
        AddOrRemovePiece(new Pawn(colour, tile[0], int.Parse(tile.Substring(1, 1))));
        return this;
    }
    
    public ChessBoardBuilder SetQueenAt(string tile, PieceColour colour = PieceColour.Black)
    {
        AddOrRemovePiece(new Pawn(colour, tile[0], int.Parse(tile.Substring(1, 1))));
        return this;
    }

    public ChessBoardBuilder SetRookAt(string tile, PieceColour colour = PieceColour.Black)
    {
        AddOrRemovePiece(new Rook(colour, tile[0], int.Parse(tile.Substring(1, 1))));
        return this;
    }

    public ChessBoardBuilder WithBlackPieces(params char[] tiles)
    {
        AddPieces(PieceColour.Black, tiles);
        if (_pieces.Count == 1)
        {
            _movingPiece = _pieces.First();
        }

        return this;
    }
    
    public ChessBoardBuilder WithWhitePieces(params char[] tiles)
    {
        AddPieces(PieceColour.White, tiles);
        if (_pieces.Count == 1)
        {
            _movingPiece = _pieces.First();
        }
        
        return this;
    }

    public Board Build()
    {
        return new Board(_pieces);
    }

    public IReadOnlyCollection<string> BuildCoordinates()
    {
        return _pieces
            .OrderBy(b => b.Position.X)
            .ThenBy(b => b.Position.Y)
            .Select(b => b.Position.ToString())
            .ToArray();
    }

    public IReadOnlyCollection<string> BuildPossibleMoves()
    {
        return _movingPiece
            .PossibleMoves(Build())
            .OrderBy(m => m.Position.X)
            .ThenBy(m => m.Position.Y)
            .Select(m => m.Position.ToString())
            .ToArray();
    }
    
    private void AddOrRemovePiece(Piece piece)
    {
        _pieces.RemoveAll(p => p.Position.Equals(piece.Position));
        _pieces.Add(piece);
        _movingPiece = piece;
    }

    private void AddPieces(PieceColour colour, char[] tiles)
    {
        if (tiles.Length != 64)
        {
            throw new ArgumentException(
                "Invalid tile set. Expecting 64 tiles, the chessboard is 8 x 8.",
                nameof(tiles));
        }

        var x = Position.MinX;
        var y = Position.MaxY;

        foreach (var tile in tiles)
        {
            switch (tile)
            {
                case (char)PieceType.Bishop:
                    _pieces.Add(new Bishop(colour, x, y));
                    break;
                case (char)PieceType.King:
                    _pieces.Add(new King(colour, x, y));
                    break;
                case (char)PieceType.Knight:
                    _pieces.Add(new Knight(colour, x, y));
                    break;
                case (char)PieceType.Pawn:
                    _pieces.Add(new Pawn(colour, x, y));
                    break;
                case (char)PieceType.Queen:
                    _pieces.Add(new Queen(colour, x, y));
                    break;
                case (char)PieceType.Rook:
                    _pieces.Add(new Rook(colour, x, y));
                    break;
                default: break;
            }

            if (x == Position.MaxX)
            {
                x = Position.MinX;
                y--;
            }
            else
            {
                x++;
            }
        }
    }

}