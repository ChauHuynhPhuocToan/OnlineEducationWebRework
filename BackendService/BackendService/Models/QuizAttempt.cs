using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class QuizAttempt
    {
        [Key]
        public int QuizAttemptID { get; set; }
        public string AccountId { get; set; }
        public string ExamQuizCode { get; set; }
        public Boolean IsCompleted { get; set; }
        public string Result { get; set; }
        public DateTime LastTaken { get; set; }
    }
}
