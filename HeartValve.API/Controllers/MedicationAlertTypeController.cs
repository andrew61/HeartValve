using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [RoutePrefix("api/MedicationAlertType")]
    public class MedicationAlertTypeController : ApplicationController
    {
        [HttpGet]
        [Route("Types")]
        public IHttpActionResult Get()
        {
            var alertTypes = db.GetMedicationAlertTypes().ToList();
            return Ok(alertTypes);
        }
    }
}