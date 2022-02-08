using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Models;
using System.IO;
using BackendService.Controllers.Custom;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuizsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Quizs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizs()
        {
            return await _context.Quizs.ToListAsync();
        }

        // GET: api/Quizs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
            var quiz = await _context.Quizs.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        // PUT: api/Quizs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, Quiz quiz)
        {
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var file = HttpContext.Request.Form.Files[0];

                byte[] fileData = null;

                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    fileData = binaryReader.ReadBytes((int)file.Length);
                }

                quiz.QuizImage = fileData;
            }
            if (id != quiz.QuizId)
            {
                return BadRequest();
            }

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        // POST: api/Quizs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(Quiz quiz)
        {
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var file = HttpContext.Request.Form.Files[0];

                byte[] fileData = null;

                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    fileData = binaryReader.ReadBytes((int)file.Length);
                }

                quiz.QuizImage = fileData;
            }
            if (!QuizExists(quiz.Question, quiz.QuestionpoolId, quiz.QuizId))
            {
                _context.Quizs.Add(quiz);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetQuiz", new { id = quiz.QuizId }, quiz);
            }
            return null;
        }

        // DELETE: api/Quizs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _context.Quizs.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }
            var choices = await _context.Choices.Where(c => c.QuizId == quiz.QuizId).ToListAsync();
            _context.Choices.RemoveRange(choices);
            _context.Quizs.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(int id)
        {
            return _context.Quizs.Any(e => e.QuizId == id);
        }

        private bool QuizExists(string question, int id, int quizId)
        {
            return _context.Quizs.Any(e => e.Question == question && e.QuestionpoolId == id && e.QuizId != quizId);
        }
        // GET: api/Quizs/LastQuiz
        [HttpGet]
        [Route("LastQuiz")]
        public async Task<ActionResult<Quiz>> GetLastQuiz()
        {
            return await _context.Quizs.OrderByDescending(q => q.QuizId).FirstAsync();
        }
        // GET: api/Quizs/GetQuizInQuestionpool?id=1
        [HttpGet]
        [Route("GetQuizInQuestionpool")]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizInQuestionpool(int id)
        {
            return await _context.Quizs.Where(x => x.QuestionpoolId == id).ToListAsync();
        }
        // GET: api/Quizs/QuizOfQuestionpoolCount?id=1
        [HttpGet]
        [Route("QuizOfQuestionpoolCount")]
        public async Task<ActionResult> GetNumOfQuiz(int id)
        {
            var list = await _context.Quizs.Where(x => x.QuestionpoolId == id).ToListAsync();
            return Content(list.Count.ToString());
        }
        // GET: api/Quizs/QuizOfQuestionpool?id=1
        [HttpGet]
        [Route("QuizOfQuestionpool")]
        public ActionResult GetQuizsOfQuestionpool(int id)
        {
            List<Quiz> quizList = _context.Quizs.Where(x => x.QuestionpoolId == id).ToList();
            List<Choice> answerList = _context.Choices.ToList();
            var querry = from quiz in quizList
                         join answer in answerList on quiz.QuizId equals answer.QuizId
                         select new QuizAndAnswer
                         {
                             QuizId = quiz.QuizId,
                             Question = quiz.Question,
                             QuestionType = quiz.QuestionType,
                             QuizImage = quiz.QuizImage,
                             QuizImageLink = quiz.QuizImageLink,
                             TopicId = quiz.TopicId,
                             Time = quiz.Time,
                             QuestionpoolId = quiz.QuestionpoolId,
                             Choices = answer.Quiz.Choices
                         };
            if (querry != null)
            {
                return Ok(querry);
            }
            return NoContent();
        }
    }
}
