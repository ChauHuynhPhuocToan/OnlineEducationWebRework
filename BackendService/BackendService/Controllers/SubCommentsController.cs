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
    public class SubCommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SubComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubComment>>> GetSubComments()
        {
            return await _context.SubComments.ToListAsync();
        }

        // GET: api/SubComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubComment>> GetSubComment(int id)
        {
            var subComment = await _context.SubComments.FindAsync(id);

            if (subComment == null)
            {
                return NotFound();
            }

            return subComment;
        }

        // PUT: api/SubComments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubComment(int id, SubComment subComment)
        {
            if (id != subComment.SubCommentId)
            {
                return BadRequest();
            }

            _context.Entry(subComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCommentExists(id))
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

        // POST: api/SubComments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubComment>> PostSubComment(SubComment subComment)
        {
            _context.SubComments.Add(subComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubComment", new { id = subComment.SubCommentId }, subComment);
        }

        // DELETE: api/SubComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubComment(int id)
        {
            var subComment = await _context.SubComments.FindAsync(id);
            if (subComment == null)
            {
                return NotFound();
            }

            _context.SubComments.Remove(subComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubCommentExists(int id)
        {
            return _context.SubComments.Any(e => e.SubCommentId == id);
        }
        // GET: api/SubComments/GetSubCommentByParentId?id=1
        [HttpGet]
        [Route("GetSubCommentByParentId")]
        public async Task<ActionResult<IEnumerable<SubComment>>> GetSubCommentByParentId(int id)
        {
            var subComment = await _context.SubComments.Where(x => x.ParentCommentId == id).OrderBy(x => x.SubCommentId).ToListAsync();
            if (subComment != null)
            {
                var userList = await _context.Users.ToListAsync();
                subComment.ForEach(x => {
                    var user = userList.FirstOrDefault(u => u.UserId == x.UserId);
                    x.User = (user != null) ? user : null;
                });
            }
            return subComment;
        }
    }
}
