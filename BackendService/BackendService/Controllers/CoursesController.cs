using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Models;
using BackendService.Controllers.Custom;
using AutoMapper;
using BackendService.AppConsts;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CoursesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.Where(x => x.IsActive).OrderByDescending(x => x.CourseId).ToListAsync();
        }
        // GET: api/Courses/AllCourses
        [HttpGet]
        [Route("AllCourses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesAll()
        {
            return await _context.Courses.OrderByDescending(x => x.CourseId).ToListAsync();
        }
        // GET: api/Courses/GetTopCourses?numberOfCourse=1
        [HttpGet]
        [Route("GetTopCourses")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetTopCertifications(int numberOfCourse)
        {
            var data = await _context.Courses.Where(x => x.IsActive).OrderByDescending(x => x.Rating)
            .ThenByDescending(x => x.NumberOfRatings)
            .ThenByDescending(x => x.CourseId)
            .Take(numberOfCourse).ToListAsync();
            List<CourseDTO> resultList = _mapper.Map<List<Course>, List<CourseDTO>>(data);
            resultList.ForEach(x =>
            {
                x.GetRenderDescription();
                x.GetRenderRating();
                x.GetImageSource();
            });
            return resultList;
        }
        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (!CourseExists(course.CourseId, course.CourseName))
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    course.ThumbnailImage = FileRequestHandle.ConvertToByteArray(HttpContext.Request.Form.Files[0]);
                }
                if (id != course.CourseId)
                {
                    return BadRequest();
                }

                _context.Entry(course).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            return null;
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (!CourseExists(course.CourseName))
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    course.ThumbnailImage = FileRequestHandle.ConvertToByteArray(HttpContext.Request.Form.Files[0]);
                }
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
            }
            return null;
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var topicList = await _context.Topics.Where(e => e.CourseId == id).ToListAsync();
            if (topicList != null)
            {
                var subtopicDB = await _context.SubTopics.ToListAsync();
                var subtopicList = new List<SubTopic>();
                topicList.ForEach(x =>
                {
                    var subtopicFindResult = subtopicDB.Where(s => s.TopicId == x.TopicId);
                    if (subtopicFindResult != null)
                    {
                        subtopicList.AddRange(subtopicFindResult);
                    }
                });
                if (subtopicList != null)
                {
                    var lessonDB = await _context.Lessons.ToListAsync();
                    var lessonList = new List<Lesson>();
                    subtopicList.ForEach(x =>
                    {
                        var lessonFindResult = lessonDB.Where(lesson => lesson.SubTopicId == x.SubTopicId);
                        if (lessonFindResult != null)
                        {
                            lessonList.AddRange(lessonFindResult);
                        }
                    });
                    if(lessonList != null)
                    {
                        _context.Lessons.RemoveRange(lessonList);
                    }
                    _context.SubTopics.RemoveRange(subtopicList);
                }
                _context.Topics.RemoveRange(topicList);
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
        private bool CourseExists(string courseName)
        {
            return _context.Courses.Any(x => x.CourseName == courseName);
        }
        private bool CourseExists(int id, string courseName)
        {
            return _context.Courses.Any(x => x.CourseId != id && x.CourseName == courseName);
        }
        // GET: api/Courses/GetCoursesByAccountId?id=1
        [HttpGet]
        [Route("GetCoursesByAccountId")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByAccountId(int id)
        {
            return await _context.Courses.Where(x => x.IsActive && x.AccountId == id).ToListAsync();
        }
        // PUT: api/Courses/ViewCount?id=1
        [HttpPut]
        [Route("ViewCount")]
        public async Task<IActionResult> PutCourseCount(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            course.NumberOfView++;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // GET: api/Courses/GetNumberTopCourse?option=1&numberOfCourse=1
        [HttpGet]
        [Route("GetNumberTopCourse")]
        public async Task<ActionResult<IEnumerable<CourseData>>> GetNumberTopCourse(TopCourseSelectOption option, int numberOfCourse)
        {
            var account = await _context.Accounts.ToListAsync();
            var user = await _context.Users.ToListAsync();
            var course = (option == TopCourseSelectOption.FreeCourse) 
                ? await _context.Courses.Where(x => x.Price == 0 && x.IsActive && x.Account.IsActive)
                        .OrderByDescending(x => x.CourseId)
                        .Take(numberOfCourse).ToListAsync() 
                : await _context.Courses.Where(x => x.IsActive && x.Account.IsActive).OrderByDescending(x => x.NumberOfView)
                  .ThenByDescending(x => x.Rating)
                  .ThenByDescending(x => x.CourseId)
                  .Take(numberOfCourse)
                  .ToListAsync();
            List<CourseData> courseData = new List<CourseData>();
            course.ForEach(x =>
            {
                var userId = account.FirstOrDefault(a => a.AccountId == x.AccountId).UserId;
                var userInfo = user.FirstOrDefault(u => u.UserId == userId);
                var data = new CourseData() {
                    CourseId = x.CourseId,
                    CourseName = x.CourseName,
                    Rating = x.Rating,
                    NumberOfRatings = x.NumberOfRatings,
                    NumberOfParticipants = x.NumberOfParticipants,
                    Price = x.Price,
                    StartDate = x.StartDate.ToString(PropertyConst.DatetimeFormat),
                    CourseDuration = x.CourseDuration,
                    Description = x.Description,
                    ThumbnailImage = FileRequestHandle.GetImageSource(x.ThumbnailImage),
                    Hastag = x.Hastag,
                    Level = x.Level,
                    NumberOfView = x.NumberOfView,
                    AccountId = x.AccountId,
                    UserId = userId,
                    UserName = $"{userInfo.FirstName} {userInfo.LastName}",
                    AvatarPath = FileRequestHandle.GetImageSource(userInfo.AvatarPath)
                };
                data.GetRenderDescription();
                data.GetRenderRating();
                courseData.Add(data);
            });
            return courseData;
        }
        // GET: api/Courses/GetFullCourseData?id=1
        [HttpGet]
        [Route("GetFullCourseData")]
        public ActionResult GetFullCourseData(int id)
        {
            var course = _context.Courses.FirstOrDefault(x => x.CourseId == id);
            var courseData = new CourseFullData() { 
                Course = course,
                TopicList = new List<TopicData>()
            };
            var topic = _context.Topics.Where(e => e.CourseId == id).ToList();
            var subtopic = _context.SubTopics.ToList();
            var lesson = _context.Lessons.ToList();
            if (topic != null)
            {
                topic.ForEach(x =>
                {
                    var topicData = new TopicData() {
                        Topic = x,
                        SubtopicList = new List<SubtopicData>()
                    };
                    var subtopicFindResult = subtopic.Where(s => s.TopicId == x.TopicId).ToList();
                    if (subtopicFindResult != null)
                    {
                        subtopicFindResult.ForEach(e =>
                        {
                            var subtopicData = new SubtopicData()
                            {
                                Subtopic = e,
                                LessonList = new List<Lesson>()
                            };
                            var lessonFindResult = lesson.Where(l => l.SubTopicId == e.SubTopicId).ToList();
                            if(lessonFindResult != null)
                            {
                                subtopicData.LessonList.AddRange(lessonFindResult);
                            }
                            topicData.SubtopicList.Add(subtopicData);
                        });
                    }
                    courseData.TopicList.Add(topicData);
                });
            }
            return Ok(courseData);
        }
        // GET: api/Courses/SearchCourse?courseName=abc&option=1&accountId=1
        [HttpGet]
        [Route("SearchCourse")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourseSearch(string courseName, SearchCourseOption option, int accountId)
        {
            List<Course> courseList = new List<Course>();
            switch (option)
            {
                case SearchCourseOption.IsBought:
                    var mycourse = _context.AccountInventories.Where(x => x.AccountId == accountId && x.IsBought).Select(x => x.CourseId).ToList();
                    courseList = await _context.Courses.Where(x => x.CourseName.ToLower().Contains(courseName.ToLower()) && x.IsActive && mycourse.Any(e => e == x.CourseId)).ToListAsync();
                    break;
                case SearchCourseOption.PublishCourse:
                    courseList = await _context.Courses.Where(x => x.CourseName.ToLower().Contains(courseName.ToLower()) && x.AccountId == accountId && x.IsActive).ToListAsync();
                    break;
                default:
                    courseList = await _context.Courses.Where(x => x.CourseName.ToLower().Contains(courseName.ToLower()) && x.IsActive).ToListAsync();
                    break;
            }
            if (courseList == null)
            {
                return NotFound();
            }
            return courseList;
        }
        // GET: api/Courses/FilterCourse?hastag=1&courseName=abc&maxPrice=0&minPrice=0&level=0
        [HttpGet]
        [Route("FilterCourse")]

        public async Task<ActionResult<IEnumerable<Course>>> FilterCourse(int hastag = -1, string? courseName = null, int? maxPrice = null, int? minPrice = null, int? level = null)
        {
            return await _context.Courses.Where(x => (hastag == -1 || hastag == (int)x.Hastag) 
                                                && (level == null || level == (int)x.Level) 
                                                && (courseName == null || x.CourseName.ToLower().Contains(courseName.ToLower())) 
                                                && (minPrice == null || x.Price >= minPrice) 
                                                && (maxPrice == null || x.Price <= maxPrice) 
                                                && x.IsActive)
              .OrderByDescending(x => x.CourseId)
              .ThenByDescending(x => x.Rating)
              .ThenByDescending(x => x.NumberOfRatings)
              .ThenByDescending(x => x.NumberOfParticipants)
              .ToListAsync();
        }
        // GET: api/Courses/GetInstructorFeatureCourse?id=1&numberOfCourse=1
        [HttpGet]
        [Route("GetInstructorFeatureCourse")]
        public async Task<ActionResult<IEnumerable<Course>>> GetInstructorFeatureCourse(int id, int numberOfCourse)
        {
            return await _context.Courses.Where(e => e.AccountId == id)
              .OrderByDescending(e => e.Rating)
              .ThenByDescending(e => e.NumberOfView)
              .ThenByDescending(e => e.NumberOfParticipants)
              .ThenByDescending(e => e.CourseId)
              .Take(numberOfCourse)
              .ToListAsync();
        }
        // GET: api/Courses/GetChartData
        [HttpGet]
        [Route("GetChartData")]
        public async Task<ActionResult<ChartData>> GetChartData()
        {
            var chartData = new ChartData();
            var courseList = await _context.Courses.ToListAsync();
            int i = 0;
            var totalCourse = courseList.Count();
            var hastagCount = 0;
            chartData.HasTag.ForEach(x =>
            {
                hastagCount = courseList.FindAll(e => e.Hastag == (CourseHastag)i).Count();
                i++;
                chartData.Rate.Add((hastagCount * 100.00 / totalCourse));
            });
            return chartData;
        }
        // GET: api/Courses/GetNumberTopCourseNonBuy?option=1&accountId=1&numberOfCourse=1
        [HttpGet]
        [Route("GetNumberTopCourseNonBuy")]
        public async Task<ActionResult<IEnumerable<CourseData>>> GetTopCourseNonBuy(TopCourseSelectOption option, int accountId, int numberOfCourse)
        {
            var account = await _context.Accounts.ToListAsync();
            var user = await _context.Users.ToListAsync();
            List<Account> accountList = new List<Account>();
            List<Course> courses = new List<Course>();
            var courseList = await _context.Courses.Where(x => x.IsActive && !CourseReadyBuy(accountId, x.CourseId)).ToListAsync();
            var course = new List<Course>();
            switch (option)
            {
                case TopCourseSelectOption.FreeCourse:
                    course = courses.Where(e => e.Price == 0)
                                    .OrderByDescending(e => e.CourseId)
                                    .Take(numberOfCourse)
                                    .ToList();
                    break;
                case TopCourseSelectOption.HightView:
                    course = courses.OrderByDescending(e => e.NumberOfView)
                           .ThenByDescending(e => e.Rating)
                           .ThenByDescending(e => e.CourseId)
                           .Take(numberOfCourse)
                           .ToList();
                    break;
            }
            List<CourseData> courseData = new List<CourseData>();
            course.ForEach(x =>
            {
                var userId = account.FirstOrDefault(a => a.AccountId == x.AccountId).UserId;
                var userInfo = user.FirstOrDefault(u => u.UserId == userId);
                var data = new CourseData()
                {
                    CourseId = x.CourseId,
                    CourseName = x.CourseName,
                    Rating = x.Rating,
                    NumberOfRatings = x.NumberOfRatings,
                    NumberOfParticipants = x.NumberOfParticipants,
                    Price = x.Price,
                    StartDate = x.StartDate.ToString(PropertyConst.DatetimeFormat),
                    CourseDuration = x.CourseDuration,
                    Description = x.Description,
                    ThumbnailImage = FileRequestHandle.GetImageSource(x.ThumbnailImage),
                    Hastag = x.Hastag,
                    Level = x.Level,
                    NumberOfView = x.NumberOfView,
                    AccountId = x.AccountId,
                    UserId = userId,
                    UserName = $"{userInfo.FirstName} {userInfo.LastName}",
                    AvatarPath = FileRequestHandle.GetImageSource(userInfo.AvatarPath)
                };
                data.GetRenderDescription();
                data.GetRenderRating();
                courseData.Add(data);
            });
            return courseData;
        }
        private bool CourseReadyBuy(int accountId, int courseId)
        {
            return _context.AccountInventories.Any(acc => acc.AccountId == accountId && acc.CourseId == courseId && acc.IsBought);
        }
        // GET: api/Courses/GetCourseListNonBuy?option=1&accountId=1&numberOfCourse=1
        [HttpGet]
        [Route("GetCourseListNonBuy")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesNonBuy(GetCourseListOption option, int accountId, int? numberOfCourse)
        {
            switch (option)
            {
                case GetCourseListOption.GetTopCourse:
                    return await _context.Courses.Where(x => x.IsActive && !CourseReadyBuy(accountId, x.CourseId))
                                                    .OrderByDescending(e => e.Rating)
                                                    .ThenByDescending(e => e.NumberOfRatings)
                                                    .ThenByDescending(e => e.CourseId)
                                                    .Take(numberOfCourse.Value)
                                                    .ToListAsync();
                default:
                    return await _context.Courses.Where(x => x.IsActive && !CourseReadyBuy(accountId, x.CourseId))
                                                    .OrderByDescending(c => c.CourseId)
                                                    .ToListAsync();
            }
        }
    }    
}
