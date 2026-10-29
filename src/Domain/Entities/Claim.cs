using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Claim
{
    public Guid Id { get; set; }
    public Guid PolicyId { get; set; }
    public string ClaimNumber { get; set; } = default!;
    public DateTime IncidentUtc { get; set; }
    public string Description { get; set; } = default!;
    public decimal EstimatedAmount { get; set; }
    public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Policy Policy { get; set; } = default!;
}

public enum ClaimStatus { Submitted = 0, UnderReview = 1, Approved = 2, Rejected = 3, Paid = 4 }
