using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Models;
using BackendService.Controllers.Custom;
using BackendService.AppConsts;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.CommentId }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
        // GET: api/Comments/GetCommentListByCourseId?id=1
        [HttpGet]
        [Route("GetCommentListByCourseId")]
        public async Task<ActionResult<IEnumerable<CommentData>>> GetCommentListByCourseId(int id)
        {
            var commentListDB = await _context.Comments.Where(e => e.CourseId == id && e.Type == CommentType.Comment).OrderByDescending(e => e.CommentId).ToListAsync();
            var commentList = new List<CommentData>();
            if (commentListDB != null)
            {
                var userList = await _context.Users.ToListAsync();
                commentListDB.ForEach(x => {
                    var user = userList.FirstOrDefault(u => u.UserId == x.UserId);
                    var comment = new CommentData()
                    {
                        CommentId = x.CommentId,
                        CommentContext = x.CommentContext,
                        DatePost = x.DatePost.ToString(PropertyConst.DatetimeFormat),
                        UserId = user.UserId,
                        UserName = $"{user.FirstName} {user.LastName}",
                        AvatarPath = user.AvatarPath
                    };
                    commentList.Add(comment);
                });
            }
            return commentList;
        }
        // GET: api/Comments/GetRatingListByCourseId?id=1
        [HttpGet]
        [Route("GetRatingListByCourseId")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetRatingListByCourseId(int id)
        {
            return await _context.Comments.Where(e => e.CourseId == id && e.Type == CommentType.Rating).ToListAsync();
        }
    }
}
