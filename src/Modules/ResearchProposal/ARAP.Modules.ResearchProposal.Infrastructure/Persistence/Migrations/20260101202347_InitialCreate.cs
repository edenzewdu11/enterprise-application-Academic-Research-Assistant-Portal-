using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ARAP.Modules.ResearchProposal.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "research_proposal");

            migrationBuilder.CreateTable(
                name: "research_proposals",
                schema: "research_proposal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    advisor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    @abstract = table.Column<string>(name: "abstract", type: "character varying(3000)", maxLength: 3000, nullable: false),
                    research_question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    review_comments = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_research_proposals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "milestones",
                schema: "research_proposal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    research_proposal_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_milestones", x => x.id);
                    table.ForeignKey(
                        name: "FK_milestones_research_proposals_research_proposal_id",
                        column: x => x.research_proposal_id,
                        principalSchema: "research_proposal",
                        principalTable: "research_proposals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_milestones_research_proposal_id",
                schema: "research_proposal",
                table: "milestones",
                column: "research_proposal_id");

            migrationBuilder.CreateIndex(
                name: "ix_research_proposals_advisor_id",
                schema: "research_proposal",
                table: "research_proposals",
                column: "advisor_id");

            migrationBuilder.CreateIndex(
                name: "ix_research_proposals_created_at",
                schema: "research_proposal",
                table: "research_proposals",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_research_proposals_state",
                schema: "research_proposal",
                table: "research_proposals",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_research_proposals_student_id",
                schema: "research_proposal",
                table: "research_proposals",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "milestones",
                schema: "research_proposal");

            migrationBuilder.DropTable(
                name: "research_proposals",
                schema: "research_proposal");
        }
    }
}
