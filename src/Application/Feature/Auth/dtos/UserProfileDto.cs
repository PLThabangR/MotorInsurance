using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.dtos;



/// <summary>
/// Represents the profile information exposed to the authenticated user.
/// Does not include passwords, token hashes, or other authentication secrets.
/// </summary>
public sealed record UserProfileDto(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    IReadOnlyCollection<string> Roles);

