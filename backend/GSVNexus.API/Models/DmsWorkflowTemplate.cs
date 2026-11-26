using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsWorkflowTemplate
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string DocumentTypeCode { get; set; } = string.Empty;

    [Required]
    public string StepsJson { get; set; } = "[]";

    [MaxLength(1000)]
    public string? RuleDescription { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
