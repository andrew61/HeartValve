using HeartValve.Shared.Data;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class BloodGlucoseController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var measurements = db.GetBloodGlucoseMeasurements(UserId).ToList();
            return Ok(measurements);

        }

        [HttpPost]
        public IHttpActionResult Post(GetBloodGlucoseMeasurements_Result measurement)
        {
            db.AddBloodGlucoseMeasurement(UserId, measurement.GlucoseLevel, measurement.ReadingDate);
            return Ok();
        }
    }
}