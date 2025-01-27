using Microsoft.AspNetCore.Mvc;
using ImportacionesApp.API.Models;
using ImportacionesApp.API.Services;

namespace ImportacionesApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Intento de inicio de sesión fallido para cédula: {Cedula}", request.Cedula);
                return Unauthorized(new { message = ex.Message });
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
                var response = await _authService.ValidateUserAsync(cedula);
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
                if (request.Password != request.ConfirmPassword)
                {
                    return BadRequest(new { message = "Las contraseñas no coinciden" });
                }

                var result = await _authService.RegisterAsync(request);
                return Ok(new { message = "Usuario registrado exitosamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el registro para cédula: {Cedula}", request.Cedula);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}