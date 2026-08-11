
using MediatR;

namespace MotorInsurance.API.Features.Users.Commands
{
    
    public record CreateUserCommand(string Email, string Name) : IRequest<CreateUserResponse>;


    public record CreateUserResponse(Guid Id, string Email, string Name, DateTime CreatedAt);

}