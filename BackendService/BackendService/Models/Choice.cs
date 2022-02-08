using BackendService.AppConsts;
using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Choice
    {
        [Key]
        public int ChoiceId { get; set; }
        [StringLength(PropertyConst.AnswerMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.AnswerMinLength)]
        public string Answer { get; set; }
        public Boolean IsCorrect { get; set; }
        public Byte[] AnswerImage { get; set; }
        public string AnswerImageLink { get; set; }
        public Quiz Quiz { get; set; }
        [Required]
        public int QuizId { get; set; }
    }
}
