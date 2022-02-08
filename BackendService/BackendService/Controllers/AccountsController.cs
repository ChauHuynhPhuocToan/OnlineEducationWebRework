using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Models;
using BC = BCrypt.Net.BCrypt;
using BackendService.Controllers.Custom;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountDTO>> PostAccount(Account account)
        {
            if (!AccountExists(account.Username))
            {
                account.Password = BC.HashPassword(account.Password);
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAccount", new { id = account.AccountId },
                                        new AccountDTO { AccountId = account.AccountId,
                                            Username = account.Username,
                                            IsActive = true,
                                            Role = account.Role,
                                            Verification = account.Verification });
            }
            return null;
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string userName)
        {
            return _context.Accounts.Any(e => e.Username == userName);
        }
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
        // POST: api/Accounts/Signin
        [HttpPost]
        [Route("Signin")]
        public async Task<ActionResult<Account>> SignIn(Account account)
        {
            var accountCheck = _context.Accounts.FirstOrDefault(x => x.Username == account.Username);
            if (accountCheck != null)
            {
                if (BC.Verify(account.Password, accountCheck.Password))
                {
                    return CreatedAtAction("GetAccount", new { id = accountCheck.AccountId }, new AccountDTO
                    {
                        AccountId = accountCheck.AccountId,
                        Username = accountCheck.Username,
                        IsActive = accountCheck.IsActive,
                        Role = accountCheck.Role,
                        Verification = accountCheck.Verification
                    });
                }
            }
            return null;
        }
        // PUT: api/Accounts/Updateinstructor?id=1
        [HttpPut]
        [Route("Updateinstructor")]
        public async Task<IActionResult> UpToInstructor(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            account.Role = AccountRole.Instructor;
            _context.Entry(account).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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
        // GET: api/Accounts/AccountRole?role1=1&role2=2
        [HttpGet("AccountRole")]
        public async Task<ActionResult<IEnumerable<Account>>> GetListAccountByRoles(int role1, int? role2)
        {
            return await _context.Accounts.Where(x => x.Role == (AccountRole)role1 || (!role2.HasValue || x.Role == (AccountRole)role2.Value)).ToListAsync();
        }
        // GET: api/Accounts/InstructorProfile?id=1
        [HttpGet("InstructorProfile")]
        public async Task<ActionResult<InstructorProfile>> GetInstructorProfile(int id)
        {
            var userId = _context.Accounts.FirstOrDefault(x => x.AccountId == id).UserId;
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            var instructorProfile = new InstructorProfile() {
                InstructorName = $"{user.FirstName} {user.LastName}",
                Description = user.Description,
                AvatarPath = FileRequestHandle.GetImageSource(user.AvatarPath),
                ReviewerCounter = 0,
                CourseCounter = 0,
                StudentCounter = 0
            };
            var course = await _context.Courses.Where(e => e.AccountId == id && e.IsActive).ToListAsync();
            if (course != null)
            {
                course.ForEach(e =>
                {
                    instructorProfile.ReviewerCounter += e.NumberOfRatings;
                    instructorProfile.CourseCounter++;
                    instructorProfile.StudentCounter += e.NumberOfParticipants;
                });
            }
            return instructorProfile;
        }
    }
}
