using System;

using Infrastructure.ApiClients.DTOs;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees.Migrations
{
    /// <inheritdoc />
    public partial class CreateCompetencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competencies",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectExperienceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastSynced = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencies", x => new {x.Name, x.ProjectExperienceId});
                    table.ForeignKey(
                        name: "FK_Competencies_ProjectExperiences_ProjectExperienceId",
                        column: x => x.ProjectExperienceId,
                        principalTable: "ProjectExperiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_Competencies_Name",
                table: "Competencies",
                column: "Name");
            migrationBuilder.CreateIndex(
                name: "IX_Competencies_ProjectExperienceId",
                table: "Competencies",
                column: "ProjectExperienceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Competencies");
            
        }
    }
}
