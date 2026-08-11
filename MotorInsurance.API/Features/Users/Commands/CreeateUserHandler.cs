

using MediatR;
using MotorInsurance.Domain.Entities;
using Mapster;

namespace MotorInsurance.API.Features.Users.Commands;


public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    //in memory
    private static readonly List<User> _users = new ();
    public Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Business RUles ##1
        if(_users.Any(x => x.Email == request.Email)){
            throw new ArgumentException($"User with email {request.Email} already exists.");
            }// Business RUles ##1

            //business rules ##2
            var user = new User(request.Email, request.Name);

            _users.Add(user);

           //Mapster
           var response = user.Adapt<CreateUserResponse>();
            //return Task.FromResult(response);
            return Task.FromResult(response);
            
    }



}