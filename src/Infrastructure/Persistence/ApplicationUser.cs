using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//identity dependency to be able to use the IdentityUser class for user management
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;
/// <summary>
/// ASP.NET Core Identity user. Extended with display name only.
/// Role assignments handled by IdentityRole.
/// </summary>
public class ApplicationUser:IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

}
