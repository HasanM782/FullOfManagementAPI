using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs.Auth;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthService(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);

            if (userExists)
                return "Bu istifadəçi artıq mövcuddur!";

            var user = new AppUser
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return "Qeydiyyat uğurlu oldu!";
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return null;

            var passwordCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordCorrect)
                return null;

            return _tokenService.CreateToken(user);
        }
    }
}