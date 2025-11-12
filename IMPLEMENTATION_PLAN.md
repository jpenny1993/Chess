# Chess Library - Implementation Plan

This document outlines the roadmap for completing the chess library and building it into a full chess engine.

## Phase 1: Bug Fixes (Priority: Critical)

### 1.1 Fix Existing Bugs

**Files to Fix:**

1. **Chess/Pieces/Pawn.cs:77, 83**
   - Issue: Capture logic uses `y2` instead of `y`
   - Impact: Pawns check wrong row for diagonal captures
   - Fix: Change to use correct row variable

2. **Chess/Pieces/Queen.cs:30**
   - Issue: Right-down diagonal calculation wrong
   - Fix: Change `Position.X - offset` to `Position.X + offset`

3. **Chess/Board.cs:120**
   - Issue: King and Queen starting positions swapped
   - Fix: Swap the initialization positions

**Estimated Time:** 1-2 hours

---

## Phase 2: Complete Core Game Rules (Priority: High)

### 2.1 Check Detection System

**Objective:** Implement logic to detect when a king is in check

**Implementation:**

1. Create `CheckDetector` class in Chess/GameLogic/
   - `IsKingInCheck(Board board, PieceColour kingColour)`
   - `GetCheckingPieces(Board board, PieceColour kingColour)`
   - `GetSquaresUnderAttack(Board board, PieceColour attackingColour)`

2. Update `Board` class
   - Add `IsInCheck(PieceColour colour)` method
   - Add `GetKingPosition(PieceColour colour)` helper

3. Integration
   - Update `Piece.PossibleMoves()` to filter moves that leave king in check
   - Mark movements with `Check` action when they put opponent in check

**Test Coverage:**
- Test king in check from each piece type
- Test multiple checking pieces
- Test discovered check
- Test moving into check (should be invalid)

**Estimated Time:** 4-6 hours

### 2.2 Checkmate and Stalemate Detection

**Objective:** Detect game-ending conditions

**Implementation:**

1. Create `GameStateAnalyzer` class in Chess/GameLogic/
   - `IsCheckmate(Board board, PieceColour colour)`
   - `IsStalemate(Board board, PieceColour colour)`
   - `HasLegalMoves(Board board, PieceColour colour)`

2. Logic:
   - Checkmate: King in check AND no legal moves
   - Stalemate: King NOT in check AND no legal moves

3. Update `Board` or create `Game` class
   - `GameState` enum: InProgress, Checkmate, Stalemate, Draw
   - `GetGameState()` method

**Test Coverage:**
- Back-rank mate
- Smothered mate
- Queen + King mate
- Stalemate scenarios
- Positions with legal moves remaining

**Estimated Time:** 4-6 hours

### 2.3 Castling Validation

**Objective:** Implement full castling rules

**Implementation:**

1. Create `CastlingValidator` class in Chess/GameLogic/
   - `CanCastleKingside(Board board, PieceColour colour)`
   - `CanCastleQueenside(Board board, PieceColour colour)`

2. Rules to enforce:
   - King and rook haven't moved (requires move history)
   - No pieces between king and rook
   - King not currently in check
   - King doesn't pass through check
   - King doesn't land in check

3. Update `King` and `Rook` classes
   - Track `HasMoved` property
   - Update in move execution

4. Update castling movement generation

**Test Coverage:**
- Valid castling both sides
- Invalid: pieces between
- Invalid: king in check
- Invalid: passing through check
- Invalid: after king moved
- Invalid: after rook moved

**Estimated Time:** 4-6 hours

### 2.4 En Passant

**Objective:** Implement en passant capture rule

**Implementation:**

1. Create `EnPassant` action class (implements IAction)

2. Track last move in Board or Game
   - Store last moved piece and positions

3. Update `Pawn.PossibleMoves()`
   - Check if enemy pawn just moved two squares
   - Check if current pawn is in correct position
   - Add en passant capture to possible moves

4. Update move execution to handle en passant

**Test Coverage:**
- Valid en passant both sides
- Invalid: too late (not immediately after)
- Invalid: enemy pawn moved one square
- Invalid: wrong pawn positions

**Estimated Time:** 3-4 hours

### 2.5 Pawn Promotion

**Objective:** Complete pawn promotion logic

**Implementation:**

1. Update `Pawn` class
   - Detect when pawn reaches back rank
   - Create `Promotion` action with available pieces

2. Create promotion selection mechanism
   - Interface: `IPromotionSelector`
   - Console implementation: prompt user
   - Engine implementation: evaluation-based

3. Update move execution
   - Replace pawn with selected piece
   - Default to Queen if not specified

