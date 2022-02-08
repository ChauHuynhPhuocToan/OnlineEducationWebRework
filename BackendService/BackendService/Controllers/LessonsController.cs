using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Models;
using BackendService.Controllers.Custom;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons()
        {
            return await _context.Lessons.ToListAsync();
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
        }

        // PUT: api/Lessons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLesson(int id, Lesson lesson)
        {
            if (id != lesson.LessonId)
            {
                return BadRequest();
            }

            _context.Entry(lesson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(id))
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

        // POST: api/Lessons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lesson>> PostLesson(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLesson", new { id = lesson.LessonId }, lesson);
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonId == id);
        }
        // GET: api/Lessons/GetLessonByCourseId?option=1&id=1
        [HttpGet]
        [Route("GetLessonByCourseId")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessonByCourseId(GetLessonOption option,int id)
        {
            List<Topic> topicList = new List<Topic>();
            var subtopicList = await _context.SubTopics.ToListAsync();
            var lessonList = await _context.Lessons.ToListAsync();
            switch (option)
            {
                case GetLessonOption.AlreadyBought:
                    topicList = await _context.Topics.Where(x => x.CourseId == id).ToListAsync();
                    break;
                default:
                    topicList = await _context.Topics.Where(x => x.CourseId == id && !x.IsLocked).ToListAsync();
                    break;
            }
            var lessonListResult = new List<Lesson>();
            topicList.ForEach(x => {
                var subtopicFind = subtopicList.Where(e => e.TopicId == x.TopicId).ToList();
                if(subtopicFind != null)
                {
                    subtopicFind.ForEach(i =>
                    {
                        var lessonFind = lessonList.Where(l => l.SubTopicId == i.SubTopicId).ToList();
                        if (lessonFind != null)
                        {
                            lessonListResult.AddRange(lessonFind);
                        }
                    });
                }
            });
            return lessonListResult;
        }
        // GET: api/Lessons/GetLessonBySubtopicId?option=1&id=1
        [HttpGet]
        [Route("GetLessonBySubtopicId")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessonBySubtopicId(int id)
        {
            return await _context.Lessons.Where(x => x.SubTopicId == id).ToListAsync();
        }
    }
}
