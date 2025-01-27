using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ImportacionesApp.API.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ImportacionesApp.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<UserValidationResponse> ValidateUserAsync(string cedula);
        Task<bool> RegisterAsync(RegisterRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly GIAXContext _giaxContext;
        private readonly ProyectosClaudeContext _proyectosContext;

        public AuthService(
            IConfiguration configuration,
            GIAXContext giaxContext,
            ProyectosClaudeContext proyectosContext)
        {
            _configuration = configuration;
            _giaxContext = giaxContext;
            _proyectosContext = proyectosContext;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var credentials = await _proyectosContext.CreedencialesApps
                .FirstOrDefaultAsync(c => c.Cedula == request.Cedula && c.Fuente == "ImportacionesApp");

            if (credentials == null || !VerifyPassword(request.Password, credentials.Contrasena))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            var userData = await _giaxContext.DirPartyTable
                .FirstOrDefaultAsync(u => u.VATNum_FE == request.Cedula);

            if (userData == null)
            {
                throw new UnauthorizedAccessException("Usuario no encontrado");
            }

            var token = GenerateJwtToken(request.Cedula, userData.Name);

            return new LoginResponse
            {
                Token = token,
                Name = userData.Name,
                Cedula = request.Cedula
            };
        }

        public async Task<UserValidationResponse> ValidateUserAsync(string cedula)
        {
            var user = await _giaxContext.DirPartyTable
                .FirstOrDefaultAsync(u => u.VATNum_FE == cedula);

            if (user == null)
            {
                return new UserValidationResponse
                {
                    IsValid = false,
                    Message = "Usuario no encontrado"
                };
            }

            var existingCredentials = await _proyectosContext.CreedencialesApps
                .AnyAsync(c => c.Cedula == cedula && c.Fuente == "ImportacionesApp");

            if (existingCredentials)
            {
                return new UserValidationResponse
                {
                    IsValid = false,
                    Message = "El usuario ya tiene credenciales registradas"
                };
            }

            return new UserValidationResponse
            {
                IsValid = true,
                Name = user.Name,
                Message = "Usuario válido"
            };
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var validation = await ValidateUserAsync(request.Cedula);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(validation.Message);
            }

            var hashedPassword = HashPassword(request.Password);

            var credentials = new CreedencialesApps
            {
                Cedula = request.Cedula,
                Contrasena = hashedPassword,
                Fuente = "ImportacionesApp",
                FechaCreacion = DateTime.UtcNow,
                UltimaModificacion = DateTime.UtcNow
            };

            _proyectosContext.CreedencialesApps.Add(credentials);
            await _proyectosContext.SaveChangesAsync();

            return true;
        }

        private string GenerateJwtToken(string cedula, string userName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cedula),
                new Claim(ClaimTypes.Name, userName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
}