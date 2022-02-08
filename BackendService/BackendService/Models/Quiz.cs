using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }
        [StringLength(PropertyConst.QuestionMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.QuestionMinLength)]
        public string Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public Byte[]? QuizImage { get; set; }
        public string? QuizImageLink { get; set; }
        public int Time { get; set; }
        public string TopicId { get; set; }
        public Questionpool Questionpool { get; set; }
        [Required]
        public int QuestionpoolId { get; set; }
        public ICollection<Choice> Choices { get; set; }
    }
    public enum QuestionType
    {
        MultipleChoice
    }
}
