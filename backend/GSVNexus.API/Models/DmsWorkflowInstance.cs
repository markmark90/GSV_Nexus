using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsWorkflowInstance
{
    public Guid Id { get; set; }
    public Guid CaseId { get; set; }
    public DmsCase Case { get; set; } = null!;

    public Guid TemplateId { get; set; }
    public DmsWorkflowTemplate Template { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Aktivan";

    [MaxLength(100)]
    public string? CurrentStep { get; set; }
    public Guid? DocumentVersionId { get; set; }
    public ICollection<DmsWorkflowTask> Tasks { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
