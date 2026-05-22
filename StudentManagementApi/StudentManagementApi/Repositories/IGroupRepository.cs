using StudentManagementApi.Models;

namespace StudentManagementApi.Repositories
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAllAsync();
        Task<Group?> GetByIdAsync(int id);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(Group group);
        Task<bool> NameExistsAsync(string name);
    }
}
