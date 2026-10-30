using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

/// <summary>
/// Hashed refresh token. Plaintext never stored.
/// Rotation: on use, old token revoked, new issued.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!;
    public string TokenHash { get; set; } = default!; // SHA-256 hex
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAtUtc { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    public bool IsActive => RevokedAtUtc == null && ExpiresAtUtc > DateTime.UtcNow;
}