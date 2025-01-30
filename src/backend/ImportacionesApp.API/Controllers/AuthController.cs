using Microsoft.AspNetCore.Mvc;
using ImportacionesApp.API.Models;
using ImportacionesApp.API.Services;

namespace ImportacionesApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Implementación temporal para pruebas
                var response = new LoginResponse
                {
                    Token = "test-token",
                    Name = "Usuario Prueba",
                    Cedula = request.Cedula
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el inicio de sesión para cédula: {Cedula}", request.Cedula);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("validate")]
        public async Task<ActionResult<UserValidationResponse>> ValidateUser([FromBody] string cedula)
        {
            try
            {
                // Implementación temporal para pruebas
                var response = new UserValidationResponse
                {
                    IsValid = true,
                    Name = "Usuario Prueba",
                    Message = "Usuario válido"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando usuario con cédula: {Cedula}", cedula);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Implementación temporal para pruebas
                return Ok(new { message = "Usuario registrado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el registro para cédula: {Cedula}", request.Cedula);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}