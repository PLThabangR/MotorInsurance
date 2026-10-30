using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;


public record AuthUserInfo(string userId, string email,string firstName, string lastName);

//this is a service interface for managing user accounts in an identity system. It defines a method for creating a new user account with the specified email, password, first name, and last name.
//The method returns an IdentityResult indicating the success or failure of the account creation operation.
public interface IIdentityAccountService
{
    Task<Result<string>> CreateAsync(string email, string password, string firstName, string lastName,CancellationToken ct=default);
    Task<bool> CheckPasswaordAsyn(string email, string password,CancellationToken ct =default);
    Task<string?> GetUserIdByEmailAsync(string email, CancellationToken ct = default);
    Task<AuthUserInfo?> GetAuthUserByIdAsync(string userId, CancellationToken ct = default);
    Task <Result> UpadateProfileAsync(string userId, string firstName, string lastName, CancellationToken ct = default);

    Task<IReadOnlyList<string>> GetUserRolesAsync(string userId, CancellationToken ct = default);

    Task<Result> EnsureRoleAsync(string roleName);
    Task<Result> AddUserToRoleAsync(string userId, string roleName);

    Task DeleteUserAsync(string userId, CancellationToken ct = default);
    
}
