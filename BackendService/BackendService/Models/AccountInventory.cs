using System;
using System.ComponentModel.DataAnnotations;
using BackendService.AppConsts;

namespace BackendService.Models
{
    public class AccountInventory
    {
        [Key]
        public int AccountInventoryID { get; set; }
        public Account Account { get; set; }
        [Required]
        public int AccountId { get; set; }
        public Course Course { get; set; }
        [Required]
        public int CourseId { get; set; }
        [StringLength(PropertyConst.InvoiceCodeMaxLength, ErrorMessage = "The {0} must be at least {2} characters long, maximum lenghth {1}!", MinimumLength = PropertyConst.InvoiceCodeMinLength)]
        public string InvoiceCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Boolean IsBought { get; set; }
        public Boolean IsCart { get; set; }
        public Boolean GetPayment { get; set; }
    }
    public enum PaymentMethod
    {
        Paypal,
        CreditCard
    }
}
