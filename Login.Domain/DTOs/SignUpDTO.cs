using System.ComponentModel.DataAnnotations;

namespace Login.Domain.DTOs;

public class SignUpDTO
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string PasswordConfirm { get; set; }

    [Required(ErrorMessage = "Salário é necessario")]
}
