using System;
using System.ComponentModel.DataAnnotations;

namespace HeartValve.Models
{
    public class UserViewModel
    {
        [ScaffoldColumn(false)]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Enrollment Date")]
        public Nullable<DateTime> EnrollmentDate { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string PasswordVerify { get; set; }

        [ScaffoldColumn(true)]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                {
                    return Email;
                }

                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }

        [Display(Name = "Systolic Min")]
        public Nullable<int> SystolicMinThreshold { get; set; }
        [Display(Name = "Systolic Max")]
        public Nullable<int> SystolicMaxThreshold { get; set; }
        [Display(Name = "Diastolic Min")]
        public Nullable<int> DiastolicMinThreshold { get; set; }
        [Display(Name = "Diastolic Max")]
        public Nullable<int> DiastolicMaxThreshold { get; set; }
        [Display(Name = "HeartRate Min")]
        public Nullable<int> HeartRateMinThreshold { get; set; }
        [Display(Name = "HeartRate Max")]
        public Nullable<int> HeartRateMaxThreshold { get; set; }
        [Display(Name = "Weight Min")]
        public decimal? WeightMinThreshold { get; set; }
        [Display(Name = "Weight Max")]
        public decimal? WeightMaxThreshold { get; set; }
        [Display(Name = "Sp02 Min")]
        public decimal? Sp02MinThreshold { get; set; }
        [Display(Name = "Sp02 Max")]
        public decimal? Sp02MaxThreshold { get; set; }

        public int? MRN { get; set; }

        public Boolean IsActive { get; set;}

        [Display(Name = "Equipment Delivery Date")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> DeliveryDate { get; set; }

        public int? SBPAvg { get; set; }
        public int? DBPAvg { get; set; }
        public int? PulseAvg { get; set; }
        public int? MapAvg { get; set; }
        public decimal? WeightAvg { get; set; }
        public decimal? SpO2Avg { get; set; }
        public bool? DiastolicBreached { get; set; }
        public bool? SystolicBreached { get; set; }
        public bool? HeartRateBreached { get; set; }
        public bool? WeightBreached { get; set; }
        public bool? SpO2Breached { get; set; }

        public int? BpCuffSizeId { get; set; }
        public bool IsInStudy { get; set; }

    }
    public class BPCuffSize
    {
        public int? BPCuffSizeId { get; set; }

        public String Size { get; set; }

    }
}