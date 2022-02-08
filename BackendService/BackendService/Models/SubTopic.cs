using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class SubTopic
    {
        [Key]
        public int SubTopicId { get; set; }
        [StringLength(PropertyConst.SubtopicTitleMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.SubtopicTitleMinLength)]
        public string SubTopicTitle { get; set; }
        public int SubTopicNumber { get; set; }
        public Topic Topic { get; set; }
        [Required]
        public int TopicId { get; set; }
        public DateTime LastUpdate { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
