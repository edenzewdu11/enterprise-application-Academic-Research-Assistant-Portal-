using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ARAP.Modules.DocumentReview.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_Jan8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "document_review");

            migrationBuilder.CreateTable(
                name: "documents",
                schema: "document_review",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DocumentVersion = table.Column<int>(type: "integer", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reviewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    feedback = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_documents_proposal_id",
                schema: "document_review",
                table: "documents",
                column: "proposal_id");

            migrationBuilder.CreateIndex(
                name: "IX_documents_reviewer_id",
                schema: "document_review",
                table: "documents",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_documents_status",
                schema: "document_review",
                table: "documents",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_documents_student_id",
                schema: "document_review",
                table: "documents",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documents",
                schema: "document_review");
        }
    }
}
