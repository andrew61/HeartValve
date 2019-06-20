using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class MedicationDosageFormController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var dosageForms = db.GetMedicationDosageForms().ToList();
            return Ok(dosageForms);
        }
    }
}