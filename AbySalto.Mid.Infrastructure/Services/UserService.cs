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

        public async Task<ServiceResponse<AuthResult>> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return ServiceResponse<AuthResult>.Fail("Invalid login. Please try again.", 401);
            }

            var dto = UserMapper.ToDto(user);
            var token = _jwt.GenerateToken(dto);

            var newRefreshToken = _jwt.GenerateRefreshToken(user.Id);
            user.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResult(token, newRefreshToken, dto);
            return ServiceResponse<AuthResult>.Ok(response, "Login successful.");
        }

        public async Task<ServiceResponse<AuthResult>> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || !System.Net.Mail.MailAddress.TryCreate(request.Email, out _))
            {
                return ServiceResponse<AuthResult>.Fail("Invalid email format.", 400);
            }

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
            {
                return ServiceResponse<AuthResult>.Fail("Password must be at least 8 characters long.", 400);
            }

            if (!request.Password.Any(char.IsUpper) || !request.Password.Any(char.IsDigit))
            {
                return ServiceResponse<AuthResult>.Fail("Password must contain at least one uppercase letter and one number.", 400);
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return ServiceResponse<AuthResult>.Fail("User wioth email already exists.", 409);
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User user = new User { Email = request.Email, Username = request.Username, PasswordHash = hashedPassword };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = UserMapper.ToDto(user);
            var token = _jwt.GenerateToken(dto);

            var refreshToken = _jwt.GenerateRefreshToken(user.Id);
            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResult(token, refreshToken, dto);
            return ServiceResponse<AuthResult>.Ok(response);
        }

        public async Task<ServiceResponse<AuthResult>> RefreshAsync(string refreshToken)
        {
            var token = await _context.Set<RefreshToken>().Include(t => t.User).SingleOrDefaultAsync(t => t.Token == refreshToken);

            if (token == null || !token.IsActive)
            {
                return ServiceResponse<AuthResult>.Fail("Invalid refresh token.", 401);
            }

            var user = token.User;

            var newRefreshToken = _jwt.GenerateRefreshToken(user.Id);
            RevokeToken(token, newRefreshToken.Token);
            user.RefreshTokens.Add(newRefreshToken);

            var dto = UserMapper.ToDto(user);
            var newJwtToken = _jwt.GenerateToken(dto);

            await _context.SaveChangesAsync();

            var response = new AuthResult(newJwtToken, newRefreshToken, dto);
            return ServiceResponse<AuthResult>.Ok(response, "Token refreshed.");
        }

        public async Task<ServiceResponse<bool>> RevokeAsync(string refreshToken)
        {
            var token = await _context.Set<RefreshToken>().SingleOrDefaultAsync(t => t.Token == refreshToken);

            if (token == null || !token.IsActive)
            {
                return ServiceResponse<bool>.Ok(true);
            }

            RevokeToken(token);
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true);
        }

        private void RevokeToken(RefreshToken token, string? newToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.ReplacedByToken = newToken;
        }
    }
}
