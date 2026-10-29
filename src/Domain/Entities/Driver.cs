using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Driver
{
    public Guid Id { get; set; }
    public Guid PolicyId { get; set; }
    public string FullName { get; set; } = default!;
    public DateTime LicenceIssuedUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Policy Policy { get; set; } = default!;
}
