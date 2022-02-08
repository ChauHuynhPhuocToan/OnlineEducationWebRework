﻿using System;

namespace BackendService.Models
{
    public class LessonComplete
    {
        public int LessonCompleteId { get; set; }
        public string LessonId { get; set; }
        public string AccountId { get; set; }
        public DateTime CompleteDate { get; set; }
        public string CourseId { get; set; }
    }
}
