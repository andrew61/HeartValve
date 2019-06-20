using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace HeartValve.Models
{
    public class ScheduleGroupViewModel
    {
        public string UserId { get; set; }

        [Required]
        public int ScheduleTypeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [Required]
        public DateTime? ScheduleTime { get; set; }

        public int UserMedicationId { get; set; }

        [UIHint("UserMedications")]
        public UserMedicationViewModel UserMedication { get; set; }

        public string Dose { get; set; }

        public bool Active { get; set; }
        public int GroupId { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public DataTable SummaryTable { get; set; }
    }
}