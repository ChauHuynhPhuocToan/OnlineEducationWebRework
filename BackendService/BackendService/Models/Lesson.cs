using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        [StringLength(PropertyConst.LessonTitleMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.LessonTitleMinLength)]
        public string LessonTitle { get; set; }
        public string VideoURL { get; set; }
        public int LessonNo { get; set; }
        public SubTopic SubTopic { get; set; }
        [Required]
        public int SubTopicId { get; set; }
        public string VideoQuizTime { get; set; }
        public string QuizId { get; set; }
        public DateTime LastUpdate { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
}
