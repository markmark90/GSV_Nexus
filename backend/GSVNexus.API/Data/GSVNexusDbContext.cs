using Microsoft.EntityFrameworkCore;
using GSVNexus.API.Models;

namespace GSVNexus.API.Data;

public sealed class GSVNexusDbContext(DbContextOptions<GSVNexusDbContext> options)
    : DbContext(options)
{
    public DbSet<DmsCategory> DmsCategories => Set<DmsCategory>();
    public DbSet<DmsDocument> DmsDocuments => Set<DmsDocument>();
    public DbSet<DmsDocumentVersion> DmsDocumentVersions => Set<DmsDocumentVersion>();
    public DbSet<DmsCase> DmsCases => Set<DmsCase>();
    public DbSet<DmsWorkflowTemplate> DmsWorkflowTemplates => Set<DmsWorkflowTemplate>();
    public DbSet<DmsWorkflowInstance> DmsWorkflowInstances => Set<DmsWorkflowInstance>();
    public DbSet<DmsWorkflowTask> DmsWorkflowTasks => Set<DmsWorkflowTask>();
    public DbSet<DmsAuditEvent> DmsAuditEvents => Set<DmsAuditEvent>();
    public DbSet<DmsProjectRole> DmsProjectRoles => Set<DmsProjectRole>();
    public DbSet<DmsDelegation> DmsDelegations => Set<DmsDelegation>();
    public DbSet<DmsNotification> DmsNotifications => Set<DmsNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DmsCategory>(entity =>
        {
            entity.ToTable("DmsCategories");
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(category => category.Name);
            entity.HasIndex(category => category.ParentId);

            entity.HasOne(category => category.Parent)
                .WithMany(category => category.Children)
                .HasForeignKey(category => category.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DmsDocument>(entity =>
        {
            entity.ToTable("DmsDocuments");
            entity.HasKey(document => document.Id);
            entity.Property(document => document.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.Property(document => document.Status).HasDefaultValue("Draft");
            entity.HasIndex(document => document.CategoryId);
            entity.HasIndex(document => document.Name);
            entity.HasIndex(document => document.Status);
            entity.HasIndex(document => document.IsDeleted);

            entity.HasOne(document => document.Category)
                .WithMany(category => category.Documents)
                .HasForeignKey(document => document.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<DmsDocumentVersion>(entity =>
        {
            entity.ToTable("DmsDocumentVersions");
            entity.HasKey(version => version.Id);
            entity.Property(version => version.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(version => new { version.DocumentId, version.VersionNumber })
                .IsUnique();

            entity.HasOne(version => version.Document)
                .WithMany(document => document.Versions)
                .HasForeignKey(version => version.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DmsCase>(entity =>
        {
            entity.ToTable("DmsCases");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => item.CaseNumber).IsUnique();
            entity.HasIndex(item => item.Status);
            entity.HasIndex(item => item.ProjectCode);
            entity.HasIndex(item => item.DocumentTypeCode);

            entity.HasOne(item => item.Document)
                .WithMany()
                .HasForeignKey(item => item.DocumentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<DmsWorkflowTemplate>(entity =>
        {
            entity.ToTable("DmsWorkflowTemplates");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => item.Code).IsUnique();
            entity.HasIndex(item => item.DocumentTypeCode);
        });

        modelBuilder.Entity<DmsWorkflowInstance>(entity =>
        {
            entity.ToTable("DmsWorkflowInstances");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => item.Status);

            entity.HasOne(item => item.Case)
                .WithMany(caseItem => caseItem.WorkflowInstances)
                .HasForeignKey(item => item.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.Template)
                .WithMany()
                .HasForeignKey(item => item.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DmsWorkflowTask>(entity =>
        {
            entity.ToTable("DmsWorkflowTasks");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => item.Status);
            entity.HasIndex(item => item.AssigneePerson);
            entity.HasIndex(item => item.AssigneeRole);
            entity.HasIndex(item => item.DueAt);

            entity.HasOne(item => item.WorkflowInstance)
                .WithMany(instance => instance.Tasks)
                .HasForeignKey(item => item.WorkflowInstanceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DmsAuditEvent>(entity =>
        {
            entity.ToTable("DmsAuditEvents");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => new { item.EntityType, item.EntityId });
            entity.HasIndex(item => item.OccurredAt);
        });

        modelBuilder.Entity<DmsProjectRole>(entity =>
        {
            entity.ToTable("DmsProjectRoles");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => new { item.ProjectCode, item.RoleName, item.IsActive });
        });

        modelBuilder.Entity<DmsDelegation>(entity =>
        {
            entity.ToTable("DmsDelegations");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => new { item.DelegatingPerson, item.StartsAt, item.EndsAt });
        });

        modelBuilder.Entity<DmsNotification>(entity =>
        {
            entity.ToTable("DmsNotifications");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            entity.HasIndex(item => new { item.Recipient, item.IsRead });
            entity.HasIndex(item => item.DueAt);
        });
    }
}
