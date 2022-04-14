using System.Runtime.Serialization;

namespace Chess.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException()
    {
    }

    protected DomainException(string messageTemplate)
        : base(messageTemplate)
    {
    }

    protected DomainException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}