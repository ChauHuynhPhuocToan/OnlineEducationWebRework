using OnlineEducation.Models;
using OnlineEducation.Shared;
using System.ComponentModel.DataAnnotations;

namespace OnlineEducation.Dtos.AccountDtos
{
    public class CreateUpdateAccountDto: BaseClasses
    {
        public string UserName { get; set; } = string.Empty;
        public string InputPassword { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsVerification { get; set; }
        public DateTime LastLogOnDate { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string? Avatarpath { get; set; } = string.Empty;
        public IFormFile? AvatarInput { get; set; }
        public string? UserDescription { get; set; } = string.Empty;
        public double Balance { get; set; }
    }
}
