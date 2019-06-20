using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Models;
using System.Collections.Generic;
using System.Data;

namespace HeartValve.Shared.StoredProcedures
{
    [StoredProcedure("AddUpdateMedicationSchedules")]
    public class AddUpdateMedicationSchedules
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "schedules")]
        public List<MedicationScheduleType> Schedules { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Int, Direction = ParameterDirection.Output)]
        public int GroupId { get; set; }
    }
}