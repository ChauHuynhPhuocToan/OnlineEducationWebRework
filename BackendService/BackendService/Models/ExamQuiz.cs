using BackendService.AppConsts;
using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class ExamQuiz
    {
        [Key]
        public int ExamQuizId { get; set; }
        [StringLength(PropertyConst.ExamQuizNameMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.ExamQuizNameMinLength)]
        public string ExamQuizName { get; set; }
        public string ExamQuestion { get; set; }
        public CorrectAnswer ExamIsCorrect { get; set; }
        [StringLength(PropertyConst.ExamQuizOptionMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.ExamQuizOptionMinLength)]
        public string ExamOption1 { get; set; }
        [StringLength(PropertyConst.ExamQuizOptionMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.ExamQuizOptionMinLength)]
        public string ExamOption2 { get; set; }
        [StringLength(PropertyConst.ExamQuizOptionMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.ExamQuizOptionMinLength)]
        public string ExamOption3 { get; set; }
        [StringLength(PropertyConst.ExamQuizOptionMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.ExamQuizOptionMinLength)]
        public string ExamOption4 { get; set; }
        [StringLength(PropertyConst.ExamQuizOptionMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.ExamQuizOptionMinLength)]
        public string? ExamOption5 { get; set; }
        public string? ExamQuestionImageURL { get; set; }
        public string? ExamOptionImageURL1 { get; set; }
        public string? ExamOptionImageURL2 { get; set; }
        public string? ExamOptionImageURL3 { get; set; }
        public string? ExamOptionImageURL4 { get; set; }
        public string? ExamOptionImageURL5 { get; set; }
        public Byte[]? ExamThumbnailImage { get; set; }
        public Byte[]? ExamQuestionImage { get; set; }
        public Byte[]? ExamOptionImage1 { get; set; }
        public Byte[]? ExamOptionImage2 { get; set; }
        public Byte[]? ExamOptionImage3 { get; set; }
        public Byte[]? ExamOptionImage4 { get; set; }
        public Byte[]? ExamOptionImage5 { get; set; }
        public string? ExamTagTopic { get; set; }
        [StringLength(PropertyConst.ExamQuizCodeMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.ExamQuizCodeMinLength)]
        public string ExamQuizCode { get; set; }
        public string CourseId { get; set; }
        public int? ExamTime { get; set; }
        public string QuizId { get; set; }
        public Boolean IsBlocked { get; set; }
        public Boolean IsFinalQuiz { get; set; }
    }
    public enum CorrectAnswer
    {
        Answer1,
        Answer2,
        Answer3,
        Answer4
    }
}
