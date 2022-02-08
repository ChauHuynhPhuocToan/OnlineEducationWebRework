using BackendService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackendService.Controllers.Custom
{
    public enum OptionType
    {
        IsBought,
        GetPayment
    }
    public class InstructorProfile
    {
        public double ReviewerCounter { get; set; }
        public int CourseCounter { get; set; }
        public double StudentCounter { get; set; }
        public string InstructorName { get; set; }
        public string Description { get; set; }
        public string AvatarPath { get; set; }
    }
    public class FullCertificateInfo
    {
        public List<String> CourseName { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public List<String> GetDate { get; set; }
    }
    public class CommentData
    {
        public int CommentId { get; set; }
        public string CommentContext { get; set; }
        public string DatePost { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Byte[] AvatarPath { get; set; }

    }
    public class CourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public float Rating { get; set; }
        public double NumberOfVoters { get; set; }
        public double NumberOfParticipants { get; set; }
        public float Price { get; set; }
        public DateTime StartDate { get; set; }
        public string CourseDuration { get; set; }
        public string Description { get; set; }
        public byte[] ThumbnailImage { get; set; }
        public string Hastag { get; set; }
        public string Level { get; set; }
        public DateTime LastUpdate { get; set; }
        public int LessonNumber { get; set; }
        public Boolean IsActive { get; set; }
        public int ViewCount { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public string RenderDescripton { get; set; }
        public double RenderRating { get; set; }
        public string RenderImagePath { get; set; }
        public Boolean CheckFlagRatingRender { get; set; }
        const int CharacterLimit = 20;
        public CourseDTO()
        {
        }
        public void GetRenderDescription()
        {
            this.RenderDescripton = this.Description.Substring(0, CharacterLimit) + "...";
        }
        public void GetRenderRating()
        {
            this.RenderRating = Math.Floor(this.Rating);
            this.CheckFlagRatingRender = ((this.Rating - this.RenderRating) > 0.5) ? true : false;
        }
        public string GetImageMime()
        {
            switch (this.ThumbnailImage[0].ToString())
            {
                case "/": return "jpg";
                case "R": return "gif";
                case "i": return "png";
                default: return "jpeg";
            }
        }
        public void GetImageSource()
        {
            if (this.ThumbnailImage.Length == 0)
            {
                this.RenderImagePath = "assets/img/productplaceholder.jpg";
                return;
            }
            this.RenderImagePath = $"data:image/{GetImageMime()}; base64," + Convert.ToBase64String(this.ThumbnailImage);
        }
    }
    public class CourseData
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public float Rating { get; set; }
        public double NumberOfRatings { get; set; }
        public double NumberOfParticipants { get; set; }
        public float Price { get; set; }
        public string StartDate { get; set; }
        public string CourseDuration { get; set; }
        public string Description { get; set; }
        public string RenderDescripton { get; set; }
        public double RenderRating { get; set; }
        public string ThumbnailImage { get; set; }
        public CourseHastag Hastag { get; set; }
        public CourseLevel Level { get; set; }
        public int NumberOfView { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public Boolean CheckFlagRatingRender { get; set; }
        const int CharacterLimit = 20;
        public void GetRenderDescription()
        {
            this.RenderDescripton = this.Description.Substring(0, CharacterLimit) + "...";
        }
        public void GetRenderRating()
        {
            this.RenderRating = Math.Floor(this.Rating);
            this.CheckFlagRatingRender = ((this.Rating - this.RenderRating) > 0.5) ? true : false;
        }
    }
    public enum TopCourseSelectOption
    {
        FreeCourse,
        HightView
    }
    class CourseFullData
    {
        public Course Course { get; set; }
        public List<TopicData> TopicList { get; set; }
    }
    class TopicData
    {
        public Topic Topic { get; set; }
        public List<SubtopicData> SubtopicList { get; set; }
    }
    class SubtopicData
    {
        public SubTopic Subtopic { get; set; }
        public List<Lesson> LessonList { get; set; }
    }
    public enum SearchCourseOption
    {
        IsBought,
        PublishCourse
    }
    public enum GetCourseListOption
    {
        GetTopCourse,
        GetAllCourseList
    }
    public class ChartData
    {
        public List<string> HasTag { get; set; }
        public List<double> Rate { get; set; }
        public ChartData()
        {
            this.HasTag = new List<string>(new string[] { "C", "C#", "C++", "Java", "Html/css", "Python", "IOS-Android", "AI", "Javascript", "Machine Learning", "UX/UI", "Framework", "Orther" });
            this.Rate = new List<double>();
        }
    }
    public enum GetLessonOption
    {
        NotBoughtYet,
        AlreadyBought
    }
    public enum DateSortOption
    {
        Increase,
        Decrease
    }
    public class QuizAndAnswer
    {
        public int QuizId { get; set; }
        public string Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public Byte[] QuizImage { get; set; }
        public string QuizImageLink { get; set; }
        public string TopicId { get; set; }
        public int Time { get; set; }
        public string Description { get; set; }
        public int QuestionpoolId { get; set; }
        public ICollection<Choice> Choices { get; set; }
    }
    public class FileRequestHandle
    {
        public static byte[] ConvertToByteArray(IFormFile file)
        {
            byte[] fileData = null;

            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                return fileData = binaryReader.ReadBytes((int)file.Length);
            }
        }
        public static string GetImageMime(byte[] imagePath)
        {
            switch (imagePath[0].ToString())
            {
                case "/": return "jpg";
                case "R": return "gif";
                case "i": return "png";
                default: return "jpeg";
            }
        }
        public static string GetImageSource(byte[] imagePath)
        {
            if (imagePath.Length == 0)
            {
                return "assets/img/productplaceholder.jpg";
            }
            return $"data:image/{GetImageMime(imagePath)}; base64,{Convert.ToBase64String(imagePath)}";
        }
    }
    public class ExamHistory
    {
        public QuizAttempt QuizAttempts { get; set; }
        public string Username { get; set; }
    }

}
