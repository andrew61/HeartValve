using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Models;
using System.Collections.Generic;

namespace HeartValve.Shared.StoredProcedures
{
    [StoredProcedure("SetMedicationSchedulesInactive")]
    public class SetMedicationSchedulesInactive
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "schedules")]
        public List<MedicationScheduleType> Schedules { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Bit, ParameterName = "inactive")]
        public bool Inactive { get; set; }
    }
}