using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Models;
using System.Collections.Generic;

namespace HeartValve.Shared.StoredProcedures
{
    [StoredProcedure("AddMedicationActivityBatch")]
    public class AddMedicationActivityBatch
    {
        [StoredProcedureParameter(System.Data.SqlDbType.NVarChar, ParameterName = "userId")]
        public string UserId { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "activity")]
        public List<MedicationActivityType> Activity { get; set; }
    }
}