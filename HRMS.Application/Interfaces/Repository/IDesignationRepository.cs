using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Designation;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IDesignationRepository
    {
        Task<(List<Designation>, int totalCount)> GetAllDesignationsAsync(QueryParameters parameters);
        Task<Designation?> GetDesignationByIdAsync(int id);
        Task<Designation> CreateDesignationAsync(Designation Designation);
        Task<Designation> UpdateDesignationAsync(int id, Designation Designation);
        Task<bool> DeleteDesignationAsync(int id, int userId);
    }
}
