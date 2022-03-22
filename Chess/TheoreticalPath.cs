using System.Collections;

namespace Chess;

public sealed class TheoreticalPath : IEnumerable<Position>
{
    private readonly IEnumerable<Position> _positions;

    public TheoreticalPath()
    {
        _positions = Enumerable.Empty<Position>();       
    }
    
    public TheoreticalPath(int x, int y)
    {
        _positions = new[]
        {
            new Position(x, y)
        };
    }

    public TheoreticalPath(IEnumerable<Position> positions)
    {
        _positions = positions;
    }

    public IEnumerator<Position> GetEnumerator() => _positions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator TheoreticalPath(Position[] enumerable) => new (enumerable);
    
    public static implicit operator TheoreticalPath(List<Position> enumerable) => new (enumerable);
}