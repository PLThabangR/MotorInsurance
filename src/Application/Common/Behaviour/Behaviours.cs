using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;

namespace Application.Common.Behaviour;

/// <summary>
/// Runs all registered FluentValidation validators for a request.
/// Aggregates failures into a single Application ValidationException
/// so that ExceptionMiddleware returns one consistent 400 response.
/// </summary>

/// <summary>
/// Runs all registered FluentValidation validators for a request.
/// Aggregates failures into a single Application ValidationException
/// so that ExceptionMiddleware returns one consistent 400 response.
/// </summary>
public sealed class ValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken ct)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, ct)));
        var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

        if (failures.Count > 0)
        {
            var message = string.Join("; ", failures.Select(f => f.ErrorMessage));
            throw new ApplicationException(message);
        }

        return await next();
    }
}
