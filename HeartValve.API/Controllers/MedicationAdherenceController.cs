using System;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [RoutePrefix("api/MedicationAdherence")]
    public class MedicationAdherenceController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime? startDate, DateTime? endDate)
        {
            var details = db.GetMedicationAdherence(UserId, startDate, endDate).ToList();
            return Ok(details);
        }
    }
}