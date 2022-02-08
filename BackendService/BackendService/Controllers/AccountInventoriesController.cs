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
    public class AccountInventoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountInventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AccountInventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountInventory>>> GetAccountInventories()
        {
            return await _context.AccountInventories.ToListAsync();
        }

        // GET: api/AccountInventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountInventory>> GetAccountInventory(int id)
        {
            var accountInventory = await _context.AccountInventories.FindAsync(id);

            if (accountInventory == null)
            {
                return NotFound();
            }

            return accountInventory;
        }

        // PUT: api/AccountInventories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountInventory(int id, AccountInventory accountInventory)
        {
            if (id != accountInventory.AccountInventoryID)
            {
                return BadRequest();
            }

            _context.Entry(accountInventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountInventoryExists(id))
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

        // POST: api/AccountInventories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountInventory>> PostAccountInventory(AccountInventory accountInventory)
        {
            var checkInDB = _context.AccountInventories.FirstOrDefault(x => x.AccountId == accountInventory.AccountId && accountInventory.CourseId == accountInventory.CourseId && x.IsBought == true);
            if(checkInDB != null)
            {
                _context.AccountInventories.Add(accountInventory);
                var course = await _context.Courses.FindAsync(accountInventory.CourseId);
                course.NumberOfParticipants++;
                _context.Entry(course).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (AccountInventoryExists(accountInventory.AccountInventoryID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetAccountInventory", new { id = accountInventory.AccountInventoryID }, accountInventory);
            }
            return null;
        }

        // DELETE: api/AccountInventories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountInventory(int id)
        {
            var accountInventory = await _context.AccountInventories.FindAsync(id);
            if (accountInventory == null)
            {
                return NotFound();
            }

            _context.AccountInventories.Remove(accountInventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountInventoryExists(int id)
        {
            return _context.AccountInventories.Any(e => e.AccountInventoryID == id);
        }

        // GET: api/AccountInventories/GetAccountInventoriesByAccountid?id=1&option=1
        [HttpGet]
        [Route("GetAccountInventoriesByAccountid")]
        public async Task<IEnumerable<AccountInventory>> GetAccountInCoursesByAccountid(int id, OptionType option)
        {

            var accountinCourse = (option == OptionType.IsBought) ? await _context.AccountInventories.Where(e => e.AccountId == id && e.IsBought == true).OrderByDescending(e => e.AccountInventoryID).ToListAsync() 
                : await _context.AccountInventories.Where(e => e.AccountId == id && e.GetPayment == true).OrderByDescending(e => e.AccountInventoryID).ToListAsync();

            return accountinCourse;
        }
        // GET: api/AccountInventories/GetAccountInventoriesByInvoiceCode?option=1&invoiceCode=1
        [HttpGet]
        [Route("GetAccountInventoriesByInvoiceCode")]
        public async Task<IEnumerable<AccountInventory>> GetAccountInventoriesByInvoiceCode(OptionType option, string invoiceCode)
        {

            var accountinCourse = (option == OptionType.IsBought) ? await _context.AccountInventories.Where(e => e.InvoiceCode == invoiceCode && e.IsBought == true).OrderByDescending(e => e.AccountInventoryID).ToListAsync()
            :await _context.AccountInventories.Where(e => e.InvoiceCode == invoiceCode && e.GetPayment == true).OrderByDescending(e => e.AccountInventoryID).ToListAsync();
            return accountinCourse;
        }
        // GET: api/AccountInventories/GetMyCourseList?id=1
        [HttpGet]
        [Route("GetMyCourseList")]
        public async Task<IEnumerable<Course>> GetMyCourseList(int id)
        {
            List<Course> courseList = new List<Course>();
            var accountInventory = _context.AccountInventories.Where(x => x.AccountId == id && x.IsBought == true).Select(x => x.CourseId).ToList();
            if (accountInventory != null)
            {
                var list = await _context.Courses.Where(e => e.IsActive == true).ToListAsync();
                accountInventory.ForEach(e =>
                {
                    var course = list.FirstOrDefault(x => x.CourseId == e);
                    if(course != null)
                    {
                        courseList.Add(course);
                    }
                });
            }
            return courseList;
        }
        // POST: api/AccountInventories/AddCartItem
        [HttpPost]
        [Route("AddCartItem")]
        public async Task<ActionResult<AccountInventory>> PostCartItem(AccountInventory accountInventory)
        {
            if (!CartExists(accountInventory))
            {
                _context.AccountInventories.Add(accountInventory);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetAccountinCourse", new { id = accountInventory.AccountInventoryID }, accountInventory);
            }
            return null;
        }
        // GET: api/AccountInventories/GetCartList?id=1
        [HttpGet]
        [Route("GetCartList")]
        public async Task<IEnumerable<Course>> GetCartList(int id)
        {
            List<Course> cartList = new List<Course>();
            var accountInventories = _context.AccountInventories.Where(x => x.AccountId == id && x.IsCart == true).Select(x => x.CourseId).ToList();
            if (accountInventories != null)
            {
                var list = await _context.Courses.Where(e => e.IsActive == true).ToListAsync();
                accountInventories.ForEach(e =>
                {
                    var result = list.FirstOrDefault(x => x.CourseId == e);
                    if(result != null)
                    {
                        cartList.Add(result);
                    }
                });
            }
            return cartList;
        }
        private bool CartExists(AccountInventory accountInventory)
        {
            return _context.AccountInventories.Any(e => e.CourseId == accountInventory.CourseId && e.AccountId == accountInventory.AccountId && e.IsCart == true);
        }
        // DELETE: api/AccountInventories/DeleteCartItem?accountId=1&courseId=1
        [HttpDelete]
        [Route("DeleteCartItem")]
        public async Task<AccountInventory> DeleteCartItem(int accountId, int courseId)
        {
            var accountInventory = _context.AccountInventories.FirstOrDefault(e => e.AccountId == accountId && e.CourseId == courseId && e.IsCart == true);
            if (accountInventory != null)
            {
                _context.AccountInventories.Remove(accountInventory);
                await _context.SaveChangesAsync();
            }
            return accountInventory;
        }
        // DELETE: api/AccountInventories/EmptyCartItem?accountId=1
        [HttpDelete]
        [Route("EmptyCartItem")]
        public async Task<IEnumerable<AccountInventory>> EmptyCartItem(int accountId)
        {
            var accountInventory = await _context.AccountInventories.Where(e => e.AccountId == accountId && e.IsCart == true).ToListAsync();
            if (accountInventory != null)
            {
                _context.AccountInventories.RemoveRange(accountInventory);
                await _context.SaveChangesAsync();
            }
            return accountInventory;
        }
    }
}
