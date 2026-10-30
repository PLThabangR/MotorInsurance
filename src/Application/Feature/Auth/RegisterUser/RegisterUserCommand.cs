//Using MediatR and record types to define a command for registering a user and the result of the authentication process. The RegisterUserCommand takes in user details such as email, password, first name, and last name, while the AuthResult contains information about the authenticated user, including their user ID, email, token, expiration time, and refresh token.
using Application.Feature.Auth.dtos;
using FluentValidation;
using MediatR;

namespace Application.Feature.Auth.RegisterUser;

//RegisterUserCommand record type that implements the IRequest interface from MediatR, which allows it to be handled by a corresponding handler. The command takes in user details such as email, password, first name, and last name.
public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName

    ) : IRequest<AuthResponseDto>;



//Add RegisterUserCommand to the pipeline validation
public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    //Validator for the RegisterUserCommand, ensuring that the email is not empty and is a valid email address,
    //the password meets certain complexity requirements
    //the first
    //last names are not empty and do not exceed 100 characters in length.
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128)
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain a digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a non-alphanumeric character.");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}
