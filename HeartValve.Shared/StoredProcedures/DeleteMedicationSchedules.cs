using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Models;
using System.Collections.Generic;

namespace HeartValve.Shared.StoredProcedures
{
    [StoredProcedure("DeleteMedicationSchedules")]
    public class DeleteMedicationSchedules
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "schedules")]
        public List<MedicationScheduleType> Schedules { get; set; }
    }
}