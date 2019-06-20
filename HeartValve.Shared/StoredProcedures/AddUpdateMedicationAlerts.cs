using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Models;
using System.Collections.Generic;

namespace HeartValve.Shared.StoredProcedures
{
    [StoredProcedure("AddUpdateMedicationAlerts")]
    public class AddUpdateMedicationAlerts
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "alerts")]
        public List<MedicationAlertType> Alerts { get; set; }
    }
}