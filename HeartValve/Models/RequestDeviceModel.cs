using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HeartValve.Models
{
    public class RequestDeviceModel
    {
        [Required]
        [Display(Name = "   First Name Required")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "   Last Name Required")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "   MRN Required")]
        public int mrn { get; set; }

        [Display(Name = "Device User Bp Cuff Size")]
        public int? bpCuffSizeId { get; set; }

        [Required]
        [Display(Name = "Delivery Date")]
        public DateTime deliveryDate { get; set; }

        [Display(Name = "Delivery Location")]
        public string deliveryLocation { get; set; }

        [Required]
        [Display(Name = "Enrollment Date")]
        public DateTime enrollDate { get; set; }

        //[Display(Name = "Email Required")]
        [EmailAddress]
        public string email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password ")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}