**Test Coverage:**
- Promote to Queen, Rook, Bishop, Knight
- Promotion with check
- Promotion with checkmate
- Promotion from notation

**Estimated Time:** 2-3 hours

---

## Phase 3: Game State Management (Priority: High)

### 3.1 Turn Management

**Objective:** Track game turns and enforce turn order

**Implementation:**

1. Create `Game` class in Chess/
   - `Board CurrentBoard { get; }`
   - `PieceColour CurrentTurn { get; }`
   - `List<Movement> MoveHistory { get; }`
   - `int TurnNumber { get; }`

2. Methods:
   - `ApplyMove(Movement move)`
   - `UndoMove()`
   - `GetLegalMoves()`
   - `IsGameOver()`

3. Move validation:
   - Verify correct player's turn
   - Verify move is legal
   - Apply check/checkmate detection

**Test Coverage:**
- Turn alternation
- Move history tracking
- Undo functionality
- Legal move generation

**Estimated Time:** 4-6 hours

### 3.2 Move Execution from Notation

**Objective:** Apply moves specified in algebraic notation

**Implementation:**

1. Create `NotationExecutor` class
   - `ApplyNotatedMove(Board board, NotedPlayerMove move, PieceColour colour)`
   - Disambiguation logic for notation

2. Match notation to actual moves:
   - Find pieces that can make the move
   - Use file/rank hints for disambiguation
   - Validate move is legal

3. Update `Board.ApplyTurn()` method

**Test Coverage:**
- Unambiguous moves
- Ambiguous moves (multiple pieces)
- File disambiguation (Nbd2)
- Rank disambiguation (N1f3)
- Both file+rank disambiguation

**Estimated Time:** 4-6 hours

### 3.3 Draw Conditions

**Objective:** Implement all draw rules

**Implementation:**

1. **Fifty-move rule**
   - Track half-moves since last pawn move or capture
   - Auto-draw at 75 moves (mandatory)
   - Optional claim at 50 moves

2. **Threefold repetition**
   - Track position history with hash
   - Auto-draw at fivefold (mandatory)
   - Optional claim at threefold

3. **Insufficient material**
   - King vs King
   - King+Bishop vs King
   - King+Knight vs King
   - King+Bishop vs King+Bishop (same color squares)

4. Create `DrawDetector` class
   - `IsFiftyMoveRule(Game game)`
   - `IsThreefoldRepetition(Game game)`
   - `IsInsufficientMaterial(Board board)`

**Test Coverage:**
- Each draw condition independently
- Game ending by each draw type

**Estimated Time:** 4-6 hours

---

## Phase 4: Testing and Quality (Priority: High)

### 4.1 Complete Unit Tests

**Objective:** Achieve comprehensive test coverage

**Tests Needed:**

1. **Piece Movement Tests**
   - Pawn: forward, capture, double-move, edge cases
   - Knight: all positions, blocked paths
   - Rook: horizontal, vertical, blocked
   - Queen: all directions, blocked

2. **Game Logic Tests**
   - Check detection (all scenarios)
   - Checkmate patterns
   - Stalemate patterns
   - Castling validation
   - En passant
   - Pawn promotion

3. **Integration Tests**
   - Complete game playthrough
   - Famous games from notation
   - Edge case positions

**Estimated Time:** 8-12 hours

### 4.2 Board Validation

**Objective:** Ensure board state integrity

**Implementation:**

1. Create `BoardValidator` class
   - Validate piece counts
   - Validate king positions (exactly one per side)
   - Validate pawn positions (not on back ranks)
   - Validate board consistency

2. Add validation to Board constructor
3. Add validation after move execution

**Estimated Time:** 2-3 hours

---

## Phase 5: Chess Engine (Priority: Medium)

### 5.1 Position Evaluation

**Objective:** Evaluate board positions numerically

**Implementation:**

1. Create `PositionEvaluator` class
   - Material count (piece values)
   - Piece-square tables
   - Pawn structure analysis
   - King safety
   - Mobility
   - Center control

2. Basic evaluation function:
   ```
   score = materialScore +
           positionalScore +
           mobilityScore +
           kingSafetyScore
   ```

**Piece Values (centipawns):**
- Pawn: 100
- Knight: 320
- Bishop: 330
- Rook: 500
- Queen: 900
- King: 20000

**Estimated Time:** 6-8 hours

### 5.2 Search Algorithm (Minimax)

**Objective:** Implement basic move search

**Implementation:**

1. Create `SearchEngine` class
   - `FindBestMove(Board board, PieceColour colour, int depth)`

