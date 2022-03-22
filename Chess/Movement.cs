using Chess.Actions;

namespace Chess;

public sealed class Movement
{
    public Position Origin { get; }
    
    public Position Destination { get; }

    public IEnumerable<IAction> Actions { get; }

    public Movement(Position origin, Position destination, params IAction[] actions)
    {
        Origin = origin;
        Destination = destination;
        Actions = actions ?? Enumerable.Empty<IAction>();
    }
    
    public Movement(Position origin, Position destination, IEnumerable<IAction> actions)
    {
        Origin = origin;
        Destination = destination;
        Actions = actions;
    }

    public bool IsCapture => Actions.OfType<Capture>().Any();
}