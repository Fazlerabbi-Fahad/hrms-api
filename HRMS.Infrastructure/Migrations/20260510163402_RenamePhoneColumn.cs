using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePhoneColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Phone",
                schema: "HRMS",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Phone",
                schema: "HRMS",
                table: "Employees",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "EmpCode",
                schema: "HRMS",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmpCode",
                schema: "HRMS",
                table: "Employees",
                column: "EmpCode",
                unique: true,
                filter: "[EmpCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneNumber",
                schema: "HRMS",
                table: "Employees",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_EmpCode",
                schema: "HRMS",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhoneNumber",
                schema: "HRMS",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmpCode",
                schema: "HRMS",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                schema: "HRMS",
                table: "Employees",
                newName: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Phone",
                schema: "HRMS",
                table: "Employees",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");
        }
    }
}
