# Next Features for Chess Library

This document outlines potential features and improvements for the chess library, organized by priority and complexity.

## Current Implementation Status

### ✅ Completed Features
- **Basic piece movement**: All pieces (Pawn, Rook, Knight, Bishop, Queen, King) with correct movement patterns
- **Capture mechanics**: Standard piece captures
- **Special moves**:
  - Castling (kingside and queenside)
  - En passant
  - Pawn promotion (to Queen only, hardcoded)
- **Movement tracking**: `HasMoved` flag for castling validation
- **Last move tracking**: `Board.LastMove` for en passant validation
- **Algebraic notation parsing**: Reading chess notation (partial implementation)

---

## High Priority Features

### 1. Check Detection (Phase 2.1) 🔴 **HIGHEST PRIORITY**
**Description**: Detect when a king is in check (under attack).

**Why Important**:
- Core chess rule - players must respond to check
- Required for checkmate detection
- Prevents illegal moves that leave king in check

**Implementation Steps**:
1. Add `IsKingInCheck(Board board, PieceColour kingColour)` method to Board class
2. Find king of given color
3. Check if any enemy piece can move to king's position
4. Update `PossibleMoves()` to filter out moves that leave own king in check

**Files to Modify**:
- `Chess/Board.cs` - Add check detection logic
- `Chess/Piece.cs` - Filter moves that result in check
- `Chess/Actions/Check.cs` - Already exists, integrate usage

**Estimated Complexity**: Medium

**Tests Needed**:
- King in check from each piece type
- Multiple pieces giving check
- Moving into check (should be illegal)
- Moving piece that exposes king to check (pinned pieces)

---

### 2. Checkmate Detection (Phase 2.2)
**Description**: Detect when a player is in checkmate (king in check with no legal moves).

**Dependencies**: Check detection must be complete

**Implementation Steps**:
1. Verify king is in check
2. Verify no legal moves available that remove check:
   - King cannot move to safe square
   - No piece can block the attack
   - No piece can capture the attacking piece
3. Add `IsCheckmate(Board board, PieceColour colour)` method

**Files to Modify**:
- `Chess/Board.cs` - Add checkmate detection
- `Chess/Actions/Checkmate.cs` - Already exists, integrate usage

**Estimated Complexity**: High

**Tests Needed**:
- Back rank mate
- Smothered mate
- Queen checkmate
- Two rook checkmate
- False checkmate (king has escape square)

---

### 3. Stalemate Detection
**Description**: Detect when a player has no legal moves but is not in check.

**Dependencies**: Check detection

**Implementation Steps**:
1. Verify king is NOT in check
2. Verify no legal moves available for any piece
3. Return draw result

**Files to Modify**:
- `Chess/Board.cs` - Add stalemate detection

**Estimated Complexity**: Medium

**Tests Needed**:
- King and pawn vs king stalemate
- Queen stalemate patterns
- Insufficient material positions

---

### 4. Pawn Promotion Choice
**Description**: Allow player to choose promotion piece (Queen, Rook, Bishop, Knight).

**Current State**: Hardcoded to promote to Queen only (Pawn.cs:53)

**Implementation Steps**:
1. Add parameter to `GetMovement()` or use notation to determine piece
2. Update `Promotion` action to accept chosen piece type
3. Handle underpromotion in algebraic notation

**Files to Modify**:
- `Chess/Pieces/Pawn.cs:53` - Remove hardcoded Queen promotion
- `Chess/Actions/Promotion.cs` - Already accepts PieceType
- `Chess/Notation/AlgebraicNotationReader.cs` - Parse promotion choice

**Estimated Complexity**: Low

**Tests Needed**:
- Promote to each piece type
- Underpromotion scenarios
- Notation parsing for promotion

---

## Medium Priority Features

### 5. Move Validation During Turn Application
**Description**: Actually apply moves and validate them when `Board.ApplyTurn()` is called.

**Current State**: `Board.ApplyTurn()` has TODO comments (Board.cs:24-38)

