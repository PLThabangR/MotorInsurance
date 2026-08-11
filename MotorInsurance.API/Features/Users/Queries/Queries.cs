
using MediatR;


namespace MotorInsurance.API.Features.Users.Queries;

// Query to retrieve a user by ID
public record GetUserQuery(Guid UserId) : IRequest<GetUserResponse>;


// Response DTO for GetUserByIdQuery
// Simplified version of the User entity to be returned in the API response, without exposing sensitive information or internal state.
public record GetUserResponse(
    Guid Id,
    string Email,
    string Name,
    DateTime CreatedAt
);