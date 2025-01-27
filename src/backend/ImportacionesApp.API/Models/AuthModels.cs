using System.ComponentModel.DataAnnotations;

namespace ImportacionesApp.API.Models
{
    public class LoginRequest
    {
        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        public string Cedula { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "La contraseña debe contener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Cedula { get; set; }
    }

    public class UserValidationResponse
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}