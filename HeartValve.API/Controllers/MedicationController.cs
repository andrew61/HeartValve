using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class MedicationController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var medications = db.GetMedications().ToList();
            return Ok(medications);
        }
    }
}