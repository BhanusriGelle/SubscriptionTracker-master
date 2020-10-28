using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionTracker.Models
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }


        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }


        [Required]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [NotMapped]
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [NotMapped]
        [Display(Name = "Remember me")]
        public bool RememerMe { get; set; }
    }
}