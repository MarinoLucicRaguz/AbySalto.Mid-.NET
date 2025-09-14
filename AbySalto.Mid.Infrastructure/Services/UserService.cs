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

        public Task<ServiceResponse<UserDto>> GetUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return ServiceResponse<AuthResponseDto>.Fail("Pogreška prilikom prijave, molimo Vas pokušajte ponovno.");
            }

            var dto = new UserDto(user.Id, user.Username, user.Email, user.FirstName, user.LastName);
            var token = _jwt.GenerateToken(dto);

            var response = new AuthResponseDto(token, dto);

            return ServiceResponse<AuthResponseDto>.Ok(response, "Uspješna prijava.");
        }

        public async Task<ServiceResponse<AuthResponseDto>> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return ServiceResponse<AuthResponseDto>.Fail("Korisnik s tim mailom već postoji.");
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
