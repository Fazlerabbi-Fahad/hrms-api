using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchemaToHRMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HRMS");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "Salaries",
                newName: "Salaries",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "Payrolls",
                newName: "Payrolls",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "PaymentStatuses",
                newName: "PaymentStatuses",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "EmploymentStatuses",
                newName: "EmploymentStatuses",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employees",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "Designations",
                newName: "Designations",
                newSchema: "HRMS");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Departments",
                newSchema: "HRMS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "HRMS",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "HRMS",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Salaries",
                schema: "HRMS",
                newName: "Salaries");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "HRMS",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Payrolls",
                schema: "HRMS",
                newName: "Payrolls");

            migrationBuilder.RenameTable(
                name: "PaymentStatuses",
                schema: "HRMS",
                newName: "PaymentStatuses");

            migrationBuilder.RenameTable(
                name: "EmploymentStatuses",
                schema: "HRMS",
                newName: "EmploymentStatuses");

            migrationBuilder.RenameTable(
                name: "Employees",
                schema: "HRMS",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "Designations",
                schema: "HRMS",
                newName: "Designations");

            migrationBuilder.RenameTable(
                name: "Departments",
                schema: "HRMS",
                newName: "Departments");
        }
    }
}
