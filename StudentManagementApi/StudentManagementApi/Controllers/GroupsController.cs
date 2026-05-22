using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.DTOs.Group;
using StudentManagementApi.Services;

namespace StudentManagementApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAllAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group = await _groupService.GetByIdAsync(id);
            if (group == null)
                return NotFound("Qrup tapılmadı!");

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _groupService.CreateAsync(dto);
            if (result != "Qrup uğurla əlavə edildi!")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateGroupDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _groupService.UpdateAsync(id, dto);
            if (result == "Qrup tapılmadı!")
                return NotFound(result);

            if (result != "Qrup uğurla yeniləndi!")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _groupService.DeleteAsync(id);
            if (result == "Qrup tapılmadı!")
                return NotFound(result);

            return Ok(result);
        }
    }
}
