namespace Chess.Strategies.Helpers;

/// <summary>
/// Analyzes pawn structures to evaluate pawn-related aspects of positions.
/// </summary>
public static class PawnAnalyzer
{
    /// <summary>
    /// Determines if a pawn is a passed pawn (no enemy pawns ahead on same or adjacent files).
    /// </summary>
    public static bool IsPassedPawn(Board board, Piece pawn, Position position)
    {
        if (!pawn.IsPawn)
            return false;

        var enemyColor = pawn.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

        // For white pawns, check if any enemy pawns are ahead (higher rank)
        // For black pawns, check if any enemy pawns are ahead (lower rank)
        if (pawn.IsWhite)
        {
            // Check ranks above the pawn
            for (var checkRank = position.Y + 1; checkRank <= 8; checkRank++)
            {
                // Check same file and adjacent files
                for (var checkFile = position.X - 1; checkFile <= position.X + 1; checkFile++)
                {
                    if (checkFile < 'A' || checkFile > 'H')
                        continue;

                    var checkPos = new Position((char)checkFile, checkRank);
                    var piece = board.FindPiece(checkPos);

                    if (piece != null && piece.IsPawn && piece.Colour == enemyColor)
                    {
                        return false; // Enemy pawn blocks passage
                    }
                }
            }
        }
        else
        {
            // Check ranks below the pawn
            for (var checkRank = position.Y - 1; checkRank >= 1; checkRank--)
            {
                for (var checkFile = position.X - 1; checkFile <= position.X + 1; checkFile++)
                {
                    if (checkFile < 'A' || checkFile > 'H')
                        continue;

                    var checkPos = new Position((char)checkFile, checkRank);
                    var piece = board.FindPiece(checkPos);

                    if (piece != null && piece.IsPawn && piece.Colour == enemyColor)
                    {
                        return false;
                    }
                }
            }
        }

        return true; // No enemy pawns block progression
    }

    /// <summary>
    /// Determines if a pawn is doubled (another friendly pawn on same file).
    /// </summary>
    public static bool IsDoubledPawn(Board board, Piece pawn)
    {
        if (!pawn.IsPawn)
            return false;

        var file = pawn.Position.X;
        return board.Pieces.Any(p => p.IsPawn && p.Colour == pawn.Colour && p.Position.X == file && !p.Position.Equals(pawn.Position));
    }

    /// <summary>
    /// Determines if a pawn is isolated (no friendly pawns on adjacent files).
    /// </summary>
    public static bool IsIsolatedPawn(Board board, Piece pawn)
    {
        if (!pawn.IsPawn)
            return false;

        var file = pawn.Position.X;
        var leftFile = (char)(file - 1);
        var rightFile = (char)(file + 1);

        bool hasLeftNeighbor = file > 'A' && board.Pieces.Any(p =>
            p.IsPawn && p.Colour == pawn.Colour && p.Position.X == leftFile);

        bool hasRightNeighbor = file < 'H' && board.Pieces.Any(p =>
            p.IsPawn && p.Colour == pawn.Colour && p.Position.X == rightFile);

        return !hasLeftNeighbor && !hasRightNeighbor;
    }

    /// <summary>
    /// Counts the number of pawns on the same file.
    /// Used to detect doubled/tripled pawns.
    /// </summary>
    public static int GetPawnsOnFile(Board board, PieceColour color, char file)
    {
        return board.Pieces.Count(p =>
            p.IsPawn && p.Colour == color && p.Position.X == file);
    }

    /// <summary>
    /// Counts pawn islands (groups of consecutive pawns separated by empty files).
    /// </summary>
    public static int CountPawnIslands(Board board, PieceColour color)
    {
        var pawnFiles = new HashSet<char>();

        foreach (var pawn in board.Pieces.Where(p => p.IsPawn && p.Colour == color))
        {
            pawnFiles.Add(pawn.Position.X);
        }

        if (pawnFiles.Count == 0)
            return 0;

        int islands = 0;
        bool inIsland = false;

        for (char file = 'A'; file <= 'H'; file++)
        {
            if (pawnFiles.Contains(file))
            {
                if (!inIsland)
                {
                    islands++;
                    inIsland = true;
                }
            }
            else
            {
                inIsland = false;
            }
        }

        return islands;
    }

    /// <summary>
    /// Gets the advancement distance of a pawn towards promotion.
    /// Returns 0-6 for white (rank 2-8), 0-6 for black (rank 7-1).
    /// </summary>
    public static int GetPawnAdvancement(Piece pawn)
    {
        if (!pawn.IsPawn)
            return 0;

        return pawn.IsWhite ? pawn.Position.Y - 2 : 7 - pawn.Position.Y;
    }
}
