using StudentManagementApi.Models;

namespace StudentManagementApi.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync(string? search, int page, int pageSize);
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Student student);
        Task<bool> EmailExistsAsync(string email);
    }
}
