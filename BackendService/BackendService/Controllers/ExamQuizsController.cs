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
    public class ExamQuizsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExamQuizsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ExamQuizs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamQuiz>>> GetExamQuizs()
        {
            return await _context.ExamQuizs.ToListAsync();
        }

        // GET: api/ExamQuizs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamQuiz>> GetExamQuiz(int id)
        {
            var examQuiz = await _context.ExamQuizs.FindAsync(id);

            if (examQuiz == null)
            {
                return NotFound();
            }

            return examQuiz;
        }

        // PUT: api/ExamQuizs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamQuiz(int id, ExamQuiz examQuiz)
        {
            if (id != examQuiz.ExamQuizId)
            {
                return BadRequest();
            }

            _context.Entry(examQuiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamQuizExists(id))
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

        // POST: api/ExamQuizs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExamQuiz>> PostExamQuiz(ExamQuiz examQuiz)
        {
            if (!ExamQuizCheckExists(examQuiz.QuizId, examQuiz.ExamQuizCode))
            {
                _context.ExamQuizs.Add(examQuiz);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetExamQuiz", new { id = examQuiz.ExamQuizId }, examQuiz);
            }
            return null;
        }

        // DELETE: api/ExamQuizs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamQuiz(int id)
        {
            var examQuiz = await _context.ExamQuizs.FindAsync(id);
            if (examQuiz == null)
            {
                return NotFound();
            }

            _context.ExamQuizs.Remove(examQuiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamQuizExists(int id)
        {
            return _context.ExamQuizs.Any(x => x.ExamQuizId == id);
        }
        private bool ExamQuizCheckExists(string id, string examCode)
        {
            return _context.ExamQuizs.Any(e => e.QuizId == id && e.ExamQuizCode == examCode);
        }
        // GET: api/ExamQuizs/GetExamQuizByCourseId?id=1
        [HttpGet]
        [Route("GetExamQuizByCourseId")]
        public async Task<ActionResult<IEnumerable<ExamQuiz>>> GetExamQuizByCourseId(string id)
        {
            return await _context.ExamQuizs.Where(e => e.CourseId == id)
              .OrderBy(e => e.ExamQuizCode)
              .ThenBy(e => e.ExamQuizId)
              .ToListAsync();
        }
        // GET: api/ExamQuizs/GetExamQuizListByExamCode?examcode=1
        [HttpGet]
        [Route("GetExamQuizListByExamCode")]
        public async Task<ActionResult<IEnumerable<ExamQuiz>>> GetExamQuizListByExamCode(string examcode)
        {
            return await _context.ExamQuizs.Where(e => e.ExamQuizCode == examcode)
              .OrderByDescending(e => e.ExamQuizId)
              .ToListAsync();
        }
        // PUT: api/ExamQuizs/ChangeExamquizState?examQuizCode=1
        [HttpPut]
        [Route("ChangeExamquizState")]
        public async Task<IActionResult> ChangeExamquizState(string examQuizCode)
        {
            var examQuizList = await _context.ExamQuizs.Where(e => e.ExamQuizCode == examQuizCode).ToListAsync();
            examQuizList.ForEach(e =>
            {
                e.IsBlocked = !e.IsBlocked;
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // GET: api/ExamQuizs/GetFinalExamQuizList?examcode=1&courseId
        [HttpGet]
        [Route("GetFinalExamQuizList")]
        public async Task<ActionResult<IEnumerable<ExamQuiz>>> GetFinalExamQuizList(string examCode, string courseId)
        {
            return await _context.ExamQuizs.Where(e => e.ExamQuizCode != examCode && e.CourseId == courseId && e.IsFinalQuiz).ToListAsync();
        }
    }
}
