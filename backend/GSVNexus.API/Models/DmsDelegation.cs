using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsDelegation
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string DelegatingPerson { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string DelegatePerson { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ProjectCode { get; set; }

    [MaxLength(200)]
    public string? RoleName { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }
}
