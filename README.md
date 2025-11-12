# Chess Library

A comprehensive chess library implemented in C# .NET 6.0, designed for creating chess applications and engines.

## Project Structure

```
Chess/
├── Chess/                  # Core chess library
├── Chess.Tests/           # Unit tests
└── ChessConsole/          # Console application demo
```

## Features

### Implemented

#### Core Components

- **Position System** - Board coordinate representation (A-H, 1-8)
- **Piece System** - All 6 chess pieces with complete movement logic:
  - Pawn (forward movement, diagonal captures, double-move from start)
  - Knight (L-shaped movement)
  - Bishop (diagonal movement)
  - Rook (horizontal/vertical movement)
  - Queen (combined rook + bishop)
  - King (one square in any direction)

#### Movement Engine

- **Path-based Movement Calculation** - Pieces generate theoretical paths, validated against board state
- **Collision Detection** - Pieces blocked by other pieces
- **Capture Logic** - Valid capture identification
- **Action System** - Extensible action framework:
  - Capture
  - Check
  - Checkmate
  - Castle (Kingside/Queenside)
  - Promotion

#### Algebraic Notation

- **Full Parser** - Comprehensive regex-based notation reader
- **Supported Notation**:
  - Piece moves (e.g., `Nf3`, `Bc4`)
  - Captures (e.g., `Bxf7`, `exd5`)
  - Castling (`O-O`, `O-O-O`)
  - Check (`+`)
  - Checkmate (`#`)
  - Pawn promotion (e.g., `e8=Q`)
  - Full game notation with turn numbers

#### Board Management

- Standard starting position initialization
- Custom board setup
- Piece location queries
- Tile color determination

### Known Issues

The following bugs have been identified and need fixing:

1. **Pawn.cs:77, 83** - Capture logic uses wrong row variable (`y2` instead of `y`)
2. **Queen.cs:30** - Right-down diagonal uses subtraction instead of addition
3. **Board.cs:120** - King and Queen positions are swapped in initial setup

### Not Yet Implemented

Critical features needed for a complete chess engine:

#### Game Rules
- Check detection
- Checkmate detection
- Stalemate detection
- En passant capture
- Castling validation (through check, after pieces moved)
- Pawn promotion piece selection

#### Game State Management
- Turn tracking (whose turn)
- Move history
- Game over conditions
- Move validation before application

#### Advanced Features
- Fifty-move rule
- Threefold repetition
- Position evaluation (for engine)
- Move generation optimization
- Opening book
- Endgame tablebases

## Architecture

### Design Patterns

- **Domain-Driven Design** - Clean separation of concerns
- **Template Method Pattern** - `Piece` class defines movement algorithm
- **Builder Pattern** - Test builders for readable test setup
- **Strategy Pattern** - `IAction` interface with multiple implementations
- **Value Objects** - Immutable position and movement types

### Key Design Decisions

- **Path-based Movement** - Pieces define theoretical movement paths validated against board state
- **Action Composition** - Movements carry associated game effects
- **Notation Separation** - Parsing decoupled from move execution
- **Nullable Reference Types** - Enabled for null safety

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later

### Building

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Using the Console App

```bash
cd ChessConsole
dotnet run
```

The console application allows you to input chess moves in algebraic notation.

## Usage Examples

### Creating a Board

```csharp
// Standard starting position
var board = new Board();

// Custom position
var pieces = new List<Piece>
{
    new King(PieceColour.White, new Position('e', 1)),
    new Queen(PieceColour.White, new Position('d', 1)),
    // ... more pieces
};
var customBoard = new Board(pieces);
```

### Moving Pieces

```csharp
var piece = board.FindPiece(PieceType.Knight, PieceColour.White, new Position('b', 1));
var possibleMoves = piece.PossibleMoves(board);

// Check if a specific move is valid
bool canMove = piece.CanMoveTo(new Position('c', 3), board);
```

### Parsing Notation

```csharp
var notation = "1. e4 e5 2. Nf3 Nc6";
var turns = AlgebraicNotationReader.ReadGame(notation);

foreach (var turn in turns)
{
    Console.WriteLine($"Turn {turn.TurnNumber}");
    Console.WriteLine($"White: {turn.WhitePlayerTurn}");
    Console.WriteLine($"Black: {turn.BlackPlayerTurn}");
}
```

## Testing

The project uses xUnit and FluentAssertions. Test builders provide fluent APIs for creating test scenarios:

```csharp
var board = new ChessBoardBuilder()
    .WithLayout(@"
        ........
        ........
        ........
        ........
        ........
        ........
        ........
        K.......
    ")
    .Build();
```

### Test Coverage

Currently tested:
- Bishop movement (6 tests)
- King movement (9 tests)
- Notation parsing (1 comprehensive test)

Needs tests:
- Pawn, Knight, Rook, Queen movements
- Board state management
- Turn application
- Check/checkmate detection

## Contributing

When contributing, please:

1. Write tests for new features
2. Follow existing code style
3. Update documentation
4. Fix known bugs before adding new features

## Roadmap

See [IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md) for detailed implementation roadmap.

## License

[Specify your license here]

## Authors

[Your name/team]
