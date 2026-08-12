using MediatR;
using MotorInsurance.Domain.Entities;
using Mapster;
using MotorInsurance.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MotorInsurance.API.Features.Users.Queries;

/// <summary>
/// Handles the GetUserQuery
/// Contains the actual logic to retrieve a user
/// </summary>
public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResponse>
{
    // properties
    private ApplicationDbContext _context;
    //constructor
    public GetUserHandler(ApplicationDbContext context)
    {    
        _context = context;
        
    }
        //Use async/await to communicate with the database
    public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // LOGIC: Find the user in our "database"
        var user =await  _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (user is null)
        {
            throw new KeyNotFoundException($"User with id {request.Id} not found.");
        }
        // Mapster does it automatically using convention
        //Map to response
        var response = user.Adapt<GetUserResponse>();


        // MediatR expects handlers to return Task<T>
        return response;
    }
}

