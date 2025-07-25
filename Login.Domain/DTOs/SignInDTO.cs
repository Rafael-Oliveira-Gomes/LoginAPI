﻿using System.ComponentModel.DataAnnotations;

namespace Login.Domain.DTOs;

public class SignInDTO
{
    [Required(ErrorMessage = "User Name is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
