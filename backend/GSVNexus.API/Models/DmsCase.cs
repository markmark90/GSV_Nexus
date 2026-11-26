using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsCase
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string CaseNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string DocumentTypeCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ProjectCode { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Zaprimljeno";

    [MaxLength(100)]
    public string? CurrentStep { get; set; }

    [MaxLength(200)]
    public string? AssignedRole { get; set; }

    [MaxLength(200)]
    public string? AssignedTo { get; set; }

    public Guid? DocumentId { get; set; }
    public DmsDocument? Document { get; set; }
    public ICollection<DmsWorkflowInstance> WorkflowInstances { get; set; } = [];

    public DateTime? DueAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