**Implementation Steps**:
1. Parse `NotedTurn` to get moves
2. Validate moves are legal
3. Update piece positions
4. Update `HasMoved` flags
5. Update `LastMove`
6. Handle captures (remove pieces from board)
7. Handle special moves (castling, en passant, promotion)

**Files to Modify**:
- `Chess/Board.cs:24-38` - Implement ApplyTurn logic

**Estimated Complexity**: High

**Tests Needed**:
- Apply legal moves
- Reject illegal moves
- Handle captures correctly
- Handle special moves correctly
- Update board state properly

---

### 6. Threefold Repetition Draw
**Description**: Detect when the same position occurs three times (draw by repetition).

**Implementation Steps**:
1. Track position history (board state + turn)
2. Create position hash/fingerprint
3. Count position occurrences
4. Declare draw when position repeats 3 times

**Files to Modify**:
- `Chess/Board.cs` - Add position tracking and comparison

**Estimated Complexity**: Medium

**Tests Needed**:
- Simple repetition patterns
- Repetition with different move orders
- False repetitions (different castling rights)

---

### 7. Fifty-Move Rule Draw
**Description**: Declare draw if 50 moves pass without pawn move or capture.

**Implementation Steps**:
1. Track halfmove counter
2. Reset on pawn move or capture
3. Declare draw at 50 halfmoves (100 plies)

**Files to Modify**:
- `Chess/Board.cs` - Add halfmove counter

**Estimated Complexity**: Low

**Tests Needed**:
- Counter increments correctly
- Counter resets on pawn move
- Counter resets on capture
- Draw declared at 50 moves

---

### 8. Insufficient Material Draw
**Description**: Declare draw when neither player has sufficient material to checkmate.

**Scenarios**:
- King vs King
- King + Bishop vs King
- King + Knight vs King
- King + Bishop vs King + Bishop (same color)

**Implementation Steps**:
1. Count remaining pieces and types
2. Check against insufficient material patterns
3. Declare draw if applicable

**Files to Modify**:
- `Chess/Board.cs` - Add material evaluation

**Estimated Complexity**: Low

**Tests Needed**:
- Each insufficient material scenario
- Sufficient material scenarios (should not draw)

---

### 9. Complete Algebraic Notation Support
**Description**: Full support for reading and writing chess notation.

**Current State**: Partial implementation in `AlgebraicNotationReader.cs`

