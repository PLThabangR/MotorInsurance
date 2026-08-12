

using MediatR;
using MotorInsurance.Domain.Entities;
using Mapster;
using MotorInsurance.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MotorInsurance.API.Features.Users.Commands;


public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private ApplicationDbContext _context;
    //Constructor 
    public CreateUserHandler(ApplicationDbContext context)
    {   
        _context = context;
    }
    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Business RUles ##1
        //AskNoTracking no caching
        var emailExists = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        // If email exist throw exception
        if (emailExists is not null)
        {
            throw new ArgumentException($"User with email {request.Email} already exists.");
        }

        //create user entity
        var user = new User(request.Email, request.Name);

        //save to database
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


           //Mapster
           var response = user.Adapt<CreateUserResponse>();
            //return Task.FromResult(response);
            return response;
            
    }



}