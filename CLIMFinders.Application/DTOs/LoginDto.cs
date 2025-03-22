using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Application.DTOs
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UIMessage { get; set; }
    }
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string UIMessage { get; set; }
        public int RoleId { get; set; }
        public int? SubRoleId { get; set; }
        public string? BusinessId { get; set; }
        public string SubscriptionId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public bool IsActiveSubscription { get; set; } = true;
    }
}
