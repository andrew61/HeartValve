using EntityFrameworkExtras.EF6;

namespace HeartValve.Shared.Models
{
    [UserDefinedTableType("MedicationAlertType")]
    public class MedicationAlertType
    {
        [UserDefinedTableTypeColumn(1)]
        public int AlertId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public string UserId { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public int MinutesOffset { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public int MedicationAlertTypeId { get; set; }

        [UserDefinedTableTypeColumn(5)]
        public int ScheduleGroupId { get; set; }
    }
}