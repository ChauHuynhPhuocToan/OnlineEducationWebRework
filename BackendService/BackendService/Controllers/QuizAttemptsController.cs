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
    public class QuizAttemptsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuizAttemptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/QuizAttempts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizAttempt>>> GetQuizAttempts()
        {
            return await _context.QuizAttempts.ToListAsync();
        }

        // GET: api/QuizAttempts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizAttempt>> GetQuizAttempt(int id)
        {
            var quizAttempt = await _context.QuizAttempts.FindAsync(id);

            if (quizAttempt == null)
            {
                return NotFound();
            }

            return quizAttempt;
        }

        // PUT: api/QuizAttempts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizAttempt(int id, QuizAttempt quizAttempt)
        {
            if (id != quizAttempt.QuizAttemptID)
            {
                return BadRequest();
            }

            _context.Entry(quizAttempt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizAttemptExists(id))
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

        // POST: api/QuizAttempts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizAttempt>> PostQuizAttempt(QuizAttempt quizAttempt)
        {
            _context.QuizAttempts.Add(quizAttempt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizAttempt", new { id = quizAttempt.QuizAttemptID }, quizAttempt);
        }

        // DELETE: api/QuizAttempts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizAttempt(int id)
        {
            var quizAttempt = await _context.QuizAttempts.FindAsync(id);
            if (quizAttempt == null)
            {
                return NotFound();
            }

            _context.QuizAttempts.Remove(quizAttempt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizAttemptExists(int id)
        {
            return _context.QuizAttempts.Any(e => e.QuizAttemptID == id);
        }
        [HttpGet]
        // GET: api/GetExamQuizHistory?quizCode=1
        [Route("GetExamQuizHistory")]
        public async Task<ActionResult<IEnumerable<ExamHistory>>> GetExamQuizHistory(string quizCode)
        {
            var result = await _context.QuizAttempts.Where(a => a.ExamQuizCode == quizCode).ToListAsync();
            var accountResult = await _context.Accounts.ToListAsync();
            List<ExamHistory> examHistories = new List<ExamHistory>();
            result.ForEach(e =>
            {
                var examHistory = new ExamHistory();
                examHistory.QuizAttempts = e;
                examHistory.Username = accountResult.FirstOrDefault(acc => acc.AccountId.ToString() == e.AccountId).Username;
                examHistories.Add(examHistory);
            });
            return examHistories;
        }
        [HttpGet]
        // GET: api/GetExamQuizAttempByAccountId?accountId=1
        [Route("GetExamQuizAttempByAccountId")]
        public async Task<ActionResult<IEnumerable<QuizAttempt>>> GetExamQuizAttempByAccountId(string accountId)
        {
            var result = await _context.QuizAttempts.Where(a => a.AccountId == accountId).ToListAsync();
            //if (result.Count > 0)
            //{
            //    List<QuizAttempt> list = new List<QuizAttempt>();
            //    var quizCodeCheck = result[0].ExamQuizCode;
            //    list.Add(result[0]);
            //    result.ForEach(e =>
            //    {
            //        if (e.ExamQuizCode != quizCodeCheck)
            //        {
            //            list.Add(e);
            //            quizCodeCheck = e.ExamQuizCode;
            //        }
            //    });
            //    return list;
            //}
            return result;
        }
    }
}
