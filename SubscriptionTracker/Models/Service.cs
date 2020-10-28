using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionTracker.Models
{
    [Table("tblServices")]
    public class Service
    {
        [Key]
        [Required]
        public int ServiceId { get; set; }
        [Required]
        public string ServiceName { get; set; }
        
        public string LogoUrl { get; set; }
        [Required]
        public string PlanStatus { get; set; }
        [Required]
        public int BillingTerm { get; set; }
        [Required]
        public decimal Pricing { get; set; }

        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public virtual User User { get; set; }
        
        public DateTime EndDate { get; set; }
    }
}