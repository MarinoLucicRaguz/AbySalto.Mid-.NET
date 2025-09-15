using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Application.Mapper;
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

        public async Task<ServiceResponse<UserDto>> GetUserAsync(int id, CancellationToken ct = default)
        {
            var user = await _context.Users.Include(u => u.BasketItems).Include(u => u.Favorites).SingleOrDefaultAsync(u => u.Id == id, ct);
            if (user == null)
            {
                return ServiceResponse<UserDto>.Fail($"User {id} not found.", 404);
            }

            var dto = UserMapper.ToDto(user);
            return ServiceResponse<UserDto>.Ok(dto, statusCode: 200);
        }

        public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return ServiceResponse<AuthResponseDto>.Fail("Invalid login. Please try again.", 401);
            }

            var dto = UserMapper.ToDto(user);
            var token = _jwt.GenerateToken(dto);

            var response = new AuthResponseDto(token, dto);

            return ServiceResponse<AuthResponseDto>.Ok(response, "Login successful.");
        }

        public async Task<ServiceResponse<AuthResponseDto>> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || !System.Net.Mail.MailAddress.TryCreate(request.Email, out _))
            {
                return ServiceResponse<AuthResponseDto>.Fail("Invalid email format.", 400);
            }

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
            {
                return ServiceResponse<AuthResponseDto>.Fail("Password must be at least 8 characters long.", 400);
            }

            if (!request.Password.Any(char.IsUpper) || !request.Password.Any(char.IsDigit))
            {
                return ServiceResponse<AuthResponseDto>.Fail("Password must contain at least one uppercase letter and one number.", 400);
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return ServiceResponse<AuthResponseDto>.Fail("User wioth email already exists.", 409);
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User user = new User { Email = request.Email, Username = request.Username, PasswordHash = hashedPassword };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = UserMapper.ToDto(user);
            var token = _jwt.GenerateToken(dto);

            var response = new AuthResponseDto(token, dto);
            return ServiceResponse<AuthResponseDto>.Ok(response);
        }
    }
}
