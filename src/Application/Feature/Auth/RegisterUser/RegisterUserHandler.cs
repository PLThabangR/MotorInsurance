using Application.Common.Interfaces;
using Application.Feature.Auth.dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Feature.Auth.RegisterUser;
/// <summary>
/// Registers a new customer.
///
/// Orchestration:
///   1. Create Identity user (external identity store).
///   2. Ensure "Customer" role exists and assign it.
///   3. Create PolicyHolder + RefreshToken domain rows.
///   4. Persist domain rows in a single SaveChangesAsync.
///
/// Identity's UserManager does not enlist in the domain DbContext's unit of
/// work, so steps 1–2 commit against the Identity store independently. If
/// step 4 fails, we compensate by deleting the just-created Identity user so
/// the system does not retain an account with no corresponding profile.
/// </summary>
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    private const string CustomerRole = "Customer";

    //services
    private readonly ITokenService tokenService;
    private readonly IApplicationDbContext dbContext;
    private IIdentityAccountService accountService;


    //Constructor for the RegisterUserHandler class, which takes in dependencies such as UserManager, ITokenService, IApplicationDbContext, and IIdentityAccountService. These dependencies are used to handle user registration and authentication.
    public RegisterUserHandler(IIdentityAccountService accounts, ITokenService tokenService, IApplicationDbContext dbContext)
    {   this.accountService = accounts;
        this.tokenService = tokenService;
        this.dbContext = dbContext;
       
    }

    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // ---- Step 1: create the Identity user -----------------------------
        // CreateUserAsync returns Result<string>; it may fail with concrete
        // Identity errors (duplicate email, password policy, etc.). Translate
        // to a ValidationException so the API's ExceptionMiddleware returns 400.

        var created = await accountService.CreateAsync(request.Email, request.Password,request.FirstName,request.LastName);

        //Check if user created successfully and return a user ID
        //if successfully return the userID
        var userId = created.EnsureSucceeded("Registration failed");

        // ---- Step 2: ensure & assign the Customer role --------------------
        // Both operations report Result; we surface any failure rather than
        // silently continuing with an incorrectly authorised account.
        var roleEnsured = await accountService.EnsureRoleAsync(CustomerRole);
        roleEnsured.EnsureSucceeded("role setup failed");

        var roleAdded = await accountService.AddUserToRoleAsync(userId, CustomerRole);
        roleAdded.EnsureSucceeded("Role assignment failed");

        // ---- Step 3: build domain rows ---------------------------------
        var accessToken = tokenService.GenerateAccessToken(userId, request.Email, new[] { CustomerRole });

        var refreshPlainText = tokenService.CreateRefreshTokenPlainText();

        //Create new policyholder
        var policyHolder = new PolicyHolder
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        ///create refresh token for this user
        ///
        var refreshRecord = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenService.HashRefreshToken(refreshPlainText),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        // ---- Step 4: single SaveChangesAsync for both domain rows --------
        // Both entities are tracked in the same unit of work and committed
        // together. If this fails, compensate the Identity user and rethrow.

        dbContext.PolicyHolders.Add(policyHolder);
        dbContext.RefreshTokens.Add(refreshRecord);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) {
            await accountService.DeleteUserAsync(userId);
        }

        // ---- Step 5: return tokens ---------------------------------------
        // The API layer writes RefreshToken to an HttpOnly cookie and returns
        // only the access token body to the client.
        return new AuthResponseDto(
            userId,
            request.Email,
            accessToken.AccessToken,
            accessToken.ExpiresAtUtc,
            refreshPlainText);

    }
}
