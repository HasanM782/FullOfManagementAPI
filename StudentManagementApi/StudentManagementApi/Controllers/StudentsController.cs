using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.DTOs.Student;
using StudentManagementApi.DTOs.Student;
using StudentManagementApi.Services;
using StudentManagementApi.Services;

namespace StudentManagementApi.Controllers

{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students?search=ali&page=1&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetAll(string? search, int page = 1, int pageSize = 5)
        {
            var students = await _studentService.GetAllAsync(search, page, pageSize);
            return Ok(students);
        }

        // GET: api/students/3
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound("Tələbə tapılmadı!");

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studentService.CreateAsync(dto);
            if (result != "Tələbə uğurla əlavə edildi!")
                return BadRequest(result);

            return Ok(result);
        }

        // PUT: api/students/3
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateStudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studentService.UpdateAsync(id, dto);
            if (result == "Tələbə tapılmadı!")
                return NotFound(result);

            if (result != "Tələbə uğurla yeniləndi!")
                return BadRequest(result);

            return Ok(result);
        }

        // DELETE: api/students/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.DeleteAsync(id);
            if (result == "Tələbə tapılmadı!")
                return NotFound(result);

            return Ok(result);
        }
    }
}