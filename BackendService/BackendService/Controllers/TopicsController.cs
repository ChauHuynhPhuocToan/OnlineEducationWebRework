using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Models;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TopicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Topics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopic(int id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound();
            }

            return topic;
        }

        // PUT: api/Topics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic(int id, Topic topic)
        {
            if (id != topic.TopicId)
            {
                return BadRequest();
            }

            if(!TopicExists(topic.CourseId, topic.TopicTitle))
            {
                _context.Entry(topic).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(id))
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

        // POST: api/Topics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Topic>> PostTopic(Topic topic)
        {
            if (!TopicExists(topic.CourseId, topic.TopicTitle))
            {
                _context.Topics.Add(topic);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTopic", new { id = topic.TopicId }, topic);
            }
            return null;
        }

        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            var subtopicList = await _context.SubTopics.Where(x => x.TopicId == id).ToListAsync();
            if (subtopicList != null)
            {
                var lessonListDB = await _context.Lessons.ToListAsync();
                var lessonList = new List<Lesson>();
                subtopicList.ForEach(x =>
                {
                    var lessonFind = lessonListDB.Where(e => e.SubTopicId == x.SubTopicId).ToList();
                    if (lessonFind != null) lessonList.AddRange(lessonFind);
                });
                _context.RemoveRange(subtopicList);
                _context.RemoveRange(lessonList);
            }


            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.TopicId == id);
        }

        private bool TopicExists(int courseID, string topicTitle)
        {
            return _context.Topics.Any(x => x.TopicTitle == topicTitle && x.CourseId == courseID);
        }
        // GET: api/Topics/TopicCount?id=1
        [HttpGet]
        [Route("TopicCount")]
        public async Task<ActionResult> GetNumberOfTopic(int id)
        {
            var list = await _context.Topics.Where(x => x.CourseId == id).ToListAsync();
            return Content(list.Count.ToString());
        }
        // GET: api/Topics/GetTopicByCourseId?id=1
        [HttpGet]
        [Route("GetTopicByCourseId")]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopicByCourseId(int id)
        {
            return await _context.Topics.Where(x => x.CourseId == id).ToListAsync();
        }
    }
}
