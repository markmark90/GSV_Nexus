using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsNotification
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Recipient { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = string.Empty;

    public Guid? CaseId { get; set; }
    public Guid? TaskId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? DueAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
