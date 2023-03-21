using OnlineEducation.Models;
using OnlineEducation.Shared;

namespace OnlineEducation.Dtos.AccountDtos
{
    public class AccountDto : BaseClasses
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; } 
        public bool IsActive { get; set; }
        public bool IsVerification { get; set; }
        public DateTime LastLogOnDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string? Avatarpath { get; set; }
        public string? UserDescription { get; set; }
        public double Balance { get; set; }

        public AccountDto()
        {

        }
    }
}
