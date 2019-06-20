using HeartValve.Shared.Data;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [RoutePrefix("api/UserMedication")]
    public class UserMedicationController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var medication = db.GetUserMedication(id).SingleOrDefault();
            return Ok(medication);
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var medications = db.GetUserMedications(UserId).ToList();
            return Ok(medications);
        }

        [HttpGet]
        [Route("Medications/NotTaken")]
        public IHttpActionResult NotTaken()
        {
            var medications = db.GetUserMedicationsNotTaken(UserId).ToList();
            return Ok(medications);
        }

        [HttpGet]
        [Route("Medications/AsNeeded")]
        public IHttpActionResult AsNeeded()
        {
            var medications = db.GetUserMedicationsAsNeeded(UserId).ToList();
            return Ok(medications);
        }

        [HttpPost]
        public IHttpActionResult Post(GetUserMedication_Result medication)
        {
            var id = db.AddUpdateUserMedication(medication.UserMedicationId, UserId, medication.MedicationId, medication.Quantity, medication.Strength,
                medication.Unit, medication.DosageForm, medication.Route, medication.Dose, medication.Frequency, medication.PillCapId, medication.MedicationScheduleTypeId,
                medication.Indication, medication.Instructions).SingleOrDefault();
            medication.UserMedicationId = id.Value;
            return Ok(medication);
        }

        [HttpPost]
        [Route("Delete")]
        public IHttpActionResult Delete(GetUserMedication_Result medication)
        {
            db.DeleteUserMedication(medication.UserMedicationId);
            return Ok();
        }
    }
}