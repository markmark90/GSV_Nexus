using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsProjectRole
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ProjectCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string RoleName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string PersonName { get; set; } = string.Empty;

    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
}
