using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        [StringLength(PropertyConst.UsernameMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.UsernameMinLength)]
        public string Username { get; set; }
        //Password hashcode round 4
        [StringLength(PropertyConst.PasswordMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.PasswordMinLength)]
        public string Password { get; set; }
        public AccountRole Role { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean Verification { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<Course> Courses { get; set; }

        public ICollection<AccountInventory> AccountInventories { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
    public enum AccountRole
    {
        Admin, 
        Instructor,
        User
    }
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public AccountRole Role { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean Verification { get; set; }
    }
}
