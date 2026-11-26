using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSVNexus.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDmsWorkflowLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DmsAuditEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Actor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsAuditEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DmsCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CaseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DocumentTypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentStep = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedRole = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DueAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DmsCases_DmsDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "DmsDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DmsDelegations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    DelegatingPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DelegatePerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsDelegations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DmsNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Recipient = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    DueAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DmsProjectRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ProjectCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DmsWorkflowTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentTypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StepsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RuleDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsWorkflowTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DmsWorkflowInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentStep = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsWorkflowInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DmsWorkflowInstances_DmsCases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "DmsCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DmsWorkflowInstances_DmsWorkflowTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "DmsWorkflowTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DmsWorkflowTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    WorkflowInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AssigneeRole = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AssigneePerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DueAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsWorkflowTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DmsWorkflowTasks_DmsWorkflowInstances_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalTable: "DmsWorkflowInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DmsAuditEvents_EntityType_EntityId",
                table: "DmsAuditEvents",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_DmsAuditEvents_OccurredAt",
                table: "DmsAuditEvents",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_DmsCases_CaseNumber",
                table: "DmsCases",
                column: "CaseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DmsCases_DocumentId",
                table: "DmsCases",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DmsCases_DocumentTypeCode",
                table: "DmsCases",
                column: "DocumentTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_DmsCases_ProjectCode",
                table: "DmsCases",
                column: "ProjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_DmsCases_Status",
                table: "DmsCases",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DmsDelegations_DelegatingPerson_StartsAt_EndsAt",
                table: "DmsDelegations",
                columns: new[] { "DelegatingPerson", "StartsAt", "EndsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DmsNotifications_DueAt",
                table: "DmsNotifications",
                column: "DueAt");

            migrationBuilder.CreateIndex(
                name: "IX_DmsNotifications_Recipient_IsRead",
                table: "DmsNotifications",
                columns: new[] { "Recipient", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_DmsProjectRoles_ProjectCode_RoleName_IsActive",
                table: "DmsProjectRoles",
                columns: new[] { "ProjectCode", "RoleName", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowInstances_CaseId",
                table: "DmsWorkflowInstances",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowInstances_Status",
                table: "DmsWorkflowInstances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowInstances_TemplateId",
                table: "DmsWorkflowInstances",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowTasks_AssigneePerson",
                table: "DmsWorkflowTasks",
                column: "AssigneePerson");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowTasks_AssigneeRole",
                table: "DmsWorkflowTasks",
                column: "AssigneeRole");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowTasks_DueAt",
                table: "DmsWorkflowTasks",
                column: "DueAt");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowTasks_Status",
                table: "DmsWorkflowTasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowTasks_WorkflowInstanceId",
                table: "DmsWorkflowTasks",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowTemplates_Code",
                table: "DmsWorkflowTemplates",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DmsWorkflowTemplates_DocumentTypeCode",
                table: "DmsWorkflowTemplates",
                column: "DocumentTypeCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DmsAuditEvents");

            migrationBuilder.DropTable(
                name: "DmsDelegations");

            migrationBuilder.DropTable(
                name: "DmsNotifications");

            migrationBuilder.DropTable(
                name: "DmsProjectRoles");

            migrationBuilder.DropTable(
                name: "DmsWorkflowTasks");

            migrationBuilder.DropTable(
                name: "DmsWorkflowInstances");

            migrationBuilder.DropTable(
                name: "DmsCases");

            migrationBuilder.DropTable(
                name: "DmsWorkflowTemplates");
        }
    }
}
