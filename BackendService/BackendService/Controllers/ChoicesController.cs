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
    public class ChoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Choices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Choice>>> GetChoices()
        {
            return await _context.Choices.ToListAsync();
        }

        // GET: api/Choices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Choice>> GetChoice(int id)
        {
            var choice = await _context.Choices.FindAsync(id);

            if (choice == null)
            {
                return NotFound();
            }

            return choice;
        }

        // PUT: api/Choices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChoice(int id, Choice choice)
        {
            if (!ChoiceExists(choice.QuizId, choice.Answer))
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    choice.AnswerImage = FileRequestHandle.ConvertToByteArray(HttpContext.Request.Form.Files[0]);
                }
                if (id != choice.ChoiceId)
                {
                    return BadRequest();
                }

                _context.Entry(choice).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoiceExists(id))
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

        // POST: api/Choices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Choice>> PostChoice(Choice choice)
        {
            if (!ChoiceExists(choice.QuizId, choice.Answer)) {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    choice.AnswerImage = FileRequestHandle.ConvertToByteArray(HttpContext.Request.Form.Files[0]);
                }
                _context.Choices.Add(choice);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetChoice", new { id = choice.ChoiceId }, choice);
            }
            return null;
        }

        // DELETE: api/Choices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChoice(int id)
        {
            var choice = await _context.Choices.FindAsync(id);
            if (choice == null)
            {
                return NotFound();
            }

            _context.Choices.Remove(choice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChoiceExists(int id)
        {
            return _context.Choices.Any(e => e.ChoiceId == id);
        }
        private bool ChoiceExists(int quizId, string answer)
        {
            return _context.Choices.Any(x => x.QuizId == quizId && x.Answer == answer);
        }
    }
}
