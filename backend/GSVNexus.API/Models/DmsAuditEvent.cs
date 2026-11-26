using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsAuditEvent
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string EntityType { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Actor { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Comment { get; set; }

    [Required]
    public string PayloadJson { get; set; } = "{}";

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