2. Minimax algorithm:
   - Recursive depth-first search
   - Maximize own position
   - Minimize opponent position

3. Alpha-beta pruning optimization

**Estimated Time:** 6-8 hours

### 5.3 Move Ordering

**Objective:** Optimize search efficiency

**Implementation:**

1. Move ordering heuristics:
   - Captures (MVV-LVA: Most Valuable Victim - Least Valuable Attacker)
   - Checks
   - Promotions
   - Castling
   - Killer moves
   - History heuristic

2. Order moves before searching
3. Improves alpha-beta pruning effectiveness

**Estimated Time:** 4-6 hours

### 5.4 Quiescence Search

**Objective:** Avoid horizon effect

**Implementation:**

1. Extend search at leaf nodes
2. Only search "quiet" positions (no captures/checks available)
3. Prevents evaluation of unstable positions

**Estimated Time:** 4-6 hours

### 5.5 Transposition Table

**Objective:** Cache position evaluations

**Implementation:**

1. Zobrist hashing for position identification
2. Store evaluated positions with:
   - Hash key
   - Depth searched
   - Evaluation score
   - Best move found
   - Node type (exact, lower bound, upper bound)

3. Check cache before searching

**Estimated Time:** 6-8 hours

---

## Phase 6: Advanced Engine Features (Priority: Low)

### 6.1 Opening Book

**Objective:** Use precomputed opening moves

**Implementation:**

1. Opening book format (ECO codes)
2. Book lookup before search
3. Probabilistic selection from book

**Estimated Time:** 4-6 hours

### 6.2 Endgame Tablebases

**Objective:** Perfect play in endgames

**Implementation:**

1. Integration with Syzygy tablebases
2. Probe tablebases for positions with ≤7 pieces
3. Return perfect move or evaluation

**Estimated Time:** 6-8 hours

### 6.3 Time Management

**Objective:** Allocate search time effectively

**Implementation:**

1. Time control support:
   - Classical (e.g., 90 min + 30s increment)
   - Rapid (e.g., 15 min + 10s)
   - Bullet (e.g., 1 min)

2. Time allocation algorithm
3. Iterative deepening with time checks

**Estimated Time:** 4-6 hours

### 6.4 UCI Protocol

**Objective:** Standard chess engine interface

**Implementation:**

1. Implement UCI commands:
   - `uci`, `isready`
   - `ucinewgame`
   - `position [fen | startpos] moves ...`
   - `go [options]`
   - `stop`, `quit`

2. Engine info output:
   - `info` (depth, score, nodes, pv)
   - `bestmove`

3. Allow use with chess GUIs (Arena, ChessBase)

**Estimated Time:** 6-8 hours

---

## Phase 7: Performance Optimization (Priority: Low)

### 7.1 Move Generation Optimization

**Objective:** Faster legal move generation

**Techniques:**
- Bitboards instead of list-based representation
- Magic bitboards for sliding pieces
- Pre-computed attack tables

**Estimated Time:** 12-16 hours (major refactor)

### 7.2 Search Optimization

**Objective:** Search more positions per second

**Techniques:**
- Lazy SMP (parallel search)
- Better move ordering
- Late move reductions
- Null move pruning
- Futility pruning

**Estimated Time:** 8-12 hours

---

## Summary Timeline

| Phase | Description | Priority | Estimated Time |
|-------|-------------|----------|----------------|
| 1 | Bug Fixes | Critical | 1-2 hours |
| 2 | Core Game Rules | High | 21-31 hours |
| 3 | Game State Management | High | 12-18 hours |
| 4 | Testing and Quality | High | 10-15 hours |
| 5 | Basic Chess Engine | Medium | 26-36 hours |
| 6 | Advanced Engine | Low | 20-28 hours |
| 7 | Performance | Low | 20-28 hours |

**Total:** 110-158 hours (3-4 months part-time, 3-4 weeks full-time)

---

## Recommended Order

1. **Phase 1** - Fix bugs immediately
2. **Phase 2.1-2.2** - Check and checkmate (critical for valid chess)
3. **Phase 3.1-3.2** - Turn management and move execution
4. **Phase 4.1** - Complete unit tests for implemented features
5. **Phase 2.3-2.5** - Remaining game rules
6. **Phase 3.3** - Draw conditions
7. **Phase 4.2** - Board validation
8. **Phase 5** - Basic engine (if desired)
9. **Phase 6-7** - Advanced features as needed

---

## Next Steps

1. Create issues/tasks for each phase
2. Set up CI/CD pipeline
3. Establish code review process
4. Begin with Phase 1 bug fixes
5. Iterate through phases based on priorities
