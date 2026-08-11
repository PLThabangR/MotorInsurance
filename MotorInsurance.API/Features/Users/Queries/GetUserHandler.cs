using MediatR;
using MotorInsurance.Domain.Entities;
using Mapster;

namespace MotorInsurance.API.Features.Users.Queries;

/// <summary>
/// Handles the GetUserQuery
/// Contains the actual logic to retrieve a user
/// </summary>
public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResponse>
{
    // IN-MEMORY STORE - Why?
    // We're not using a database yet (that's TICKET-002)
    // This static dictionary simulates a database
    // It's shared across all requests (like a real database would be)
    private static readonly Dictionary<Guid, User> _users = new();

    // SEED DATA - Why?
    // We need some test data to work with
    // In production, this would come from a database
    static GetUserHandler()
    {
        var user = new User("john@example.com", "John Doe");
        _users[user.Id] = user;
    }

    public Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // LOGIC: Find the user in our "database"
        if (!_users.TryGetValue(request.UserId, out var user))
        {
            // BUSINESS RULE: User must exist
            // Throw exception if not found
            // Global exception handler will catch this and return 404
            throw new KeyNotFoundException($"User with ID {request.UserId} not found");
        }

        // MAPSTER: Auto-map User entity to GetUserResponse
        // Instead of manually mapping each property:
        // var response = new GetUserResponse(user.Id, user.Email, user.Name, user.CreatedAt);
        // Mapster does it automatically using convention
        var response = user.Adapt<GetUserResponse>();

        // Task.FromResult wraps the response in a Task
        // MediatR expects handlers to return Task<T>
        return Task.FromResult(response);
    }
}