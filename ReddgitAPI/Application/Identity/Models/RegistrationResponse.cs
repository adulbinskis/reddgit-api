﻿using ReddgitAPI.Application.Identity.Roles;
using System.ComponentModel.DataAnnotations;

namespace ReddgitAPI.Application.Identity.Models
{
    public class RegistrationResponse
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
