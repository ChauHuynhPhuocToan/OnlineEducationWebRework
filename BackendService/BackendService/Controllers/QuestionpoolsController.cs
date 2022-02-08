using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Models;
using BackendService.Controllers.Custom;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionpoolsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionpoolsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Questionpools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questionpool>>> GetQuestionpools()
        {
            return await _context.Questionpools.ToListAsync();
        }

        // GET: api/Questionpools/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Questionpool>> GetQuestionpool(int id)
        {
            var questionpool = await _context.Questionpools.FindAsync(id);

            if (questionpool == null)
            {
                return NotFound();
            }

            return questionpool;
        }

        // PUT: api/Questionpools/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionpool(int id, Questionpool questionpool)
        {
            if (!QuestionpoolExists(id, questionpool.QuestionpoolName, questionpool.AccountId))
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    questionpool.QuestionpoolThumbnailImage = FileRequestHandle.ConvertToByteArray(HttpContext.Request.Form.Files[0]);
                }
                if (id != questionpool.QuestionpoolId)
                {
                    return BadRequest();
                }

                _context.Entry(questionpool).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionpoolExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return NoContent();
        }

        // POST: api/Questionpools
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Questionpool>> PostQuestionpool(Questionpool questionpool)
        {
            if (!QuestionpoolExists(questionpool.QuestionpoolName, questionpool.AccountId))
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    questionpool.QuestionpoolThumbnailImage = FileRequestHandle.ConvertToByteArray(HttpContext.Request.Form.Files[0]);
                }
                _context.Questionpools.Add(questionpool);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetQuestionpool", new { id = questionpool.QuestionpoolId }, questionpool);
            }
            return null;
        }

        // DELETE: api/Questionpools/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionpool(int id)
        {
            var questionpool = await _context.Questionpools.FindAsync(id);
            if (questionpool == null)
            {
                return NotFound();
            }
            var quizList = await _context.Quizs.Where(e => e.QuestionpoolId == id).ToListAsync();
            if (quizList != null)
            {
                List<Choice> choiceList = new List<Choice>();
                quizList.ForEach(x =>
                {
                    var choiceFindResult = _context.Choices.Where(e => e.QuizId == x.QuizId).ToList();
                    choiceList.AddRange(choiceFindResult);
                });
               _context.Choices.RemoveRange(choiceList);
               _context.Quizs.RemoveRange(quizList);
            }
            _context.Questionpools.Remove(questionpool);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionpoolExists(int id)
        {
            return _context.Questionpools.Any(e => e.QuestionpoolId == id);
        }

        private bool QuestionpoolExists(string questionpoolName, string accountId)
        {
            return _context.Questionpools.Any(x => x.QuestionpoolName == questionpoolName && x.AccountId == accountId);
        }
        private bool QuestionpoolExists(int id, string questionpoolName, string accountId)
        {
            return _context.Questionpools.Any(x => x.QuestionpoolId != id && x.QuestionpoolName == questionpoolName && x.AccountId == accountId);
        }
        // GET: api/Questionpools/GetInstructor?id=1
        [HttpGet("GetInstructor")]
        public async Task<ActionResult<User>> GetInstructor(int id)
        {
            var accountId = _context.Courses.FirstOrDefault(x => x.CourseId == id).AccountId;
            var userId = _context.Accounts.FirstOrDefault(x => x.AccountId == accountId).UserId;
            return _context.Users.FirstOrDefault(x => x.UserId == userId);
        }
        // GET: api/Questionpools/GetQuestionpoolByAccountId?accountId=1
        [HttpGet("GetQuestionpoolByAccountId")]
        public async Task<ActionResult<IEnumerable<Questionpool>>> GetListQuestionpoolByIds(string accountId)
        {
            return await _context.Questionpools.Where(x => x.AccountId == accountId)
              .OrderByDescending(x => x.IsActive)
              .ThenByDescending(x => x.QuestionpoolId)
              .ToListAsync();
        }
        // GET: api/Questionpools/CountQuizOfQuestionpools?id=1
        [HttpGet]
        [Route("CountQuizOfQuestionpools")]
        public async Task<ActionResult> GetNumOfQuiz(int id)
        {
            var list = await _context.Quizs.Where(c => c.QuestionpoolId == id).ToListAsync();
            return Content(list.Count.ToString());
        }
        // PUT: api/Questionpools/DeActiveQuestionpool?id=1
        [HttpPut]
        [Route("DeActiveQuestionpool")]
        public async Task<ActionResult> DeActiveQuestionpool(int id)
        {
            var result = await _context.Questionpools.FindAsync(id);
            result.IsActive = !result.IsActive;
            _context.SaveChanges();
            return NoContent();
        }
        // GET: api/Questionpools/GetListActiveQuestionpool?accountId=1
        [HttpGet]
        [Route("GetListActiveQuestionpool")]
        public async Task<ActionResult<IEnumerable<Questionpool>>> GetListActiveQuestionpool(string accountId)
        {
            return await _context.Questionpools.Where(x => x.IsActive && x.AccountId == accountId)
                .OrderByDescending(e => e.QuestionpoolId)
                .ToListAsync();
        }
        // GET: api/Questionpools/FilterQuestionpool?option=1&hastag=1&questionpoolName=abc&accountId=1
        [HttpGet]
        [Route("FilterQuestionpool")]
        public async Task<ActionResult<IEnumerable<Questionpool>>> FilterQuestionpool(DateSortOption option, CourseHastag hastag, string? questionpoolName = null, string? accountId = null)
        {
            switch (option)
            {
                case DateSortOption.Decrease:
                    return await _context.Questionpools.Where(e => e.AccountId == accountId
                                                                    && (!Enum.IsDefined(typeof(CourseHastag), hastag) || e.Hastag == hastag)
                                                                    && (questionpoolName == null || e.QuestionpoolName.ToLower().Contains(questionpoolName.ToLower())))
                                                        .OrderByDescending(e => e.QuestionpoolId)
                                                        .ToListAsync();
                default:
                    return await _context.Questionpools.Where(e => e.AccountId == accountId 
                                                                && (!Enum.IsDefined(typeof(CourseHastag), hastag) || e.Hastag == hastag) 
                                                                && (questionpoolName == null || e.QuestionpoolName.ToLower().Contains(questionpoolName.ToLower())))
                                                        .OrderBy(e => e.QuestionpoolId)
                                                        .ToListAsync();

            }
        }
    }
}
