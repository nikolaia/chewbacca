using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees.Migrations
{
    /// <inheritdoc />
    public partial class AddAllergiesAndDietaryPreferencesToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeAllergiesAndDietaryPreferences_EmployeeId",
                table: "EmployeeAllergiesAndDietaryPreferences");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAllergiesAndDietaryPreferences_EmployeeId",
                table: "EmployeeAllergiesAndDietaryPreferences",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeAllergiesAndDietaryPreferences_EmployeeId",
                table: "EmployeeAllergiesAndDietaryPreferences");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAllergiesAndDietaryPreferences_EmployeeId",
                table: "EmployeeAllergiesAndDietaryPreferences",
                column: "EmployeeId");
        }
    }
}
