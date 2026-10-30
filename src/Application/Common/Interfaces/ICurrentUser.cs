using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;
/// <summary>
/// Read-only view of the authenticated caller, resolved from the JWT.
/// Handlers use this instead of trusting client-supplied IDs.
/// </summary>
public interface ICurrentUser
{
    string UserId { get; }
    string Email { get; }
    bool IsAdmin { get; }
}
