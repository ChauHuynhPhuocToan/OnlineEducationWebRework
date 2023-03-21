using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineEducation.Dtos.AccountDtos;
using OnlineEducation.Shared;
using OnlineEducation.Ultils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineEducation.Models
{
    [Table("Accounts")]
    public class Account: BaseClasses
    {
        [StringLength(AppConst.BasicStringLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string UserName { get; set; } = string.Empty;
        [StringLength(AppConst.BasicLargeLongLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string Password { get; set; } = string.Empty;
        [StringLength(AppConst.BasicStringLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string Role { get; set; } = string.Empty;
        [StringLength(AppConst.BasicStringMediumLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsVerification { get; set; }
        public DateTime LastLogOnDate { get; set; }
        [StringLength(AppConst.BasicStringLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(AppConst.BasicStringLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string? Avatarpath { get; set; } = string.Empty;
        public string? UserDescription { get; set; } = string.Empty;
        public double Balance { get; set; } = 0.0;

        public Account() 
            : base()
        {

        }

        public static async Task<CreateUpdateAccountDto> CreateAccount(CreateUpdateAccountDto account)
        {
            var id = Guid.NewGuid();    
            account.Id = id;
            account.CreationTime = DateTime.Now;
            account.ModificationTime = null;
            account.CreatorId = id;
            account.ModifierId = null;
            account.IsDelete = false;
            account.Password = HashPashword(account.Password);
            account.LastLogOnDate = DateTime.Now;
            account.Avatarpath = await FileHandlers.UploadFile(account.AvatarInput, id);
            return account;
        }

        public static string HashPashword(string pass)
        {
            return BCrypt.Net.BCrypt.HashPassword(pass);
        }

        public static bool VerifiedPashword(string pass, string dbPass)
        {
            return BCrypt.Net.BCrypt.Verify(pass, dbPass);
        }

    }
}
