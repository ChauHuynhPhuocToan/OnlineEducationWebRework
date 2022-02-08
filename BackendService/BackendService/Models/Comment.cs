using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string CommentContext { get; set; }
        public float Rating { get; set; }
        public DateTime DatePost { get; set; }
        public CommentType Type { get; set; }
        public int LikeCounter { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }
        public Course Course { get; set; }
        public int? CourseId { get; set; }
        public string? LessonId { get; set; }
    }
    public enum CommentType
    {
        Comment,
        Rating
    }
}
