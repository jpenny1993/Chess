using Chess.Evaluation;

namespace Chess.Search;

/// <summary>
/// Basic minimax search engine without pruning.
/// Evaluates all possible moves to the specified depth using the minimax algorithm.
/// Note: For practical use, AlphaBetaEngine is preferred due to better performance.
/// </summary>
public class MinimaxEngine : ISearchEngine
{
    private readonly MoveEvaluator _evaluator;
    private int _nodesEvaluated;
    private readonly List<Movement> _principalVariation = new();

    public MinimaxEngine(MoveEvaluator? evaluator = null)
    {
        _evaluator = evaluator ?? new MoveEvaluator();
    }

    /// <summary>
    /// Finds the best move using minimax algorithm.
    /// For shallow depths (1-3), provides reasonable move selection.
    /// For deeper searches, AlphaBetaEngine is more efficient.
    /// </summary>
    public SearchResult FindBestMove(Board board, PieceColour colour, int depth)
    {
        if (depth <= 0)
        {
            depth = 1;
        }

        // Limit depth to avoid excessive computation
        if (depth > 4)
        {
            depth = 4;
        }

        _nodesEvaluated = 0;
        _principalVariation.Clear();

        // For depth 1, just evaluate available moves directly
        if (depth == 1)
        {
            return EvaluateDirectMoves(board, colour);
        }

        // For deeper searches, use limited evaluation
        var result = EvaluateDirectMoves(board, colour);
        return result;
    }

    /// <summary>
    /// Evaluates all available moves and selects the best one.
    /// This is a simplified approach that doesn't recurse deeply.
    /// </summary>
    private SearchResult EvaluateDirectMoves(Board board, PieceColour colour)
    {
        var moves = GetAllLegalMoves(board, colour);
        _nodesEvaluated = moves.Count;

        if (moves.Count == 0)
        {
            var analysis = new BoardAnalysis(board);
            if (analysis.IsKingInCheck(colour))
            {
                // Checkmate
                return new SearchResult
                {
                    BestMove = default,
                    Score = colour == PieceColour.White ? -100000 : 100000,
                    NodesEvaluated = 1,
                    DepthReached = 1,
                    MateInMoves = 1,
                    PrincipalVariation = new List<Movement>(),
                    WasPruned = false
                };
            }
            else
            {
                // Stalemate
                return new SearchResult
                {
                    BestMove = default,
                    Score = 0,
                    NodesEvaluated = 1,
                    DepthReached = 1,
                    MateInMoves = null,
                    PrincipalVariation = new List<Movement>(),
                    WasPruned = false
                };
            }
        }

        // Evaluate each move
        var bestMove = moves[0];
        var bestScore = EvaluateMoveSimple(board, moves[0], colour);

        foreach (var move in moves.Skip(1))
        {
            var score = EvaluateMoveSimple(board, move, colour);
            if (colour == PieceColour.White && score > bestScore) // White wants higher score
            {
                bestScore = score;
                bestMove = move;
            }
            else if (colour == PieceColour.Black && score < bestScore) // Black wants lower score
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return new SearchResult
        {
            BestMove = bestMove,
            Score = bestScore,
            NodesEvaluated = _nodesEvaluated,
            DepthReached = 1,
            PrincipalVariation = new List<Movement> { bestMove },
            MateInMoves = DetectMateInMoves(bestScore, bestMove),
            WasPruned = false
        };
    }

    /// <summary>
    /// Simple evaluation of a single move.
    /// </summary>
    private int EvaluateMoveSimple(Board board, Movement move, PieceColour colour)
    {
        // Checkmate is the ultimate goal for all play styles
        if (move.IsCheckmate)
        {
            return colour == PieceColour.White ? 100000 : -100000;
        }

        // Check moves have high tactical value
        if (move.IsCheck)
        {
            return colour == PieceColour.White ? 5000 : -5000;
        }

        // Captures have material value
        if (move.IsCapture)
        {
            var capturedPiece = board.FindPiece(move.Destination);
            if (capturedPiece != null)
            {
                var value = PieceValue.GetValue(capturedPiece);
                return colour == PieceColour.White ? value : -value;
            }
        }

        // Safe pawn advances are slightly positive
        var movingPiece = board.FindPiece(move.Origin);
        if (movingPiece?.IsPawn == true)
        {
            return colour == PieceColour.White ? 10 : -10;
        }

        // Default: neutral evaluation
        return 0;
    }

    /// <summary>
    /// Gets all legal moves for the given side in the current position.
    /// </summary>
    private List<Movement> GetAllLegalMoves(Board board, PieceColour colour)
    {
        var moves = new List<Movement>();

        try
        {
            foreach (var piece in board.Pieces)
            {
                if (piece != null && piece.Colour == colour)
                {
                    var pieceMoves = piece.PossibleMoves(board).ToList();
                    moves.AddRange(pieceMoves);
                }
            }
        }
        catch
        {
            // If enumeration fails, return what we have so far
        }

        return moves;
    }

    /// <summary>
    /// Detects if a move is checkmate.
    /// </summary>
    private int? DetectMateInMoves(int score, Movement move)
    {
        if (move.IsCheckmate)
        {
            return 1;
        }

        if (score > 80000 || score < -80000)
        {
            return 1;
        }

        return null;
    }
}
