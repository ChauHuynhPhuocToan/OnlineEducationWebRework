using System;
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
    public class CertificatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CertificatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Certificates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificates()
        {
            return await _context.Certificates.ToListAsync();
        }

        // GET: api/Certificates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> GetCertificate(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);

            if (certificate == null)
            {
                return NotFound();
            }

            return certificate;
        }

        // PUT: api/Certificates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCertificate(int id, Certificate certificate)
        {
            if (id != certificate.CertificateId)
            {
                return BadRequest();
            }

            _context.Entry(certificate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(id))
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

        // POST: api/Certificates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Certificate>> PostCertificate(Certificate certificate)
        {
            if(!CertificateCheckExists(certificate.AccountId, certificate.CourseId))
            {
                _context.Certificates.Add(certificate);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCertificate", new { id = certificate.CertificateId }, certificate);
            }
            return null;
        }

        // DELETE: api/Certificates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificateExists(int id)
        {
            return _context.Certificates.Any(e => e.CertificateId == id);
        }
        private bool CertificateCheckExists(string accountId, string courseId)
        {
            return _context.Certificates.Any(e => e.AccountId == accountId && e.CourseId == courseId);
        }
        // GET: api/Certificates/GetCertificateByCourseAccountId?accountId=1&courseId=1
        [HttpGet]
        [Route("GetCertificateByCourseAccountId")]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificateByCourseAccountId(string accountId, string courseId)
        {
            return await _context.Certificates.Where(e => e.AccountId == accountId && e.CourseId == courseId).ToListAsync();
        }
        // GET: api/Certificates/GetLastestCertificateByAccountId?accountId=1
        [HttpGet]
        [Route("GetLastestCertificateByAccountId")]
        public async Task<ActionResult<Certificate>> GetLastestCertificateByAccountId(string accountId)
        {
            return await _context.Certificates.Where(e => e.AccountId == accountId)
            .OrderByDescending(e => e.CertificateId)
            .FirstAsync();
        }
        // GET: api/Certificates/GetAllCertificateByAccountId?accountId=1
        [HttpGet]
        [Route("GetAllCertificateByAccountId")]
        public async Task<ActionResult<FullCertificateInfo>> GetAllCertificateByAccountId(int accountId)
        {
            var certificateFindList = await _context.Certificates.Where(e => e.AccountId == accountId.ToString())
            .OrderByDescending(e => e.CertificateId)
            .ToListAsync();
            var userId = _context.Accounts.FirstOrDefault(x => x.AccountId == accountId).UserId;
            var userInfo = _context.Users.FirstOrDefault(x => x.UserId == userId);
            var courseList = await _context.Courses.Where(x => x.IsActive).ToListAsync();
            if (certificateFindList != null)
            {
                var fullCertificateInfo = new FullCertificateInfo()
                {
                    CourseName = new List<String>(),
                    UserName = $"{userInfo.FirstName} {userInfo.LastName}",
                    AvatarPath = FileRequestHandle.GetImageSource(userInfo.AvatarPath),
                    GetDate = new List<String>()
                };
                certificateFindList.ForEach(e =>
                {
                    var courseName = courseList.FirstOrDefault(course => course.CourseId.ToString() == e.CourseId)?.CourseName;
                    if(courseName != null)
                    {
                        fullCertificateInfo.CourseName.Add(courseName);
                        fullCertificateInfo.GetDate.Add(e.GetDate.ToString(PropertyConst.DatetimeFormat));
                    }
                   
                });
                return fullCertificateInfo;
            }
            return null;
        }
    }
}
