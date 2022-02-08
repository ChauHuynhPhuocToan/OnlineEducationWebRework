using BackendService.AppConsts;
using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        [StringLength(PropertyConst.NotificationTitleMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.NotificationTitleMinLength)]
        public string MessageTitle { get; set; }
        [StringLength(PropertyConst.NotificationMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.NotificationMinLength)]
        public string Message { get; set; }
        public string Type { get; set; }
        public string SendTo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
