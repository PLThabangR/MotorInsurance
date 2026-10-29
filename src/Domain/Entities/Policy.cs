using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

/// <summary>
/// Insurance policy. Owns vehicles, drivers, claims.
/// Status uses an enum string in DB for readability.
/// </summary>
public class Policy
{
    public Guid Id { get; set; }

    public string PolicyNumber { get; set; } = default!;//Foreign key to the policy holder, which is a user in the system
    public Guid PolicyHolderId { get; set; }
    public PolicyStatus Status { get; set; } = PolicyStatus.Draft;
    public DateTime EffectiveFromUtc { get; set; }
    public DateTime EffectiveToUtc { get; set; }
    public decimal Premium { get; set; }
    public string Currency { get; set; } = "GBP";
    public string CoverageNotes { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public PolicyHolder PolicyHolder { get; set; } = default!;

    // Navigation properties for related entities
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<Driver> Drivers { get; set; } = new List<Driver>();
    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}

//Enums used to represent the status of a policy in the system.
//The values are stored as strings in the database for better readability and maintainability.

public enum PolicyStatus { Draft = 0, Active = 1, Expired = 2, Cancelled = 3 }
