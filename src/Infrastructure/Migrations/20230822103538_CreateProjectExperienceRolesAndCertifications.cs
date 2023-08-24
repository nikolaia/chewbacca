using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees.Migrations
{
    /// <inheritdoc />
    public partial class CreateProjectExperienceRolesAndCertifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ProjectExperienceRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectExperienceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastSynced = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectExperienceRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectExperienceRoles_ProjectExperiences_ProjectExperienceId",
                        column: x => x.ProjectExperienceId,
                        principalTable: "ProjectExperiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(name: "Certifications", columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                LastSynced = table.Column<DateTime>(type: "datetime2", nullable: false),
                IssuedMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IssuedYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Certifications", x => x.Id);
                table.ForeignKey(name: "FK_Certifications_Employee_EmployeeId", column: x => x.EmployeeId,
                    principalTable: "Employees", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            });
     
            migrationBuilder.CreateIndex(
                name: "IX_ProjectExperienceRoles_ProjectExperienceId",
                table: "ProjectExperienceRoles",
                column: "ProjectExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_EmployeeId",
                table: "Certifications",
                column: "employeeId"
            );
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectExperienceRoles");
            migrationBuilder.DropTable(name: "Certifications");
        }
    }
}
