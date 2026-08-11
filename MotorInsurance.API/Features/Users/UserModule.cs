

using Carter;
using MediatR;
using MotorInsurance.API.Features.Users.Commands;
using MotorInsurance.API.Features.Users.Queries;

namespace MotorInsurance.API.Features.Users
{
    public class UserModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //Group all user endpoints share "/api/users" prefix
            var group = app.MapGroup("/api/users")
            .WithTags("Users")//Swagger in swagger
            .WithOpenApi(); //Automatic Swagger documentation

            
        group.MapGet("/{id:guid}",async (Guid id, ISender sender) =>
        {
            //Create query
            var query = new GetUserQuery(id);
            //MediatR sends the query and get the handler automatically
            var response = await sender.Send(query);
            return Results.Ok(response);


        }).WithName("GetUser")
        .WithSummary("Get a user by ID")
        .WithDescription("Retrieves a users detail using a unique ID")
        .Produces<GetUserResponse>(200) //Return type
        .ProducesProblem(StatusCodes.Status404NotFound); //Document error


        //POST /api/users
        //
        group.MapPost("/", async (CreateUserCommand command, ISender sender) =>
        {    
            //MediatR sends the command and get the handler automatically
            var response = await sender.Send(command);

                //Response
            return Results.Created($"/api/users/{response.Id}", response);
            
        }).WithName("CreateUser")
        .WithSummary("Create a new user")
        .WithDescription("Creates a new user and returns the newly created user detail")
        .Produces<CreateUserResponse>(201) //Return type
        .ProducesProblem(StatusCodes.Status400BadRequest) //Document validation error
        .ProducesProblem(StatusCodes.Status409Conflict); //Document conflict error




        

        }//
    } //Ends of UserModule
}//NameSpace