**Missing Features**:
- Write moves to notation
- Handle ambiguous moves correctly
- Annotations (+, #, !, ?, !!, ??)
- Variations and comments

**Files to Modify**:
- `Chess/Notation/AlgebraicNotationReader.cs` - Complete parsing
- `Chess/Notation/AlgebraicNotationWriter.cs` - New file for writing notation

**Estimated Complexity**: Medium

**Tests Needed**:
- Read/write all move types
- Ambiguous move notation
- Special moves notation
- Round-trip (write then read)

---

## Low Priority / Enhancement Features

### 10. FEN (Forsyth-Edwards Notation) Support
**Description**: Import/export board positions using FEN strings.

**Use Cases**:
- Load specific positions for testing
- Save/load game states
- Share positions

**Implementation Steps**:
1. Create FEN parser
2. Create FEN writer
3. Handle all FEN components (position, turn, castling, en passant, halfmove, fullmove)

**Files to Create**:
- `Chess/Notation/FenParser.cs`
- `Chess/Notation/FenWriter.cs`

**Estimated Complexity**: Medium

---

### 11. PGN (Portable Game Notation) Support
**Description**: Import/export complete games with metadata.

**Features**:
- Game metadata (players, date, result, etc.)
- Move history
- Variations and annotations
- Comments

**Dependencies**: Complete algebraic notation support

**Files to Create**:
- `Chess/Notation/PgnParser.cs`
- `Chess/Notation/PgnWriter.cs`

**Estimated Complexity**: High

---

### 12. Move History and Undo
**Description**: Track complete move history and allow undoing moves.

**Implementation Steps**:
1. Store history of board states or moves
2. Implement undo operation
3. Handle special move state (castling rights, en passant)

**Files to Modify**:
- `Chess/Board.cs` - Add history tracking
- Add `Board.UndoMove()` method

**Estimated Complexity**: Medium

---

### 13. Time Controls
**Description**: Support chess clocks and time controls.

**Features**:
- Time per player
- Time increments
- Time pressure detection
- Flag detection (time runs out)

**Files to Create**:
- `Chess/TimeControl.cs`
- `Chess/ChessClock.cs`

**Estimated Complexity**: Low

---

### 14. Move Generation Optimization
**Description**: Optimize move generation for performance.

**Techniques**:
- Bitboards for piece positions
- Magic bitboards for sliding pieces
- Incremental zobrist hashing
- Move ordering

**Estimated Complexity**: Very High

**When Needed**: Only if performance becomes an issue

---

### 15. Opening Book
**Description**: Database of common chess openings.

**Features**:
- Opening name recognition
- Opening move suggestions
- Transposition detection

**Files to Create**:
- `Chess/Openings/OpeningBook.cs`
- Opening database file

**Estimated Complexity**: Medium

---

### 16. Endgame Tablebase Support
**Description**: Perfect play for positions with few pieces.

**Features**:
- Load tablebase files
- Query tablebase for best moves
- Mate distance calculation

**Dependencies**: External tablebase files (Syzygy, Nalimov)

**Estimated Complexity**: High

---

### 17. Chess960 (Fischer Random) Support
**Description**: Support for Chess960 variant with random starting positions.

**Implementation Steps**:
1. Generate valid starting positions
2. Adapt castling rules for Chess960
3. Update notation to handle Chess960

**Estimated Complexity**: Medium

---

## Recommended Implementation Order

1. **Check Detection** - Core game rule, highest priority
2. **Checkmate Detection** - Natural follow-up to check detection
3. **Stalemate Detection** - Complete draw conditions
4. **Pawn Promotion Choice** - Fix existing TODO
5. **Move Validation During Turn Application** - Complete game logic
6. **Fifty-Move Rule** - Simple draw rule
7. **Insufficient Material** - Simple draw rule
8. **Threefold Repetition** - Complete all draw conditions
9. **FEN Support** - Useful for testing
10. **Complete Algebraic Notation** - Polish existing feature
11. **PGN Support** - Complete game import/export
12. **Move History/Undo** - Quality of life feature
13. **Time Controls** - For actual gameplay
14. **Other features** - As needed

---

## Architecture Improvements

### Refactoring Opportunities
1. **Pin Detection**: Track pinned pieces (required for check validation)
2. **Attack Maps**: Pre-calculate squares attacked by each side
3. **Game State Object**: Separate game state from board representation
4. **Move Object**: More structured move representation beyond Movement class
5. **Validation Layer**: Separate legal move generation from theoretical moves

### Code Quality
1. Complete TODO items in codebase:
   - `Piece.cs:104-106` - Castling sniper rules, check validation
   - `Pawn.cs:17` - Edge of board promotion check
   - `Board.cs:24-38` - ApplyTurn implementation
2. Add XML documentation to public APIs
3. Add more integration tests for complex scenarios
4. Performance benchmarks

---

## Testing Strategy

For each new feature:
1. **Unit tests**: Test individual components in isolation
2. **Integration tests**: Test feature working with existing code
3. **Edge case tests**: Boundary conditions and unusual scenarios
4. **Regression tests**: Ensure existing features still work

### Test Coverage Goals
- Aim for 90%+ code coverage
- Every chess rule should have explicit tests
- Every special case should have a test

---

## Documentation

### User Documentation
- API documentation (XML docs)
- Usage examples
- Tutorial for common scenarios

### Developer Documentation
- Architecture overview (this document is a start)
- Contributing guidelines
- Code style guide

---

*Last Updated: 2025-11-12*
*Current Test Status: 66/66 tests passing (100%)*
