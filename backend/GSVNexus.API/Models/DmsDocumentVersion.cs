using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsDocumentVersion
{
    public Guid Id { get; set; }

    [MaxLength(500)]
    public string StoragePath { get; set; } = string.Empty;

    public int VersionNumber { get; set; }
    public long SizeBytes { get; set; }

    [MaxLength(100)]
    public string? MimeType { get; set; }

    [MaxLength(1000)]
    public string? ChangeNote { get; set; }

    public Guid DocumentId { get; set; }
    public DmsDocument Document { get; set; } = null!;

    [MaxLength(200)]
    public string CreatedBy { get; set; } = "System";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
