using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ARAP.Modules.AcademicIntegrity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_Jan8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "academic_integrity");

            migrationBuilder.CreateTable(
                name: "plagiarism_checks",
                schema: "academic_integrity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    initiated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    similarity_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    external_check_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    initiated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plagiarism_checks", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_plagiarism_checks_document_id",
                schema: "academic_integrity",
                table: "plagiarism_checks",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "IX_plagiarism_checks_proposal_id",
                schema: "academic_integrity",
                table: "plagiarism_checks",
                column: "proposal_id");

            migrationBuilder.CreateIndex(
                name: "IX_plagiarism_checks_status",
                schema: "academic_integrity",
                table: "plagiarism_checks",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "plagiarism_checks",
                schema: "academic_integrity");
        }
    }
}
