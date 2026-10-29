using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

/// <summary>
/// Represents the insurance customer. Owns policies.
/// Kept deliberately simple — no DDD aggregate semantics.
/// </summary>
public class PolicyHolder
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!; // FK to ApplicationUser
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    //We returning the collection of policies here for convenience,
    //but in a real system we would likely have a separate repository for policies
    //and not return them directly from the policy holder.
    public ICollection<Policy> Policies { get; set; } = new List<Policy>();
}
