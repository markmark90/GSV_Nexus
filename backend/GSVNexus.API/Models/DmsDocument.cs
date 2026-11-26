using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsDocument
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string StoragePath { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? MimeType { get; set; }

    [MaxLength(100)]
    public string? PartnerOrProject { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    public Guid CategoryId { get; set; }
    public DmsCategory Category { get; set; } = null!;
    public ICollection<DmsDocumentVersion> Versions { get; set; } = [];

    public long SizeBytes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}
