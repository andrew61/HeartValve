using System;

namespace HeartValve.Shared.Models
{
    public class MedicationSchedule
    {
        public int ScheduleId { get; set; }
        public string UserId { get; set; }
        public int UserMedicationId { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime Time { get; set; }
        public bool Inactive { get; set; }
    }
}