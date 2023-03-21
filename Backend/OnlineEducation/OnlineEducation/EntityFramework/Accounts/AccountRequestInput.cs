using OnlineEducation.Models;
using OnlineEducation.Shared;
using System.ComponentModel.DataAnnotations;

namespace OnlineEducation.EntityFramework.Accounts
{
    public class AccountRequestInput: BaseRequestClasses
    {
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsVerification { get; set; }
        public DateTime? LastLogOnDateMin { get; set; }
        public DateTime? LastLogOnDateMax { get; set; }
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; } 
        public Gender? Gender { get; set; }
        public string? Avatarpath { get; set; } 
        public string? UserDescription { get; set; }
        public double? BalanceMin { get; set; }
        public double? BalanceMax { get; set; }
    }
}
