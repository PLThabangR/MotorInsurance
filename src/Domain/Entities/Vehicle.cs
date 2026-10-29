using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

/// <summary>
/// Vehicle attached to a policy. Soft-removal via CoverageEndUtc preserves history.
/// </summary>
public class Vehicle
{
    public Guid Id { get; set; }
    public Guid PolicyId { get; set; }
    public string RegistrationNumber { get; set; } = default!;
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int Year { get; set; }
    public string Vin { get; set; } = default!;
    public DateTime CoverageStartUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CoverageEndUtc { get; set; } // null = active coverage
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Policy Policy { get; set; } = default!;
    public ICollection<TelematicsRecord> TelematicsRecords { get; set; } = new List<TelematicsRecord>();
}
