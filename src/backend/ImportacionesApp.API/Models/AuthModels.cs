using System.ComponentModel.DataAnnotations;

namespace ImportacionesApp.API.Models
{
    public class LoginRequest
    {
        public required string Cedula { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterRequest
    {
        public required string Cedula { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "La contraseña debe contener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial")]
        public required string Password { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }

    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string Name { get; set; }
        public required string Cedula { get; set; }
    }

    public class UserValidationResponse
    {
        public bool IsValid { get; set; }
        public required string Name { get; set; }
        public required string Message { get; set; }
    }
}