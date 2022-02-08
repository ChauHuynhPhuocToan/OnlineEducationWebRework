using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [StringLength(PropertyConst.CourseNameMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.CourseNameMinLength)]
        public string CourseName { get; set; }
        public float Rating { get; set; }
        public double NumberOfRatings { get; set; }
        public double NumberOfParticipants { get; set; }
        public float Price { get; set; }
        public DateTime StartDate { get; set; }
        public string CourseDuration { get; set; }
        [StringLength(PropertyConst.CourseDescriptionMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.CourseDescriptionMinLength)]
        public string Description { get; set; }
        public byte[] ThumbnailImage { get; set; }
        public CourseHastag Hastag { get; set; }
        public CourseLevel Level { get; set; }
        public DateTime LastUpdate { get; set; }
        public int LessonCounter { get; set; }
        public Boolean IsActive { get; set; }
        public int NumberOfView { get; set; }
        public Account Account { get; set; }
        [Required]
        public int AccountId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<AccountInventory> AccountInventories { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }
    public enum CourseHastag
    {
        C,
        CSharp, 
        CPlus,
        Java, 
        Htmlcss,
        Python,
        IOSAndroid,
        AI,
        Javascript,
        MachineLearning,
        UXUI, 
        Framework,
        Orther
    }
    public enum CourseLevel
    {
        Basic,
        Tutorial,
        Advance,
        DeepLearning,
        Guide
    }
}
