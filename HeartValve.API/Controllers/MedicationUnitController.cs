using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class MedicationUnitController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var units = db.GetMedicationUnits().ToList();
            return Ok(units);
        }
    }
}