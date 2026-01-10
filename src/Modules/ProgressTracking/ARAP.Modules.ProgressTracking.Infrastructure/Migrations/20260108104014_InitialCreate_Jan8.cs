using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ARAP.Modules.ProgressTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_Jan8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "progress_tracking");

            migrationBuilder.CreateTable(
                name: "activity_logs",
                schema: "progress_tracking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    progress_percentage = table.Column<int>(type: "integer", nullable: false),
                    logged_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    hours_spent = table.Column<int>(type: "integer", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_logs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_activity_logs_logged_at",
                schema: "progress_tracking",
                table: "activity_logs",
                column: "logged_at");

            migrationBuilder.CreateIndex(
                name: "ix_activity_logs_proposal_id",
                schema: "progress_tracking",
                table: "activity_logs",
                column: "proposal_id");

            migrationBuilder.CreateIndex(
                name: "ix_activity_logs_student_id",
                schema: "progress_tracking",
                table: "activity_logs",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_logs",
                schema: "progress_tracking");
        }
    }
}
