using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class SubComment
    {
        [Key]
        public int SubCommentId { get; set; }
        public string SubCommentContext { get; set; }
        public DateTime SubDatePost { get; set; }
        public int LikeCounter { get; set; }
        public Comment ParentComment { get; set; }
        public int? ParentCommentId { get; set; }
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
