using System.ComponentModel.DataAnnotations;

namespace GSVNexus.API.Models;

public sealed class DmsCategory
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public Guid? ParentId { get; set; }
    public DmsCategory? Parent { get; set; }
    public ICollection<DmsCategory> Children { get; set; } = [];
    public ICollection<DmsDocument> Documents { get; set; } = [];

    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
