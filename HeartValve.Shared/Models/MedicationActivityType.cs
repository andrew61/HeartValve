using EntityFrameworkExtras.EF6;
using System;

namespace HeartValve.Shared.Models
{
    [UserDefinedTableType("MedicationActivityType")]
    public class MedicationActivityType
    {
        [UserDefinedTableTypeColumn(1)]
        public int ActivityTypeId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public DateTime ActivityDate { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public int UserMedicationId { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public Nullable<int> ScheduleId { get; set; }

        [UserDefinedTableTypeColumn(5)]
        public Nullable<DateTime> ScheduleDate { get; set; }
    }
}