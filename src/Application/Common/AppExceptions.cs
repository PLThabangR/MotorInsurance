using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;
/// <summary>
/// Raised when input fails business or shape validation.
/// Mapped to HTTP 400 by ExceptionMiddleware.
/// </summary>
public sealed class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

/// <summary>Mapped to HTTP 404.</summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

/// <summary>Mapped to HTTP 401.</summary>
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}

/// <summary>Mapped to HTTP 403.</summary>
public sealed class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}

/// <summary>Mapped to HTTP 409.</summary>
public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
