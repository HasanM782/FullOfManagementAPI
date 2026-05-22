using StudentManagementApi.DTOs.Student;

namespace StudentManagementApi.Services
{
    public interface IStudentService
    {
        Task<List<StudentGetDto>> GetAllAsync(string? search, int page, int pageSize);
        Task<StudentGetDto?> GetByIdAsync(int id);
        Task<string> CreateAsync(CreateStudentDto dto);
        Task<string> UpdateAsync(int id, UpdateStudentDto dto);
        Task<string> DeleteAsync(int id);
    }
}
