using System;

namespace BackendService.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public string CourseId { get; set; }
        public string AccountId { get; set; }
        public DateTime GetDate { get; set; }
    }
}
