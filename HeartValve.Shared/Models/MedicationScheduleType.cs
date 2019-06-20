using EntityFrameworkExtras.EF6;
using System;

namespace HeartValve.Shared.Models
{
    [UserDefinedTableType("MedicationScheduleType")]
    public class MedicationScheduleType
    {
        [UserDefinedTableTypeColumn(1)]
        public int ScheduleId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public string UserId { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public int UserMedicationId { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public DateTime StartDate { get; set; }

        [UserDefinedTableTypeColumn(5)]
        public Nullable<DateTime> EndDate { get; set; }

        [UserDefinedTableTypeColumn(6)]
        public int DayOfWeek { get; set; }

        [UserDefinedTableTypeColumn(7)]
        public TimeSpan ScheduleTime { get; set; }

        [UserDefinedTableTypeColumn(8)]
        public Nullable<int> GroupId { get; set; }

        [UserDefinedTableTypeColumn(9)]
        public string Dose { get; set; }
    }
}