using HRMS.Application.DTOs.Menu;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IMenuRepository
    {
        Task<List<MenuResponseDto>> GetUserWiseMenuAsync(int userId);
    }
}
