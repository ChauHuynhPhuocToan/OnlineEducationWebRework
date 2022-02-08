using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Questionpool
    {
        [Key]
        public int QuestionpoolId { get; set; }
        [StringLength(PropertyConst.QuestionpoolNameMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.QuestionpoolNameMinLength)]
        public string QuestionpoolName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public CourseHastag Hastag { get; set; }
        public Byte[]? QuestionpoolThumbnailImage { get; set; }
        public string? QuestionpoolThumbnailImageURL { get; set; }
        public Boolean IsActive { get; set; }
        public string AccountId { get; set; }
        public ICollection<Quiz> Quizs { get; set; }
    }
}
