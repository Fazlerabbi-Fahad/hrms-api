using HRMS.Application.DTOs.Menu;
using HRMS.Application.Interfaces.Repository;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly HRMSDbContext _context;

        public MenuRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuResponseDto>> GetUserWiseMenuAsync(int userId)
        {
            var userParam = new SqlParameter("@User", userId);

            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            var menus = new List<MenuResponseDto>();

            using var command = connection.CreateCommand();
            command.CommandText = "EXEC HRMS.sp_GetUserWiseMenu @User";
            command.Parameters.Add(userParam);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                menus.Add(new MenuResponseDto
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    MenuName = reader.GetString(reader.GetOrdinal("MenuName")),
                    Route = reader.GetString(reader.GetOrdinal("Route")),
                    Icon = reader.IsDBNull(reader.GetOrdinal("Icon"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Icon")),
                    Sequence = reader.GetInt32(reader.GetOrdinal("Sequence"))
                });
            }

            await connection.CloseAsync();

            // Return sorted by sequence
            return menus.OrderBy(m => m.Sequence).ToList();
        }
    }
}
