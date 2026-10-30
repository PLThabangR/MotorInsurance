using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.dtos;

/// <summary>
/// Contains the authentication data returned after a successful
/// registration, login, or refresh-token operation.
/// </summary>
public sealed record AuthResponseDto(
    string UserId,
    string Email,
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken);
