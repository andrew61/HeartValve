using HeartValve.Shared.Data;
using System.Linq;
using System.Web.Http;


namespace HeartValve.API.Controllers
{
    [RoutePrefix("api/MedicationRefill")]
    public class MedicationRefillController : ApplicationController
    {
        [HttpGet]
        [Route("Refills/{userMedicationId}")]
        public IHttpActionResult Get(int userMedicationId)
        {
            var refills = db.GetUserMedicationRefills(userMedicationId).ToList();
            return Ok(refills);
        }

        [HttpPost]
        public IHttpActionResult Post(GetUserMedicationRefills_Result refill)
        {
            var id = db.AddUpdateMedicationRefill(refill.RefillId, UserId, refill.UserMedicationId, refill.RefillDate, refill.Quantity).SingleOrDefault();
            refill.RefillId = id.Value;
            return Ok(refill);
        }
    }
}