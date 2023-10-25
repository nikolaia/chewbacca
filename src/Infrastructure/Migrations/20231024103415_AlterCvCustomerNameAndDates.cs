using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees.Migrations
{
    /// <inheritdoc />
    public partial class AlterCvCustomerNameAndDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "MonthFrom",
            table: "ProjectExperiences");

            migrationBuilder.DropColumn(
                name: "YearFrom",
                table: "ProjectExperiences");

            migrationBuilder.DropColumn(
                name: "MonthTo",
                table: "ProjectExperiences");

            migrationBuilder.DropColumn(
                name: "YearTo",
                table: "ProjectExperiences");

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "ProjectExperiences",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "ProjectExperiences",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "ProjectExperiences",
                type: "date",
                nullable: true);
            migrationBuilder.DropColumn(
      name: "YearFrom",
      table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "MonthTo",
                table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "YearTo",
                table: "WorkExperiences");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "WorkExperiences",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "WorkExperiences",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "WorkExperiences",
                type: "date",
                nullable: true);
            migrationBuilder.DropColumn(
name: "Month",
table: "Presentations");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Presentations");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Presentations",
                type: "date",
                nullable: true);
            migrationBuilder.DropColumn(
       name: "IssuedYear",
       table: "Certifications");

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuedDate",
                table: "Certifications",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issuer",
                table: "Certifications",
                type: "varchar(255)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer",
                table: "ProjectExperiences");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "ProjectExperiences");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "ProjectExperiences");

            migrationBuilder.AddColumn<int>(
                name: "MonthFrom",
                table: "ProjectExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearFrom",
                table: "ProjectExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MonthTo",
                table: "ProjectExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearTo",
                table: "ProjectExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.DropColumn(
    name: "Company",
    table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "WorkExperiences");

            migrationBuilder.AddColumn<int>(
                name: "MonthFrom",
                table: "WorkExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearFrom",
                table: "WorkExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MonthTo",
                table: "WorkExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearTo",
                table: "WorkExperiences",
                type: "int",
                nullable: false);

            migrationBuilder.DropColumn(
name: "Date",
table: "Presentations");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Presentations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Presentations",
                type: "int",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.DropColumn(
name: "IssuedDate",
table: "Certifications");

            migrationBuilder.DropColumn(
                name: "Issuer",
                table: "Certifications");

            migrationBuilder.AddColumn<int>(
                name: "IssuedMonth",
                table: "Certifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IssuedYear",
                table: "Certifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

    }
}
