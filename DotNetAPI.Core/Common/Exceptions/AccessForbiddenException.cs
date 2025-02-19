using System.Runtime.Serialization;

namespace DotNetAPI.Core.Common.Exceptions;

public class AccessForbiddenException : Exception
{
    public AccessForbiddenException()
    {

    }

    public AccessForbiddenException(string message) : base(message) { }

    public AccessForbiddenException(string message, Exception innerException) : base(message, innerException) { }

    protected AccessForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
