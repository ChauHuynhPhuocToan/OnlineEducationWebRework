using OnlineEducation.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineEducation.Models
{
    [Table("Roles")]
    public class Role: BaseClasses
    {
        [StringLength(AppConst.BasicStringLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string RoleName { get; set; } = string.Empty;
        [StringLength(AppConst.BasicLargeLongLength, MinimumLength = AppConst.BasicStringMinLength)]
        public string? Description { get; set; } = string.Empty;

        public Role() :base() {
        
        }

        public Role(string? roleName = null, string? description = null) 
            : base()
        {
            this.RoleName= roleName;
            this.Description= description;
        }
    }
}
