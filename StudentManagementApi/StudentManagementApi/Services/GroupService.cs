using StudentManagementApi.DTOs.Group;
using StudentManagementApi.Models;
using StudentManagementApi.Repositories;

namespace StudentManagementApi.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<List<GroupGetDto>> GetAllAsync()
        {
            var groups = await _groupRepository.GetAllAsync();

            return groups.Select(g => new GroupGetDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
        }

        public async Task<GroupGetDto?> GetByIdAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) return null;

            return new GroupGetDto
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public async Task<string> CreateAsync(CreateGroupDto dto)
        {
            var nameExists = await _groupRepository.NameExistsAsync(dto.Name);
            if (nameExists)
                return "Bu adda qrup artıq mövcuddur!";

            var group = new Group
            {
                Name = dto.Name
            };

            await _groupRepository.AddAsync(group);
            return "Qrup uğurla əlavə edildi!";
        }

        public async Task<string> UpdateAsync(int id, UpdateGroupDto dto)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
                return "Qrup tapılmadı!";

            var nameExists = await _groupRepository.NameExistsAsync(dto.Name);
            if (nameExists && group.Name != dto.Name)
                return "Bu adda qrup artıq mövcuddur!";

            group.Name = dto.Name;

            await _groupRepository.UpdateAsync(group);
            return "Qrup uğurla yeniləndi!";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
                return "Qrup tapılmadı!";

            await _groupRepository.DeleteAsync(group);
            return "Qrup uğurla silindi!";
        }
    }
}
