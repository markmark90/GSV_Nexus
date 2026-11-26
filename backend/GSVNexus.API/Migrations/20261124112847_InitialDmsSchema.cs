using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSVNexus.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDmsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DmsCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DmsCategories_DmsCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DmsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DmsDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartnerOrProject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Draft"),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DmsDocuments_DmsCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DmsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DmsDocumentVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    StoragePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChangeNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmsDocumentVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DmsDocumentVersions_DmsDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "DmsDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DmsCategories_Name",
                table: "DmsCategories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DmsCategories_ParentId",
                table: "DmsCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DmsDocuments_CategoryId",
                table: "DmsDocuments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DmsDocuments_IsDeleted",
                table: "DmsDocuments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DmsDocuments_Name",
                table: "DmsDocuments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DmsDocuments_Status",
                table: "DmsDocuments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DmsDocumentVersions_DocumentId_VersionNumber",
                table: "DmsDocumentVersions",
                columns: new[] { "DocumentId", "VersionNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DmsDocumentVersions");

            migrationBuilder.DropTable(
                name: "DmsDocuments");

            migrationBuilder.DropTable(
                name: "DmsCategories");
        }
    }
}
