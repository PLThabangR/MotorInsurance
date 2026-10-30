using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

/// <summary>
/// Application-layer DB abstraction. Keeps Application free of EF Core coupling
/// at the call-site level while still using IQueryable.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<PolicyHolder> PolicyHolders { get; }
    DbSet<Policy> Policies { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<Claim> Claims { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}
