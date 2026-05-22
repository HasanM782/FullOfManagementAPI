using StudentManagementApi.DTOs.Student;
using StudentManagementApi.Models;
using StudentManagementApi.Repositories;

namespace StudentManagementApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;

        public StudentService(IStudentRepository studentRepository, IGroupRepository groupRepository)
        {
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
        }

        public async Task<List<StudentGetDto>> GetAllAsync(string? search, int page, int pageSize)
        {
            var students = await _studentRepository.GetAllAsync(search, page, pageSize);

            return students.Select(s => new StudentGetDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Age = s.Age,
                Email = s.Email,
                GroupName = s.Group.Name
            }).ToList();
        }

        public async Task<StudentGetDto?> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return null;

            return new StudentGetDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Age = student.Age,
                Email = student.Email,
                GroupName = student.Group.Name
            };
        }

        public async Task<string> CreateAsync(CreateStudentDto dto)
        {
            var groupExists = await _groupRepository.GetByIdAsync(dto.GroupId);
            if (groupExists == null)
                return "Bu ID-li qrup mövcud deyil!";

            var emailExists = await _studentRepository.EmailExistsAsync(dto.Email);
            if (emailExists)
                return "Bu email artıq istifadə olunur!";

            var student = new Student
            {
                FullName = dto.FullName,
                Age = dto.Age,
                Email = dto.Email,
                GroupId = dto.GroupId
            };

            await _studentRepository.AddAsync(student);
            return "Tələbə uğurla əlavə edildi!";
        }

        public async Task<string> UpdateAsync(int id, UpdateStudentDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return "Tələbə tapılmadı!";

            var groupExists = await _groupRepository.GetByIdAsync(dto.GroupId);
            if (groupExists == null)
                return "Bu ID-li qrup mövcud deyil!";

            var emailExists = await _studentRepository.EmailExistsAsync(dto.Email);
            if (emailExists && student.Email != dto.Email)
                return "Bu email artıq istifadə olunur!";

            student.FullName = dto.FullName;
            student.Age = dto.Age;
            student.Email = dto.Email;
            student.GroupId = dto.GroupId;

            await _studentRepository.UpdateAsync(student);
            return "Tələbə uğurla yeniləndi!";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return "Tələbə tapılmadı!";

            await _studentRepository.DeleteAsync(student);
            return "Tələbə uğurla silindi!";
        }
    }
}
