using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsWorkflowTask
{
    public Guid Id { get; set; }
    public Guid WorkflowInstanceId { get; set; }
    public DmsWorkflowInstance WorkflowInstance { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string StepKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string AssigneeRole { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? AssigneePerson { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Čeka";

    [MaxLength(50)]
    public string? Decision { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }

    public DateTime? DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
