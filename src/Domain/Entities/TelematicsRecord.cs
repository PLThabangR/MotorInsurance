using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class TelematicsRecord
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime RecordedAtUtc { get; set; }
    public double MilesDriven { get; set; }
    public int AverageSpeedMph { get; set; }
    public Vehicle Vehicle { get; set; } = default!;
}
