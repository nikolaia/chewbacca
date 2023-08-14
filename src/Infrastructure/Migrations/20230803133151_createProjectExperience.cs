using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees.Migrations
{
    /// <inheritdoc />
    public partial class createProjectExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectExperiences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonthFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearTo= table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSynced = table.Column<bool>(type:"datetime", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectExperience_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_ProjectExperiences_EmployeeId",
                table: "ProjectExperiences",
                column: "EmployeeId");
            
            migrationBuilder.CreateTable(
                name: "ProjectExperienceRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectExperienceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSynced = table.Column<bool>(type:"datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectExperienceRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectExperienceRoles_ProjectExperiences",
                        column: x => x.ProjectExperienceId,
                        principalTable: "ProjectExperiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ProjectExperienceRoles");
            migrationBuilder.DropTable(
                name: "ProjectExperiences");
        }
        }
}
