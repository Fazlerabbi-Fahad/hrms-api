using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class User_PhoneNumber_Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX [IX_Users_ContactNumber] ON [HRMS].[Users]");
            migrationBuilder.Sql("EXEC sp_rename 'HRMS.Users.ContactNumber', 'PhoneNumber', 'COLUMN'");
            migrationBuilder.Sql("CREATE INDEX [IX_Users_PhoneNumber] ON [HRMS].[Users] ([PhoneNumber])");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX [IX_Users_PhoneNumber] ON [HRMS].[Users]");
            migrationBuilder.Sql("EXEC sp_rename 'HRMS.Users.PhoneNumber', 'ContactNumber', 'COLUMN'");
            migrationBuilder.Sql("CREATE INDEX [IX_Users_ContactNumber] ON [HRMS].[Users] ([ContactNumber])");
        }
    }
}
