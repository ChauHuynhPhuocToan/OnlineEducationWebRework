using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [StringLength(PropertyConst.PersonNameMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.PersonNameMinLength)]
        public string FirstName { get; set; }
        [StringLength(PropertyConst.PersonNameMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.PersonNameMinLength)]
        public string LastName { get; set; }
        [StringLength(PropertyConst.PhoneNumberMaxLength, ErrorMessage = "The {0} maximum lenghth must be {1}!")]
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLogOnDate { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public Byte[]? AvatarPath { get; set; }
        public float? Balance { get; set; }
        [StringLength(PropertyConst.PesonDescriptionMaxLength, ErrorMessage = "The {0} maximum lenghth must be {1}!")]
        public string? Description { get; set; }
        public Account Account { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}
