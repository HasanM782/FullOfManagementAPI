using StudentManagementApi.DTOs.Group;

namespace StudentManagementApi.Services
{
    public interface IGroupService
    {
        Task<List<GroupGetDto>> GetAllAsync();
        Task<GroupGetDto?> GetByIdAsync(int id);
        Task<string> CreateAsync(CreateGroupDto dto);
        Task<string> UpdateAsync(int id, UpdateGroupDto dto);
        Task<string> DeleteAsync(int id);
    }
}
