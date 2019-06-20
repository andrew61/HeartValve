using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class MedicationRouteController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var routes = db.GetMedicationRoutes().ToList();
            return Ok(routes);
        }
    }
}