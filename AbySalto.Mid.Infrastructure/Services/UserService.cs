using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AbysaltoDbContext _context;
        private readonly JwtTokenService _jwt;

        public UserService(AbysaltoDbContext context, JwtTokenService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public async Task<ServiceResponse<UserDto>> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return ServiceResponse<UserDto>.Fail($"User {id} not found.", 404);

            var dto = new UserDto(user.Id, user.Username, user.Email, user.FirstName, user.LastName);
            return ServiceResponse<UserDto>.Ok(dto, statusCode: 200);
        }

        public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return ServiceResponse<AuthResponseDto>.Fail("Invalid login. Please try again.", 401);
            }

            var dto = new UserDto(user.Id, user.Username, user.Email, user.FirstName, user.LastName);
            var token = _jwt.GenerateToken(dto);

            var response = new AuthResponseDto(token, dto);

            return ServiceResponse<AuthResponseDto>.Ok(response, "Login successful.");
        }

        public async Task<ServiceResponse<AuthResponseDto>> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return ServiceResponse<AuthResponseDto>.Fail("User wioth email already exists.", 409);
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User user = new User { Email = request.Email, Username = request.Username, PasswordHash = hashedPassword };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            UserDto dto = new UserDto(user.Id, user.Username, user.Email, user.FirstName, user.LastName);
            var token = _jwt.GenerateToken(dto);

            var response = new AuthResponseDto(token, dto);
            return ServiceResponse<AuthResponseDto>.Ok(response);
        }
    }
}
