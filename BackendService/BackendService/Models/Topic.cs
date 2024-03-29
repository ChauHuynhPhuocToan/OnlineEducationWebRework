﻿using BackendService.AppConsts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        [StringLength(PropertyConst.TopicTitleMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.TopicTitleMinLength)]
        public string TopicTitle { get; set; }
        public int SessionNumber { get; set; }
        public DateTime LastUpdate { get; set; }
        public Boolean IsLocked { get; set; }
        public Course Course { get; set; }
        [Required]
        public int CourseId { get; set; }
        public ICollection<SubTopic> SubTopics { get; set; }
    }
}
