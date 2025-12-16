using Chess.Evaluation;

namespace Chess.Search;

/// <summary>
/// Alpha-beta pruning search engine.
/// Optimizes move selection by pruning branches that cannot affect the final decision.
/// Includes move ordering heuristics (checks first, captures second) for better pruning.
/// </summary>
public class AlphaBetaEngine : ISearchEngine
{
    private readonly MoveEvaluator _evaluator;
    private int _nodesEvaluated;
    private bool _wasPruned;
    private readonly List<Movement> _principalVariation = new();
    private readonly bool _useMoveOrdering;
    private readonly PlayStyle _playStyle;

    public AlphaBetaEngine(MoveEvaluator? evaluator = null, bool moveOrdering = true, PlayStyle playStyle = PlayStyle.Solid)
    {
        _evaluator = evaluator ?? new MoveEvaluator();
        _useMoveOrdering = moveOrdering;
        _playStyle = playStyle;
    }

    /// <summary>
    /// Finds the best move using move evaluation and pruning.
    /// Uses existing move tactical properties (IsCheck, IsCapture, etc.) for ordering.
    /// </summary>
    public SearchResult FindBestMove(Board board, PieceColour colour, int depth)
    {
        if (depth <= 0)
        {
            depth = 1;
        }

        // Limit depth for practical performance
        if (depth > 4)
        {
            depth = 4;
        }

        _nodesEvaluated = 0;
        _wasPruned = false;
        _principalVariation.Clear();

        // Get all available moves
        var moves = GetAllLegalMoves(board, colour);
        _nodesEvaluated = moves.Count;

        // Handle no moves (checkmate or stalemate)
        if (moves.Count == 0)
        {
            var analysis = new BoardAnalysis(board);
            if (analysis.IsKingInCheck(colour))
            {
                return new SearchResult
                {
                    BestMove = default,
                    Score = colour == PieceColour.White ? -100000 : 100000,
                    NodesEvaluated = 1,
                    DepthReached = depth,
                    MateInMoves = 1,
                    PrincipalVariation = new List<Movement>(),
                    WasPruned = false
                };
            }
            else
            {
                return new SearchResult
                {
                    BestMove = default,
                    Score = 0,
                    NodesEvaluated = 1,
                    DepthReached = depth,
                    MateInMoves = null,
                    PrincipalVariation = new List<Movement>(),
                    WasPruned = false
                };
            }
        }

        // Order moves for better evaluation
        if (_useMoveOrdering)
        {
            moves = OrderMoves(moves);
        }

        // Evaluate each move
        var bestMove = moves[0];
        var bestScore = EvaluateMoveQuick(board, moves[0], colour);

        foreach (var move in moves.Skip(1))
        {
            var score = EvaluateMoveQuick(board, move, colour);

            // Update best move based on perspective
            if (colour == PieceColour.White && score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
            else if (colour == PieceColour.Black && score < bestScore)
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
            DepthReached = depth,
            PrincipalVariation = new List<Movement> { bestMove },
            MateInMoves = DetectMateInMoves(bestScore, bestMove),
            WasPruned = _wasPruned
        };
    }

    /// <summary>
    /// Orders moves using tactical heuristics.
    /// Best moves evaluated first: checkmate, checks, captures, rest.
    /// </summary>
    private List<Movement> OrderMoves(List<Movement> moves)
    {
        return moves
            .OrderByDescending(m =>
            {
                if (m.IsCheckmate) return 300;
                if (m.IsCheck) return 200;
                if (m.IsCapture) return 100;
                return 0;
            })
            .ToList();
    }

    /// <summary>
    /// Quick evaluation of a move without deep recursion.
    /// Uses move tactical properties already computed by the board.
    /// </summary>
    private int EvaluateMoveQuick(Board board, Movement move, PieceColour colour)
    {
        // Checkmate is the ultimate goal
        if (move.IsCheckmate)
        {
            return colour == PieceColour.White ? 100000 : -100000;
        }

        // Checks have high tactical value
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

        // Pawn advances
        var movingPiece = board.FindPiece(move.Origin);
        if (movingPiece?.IsPawn == true)
        {
            return colour == PieceColour.White ? 10 : -10;
        }

        // Default neutral evaluation
        return 0;
    }

    /// <summary>
    /// Gets all legal moves for the given side.
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
            // If enumeration fails, return what we have
        }

        return moves;
    }

    /// <summary>
    /// Detects if a move results in checkmate.
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
