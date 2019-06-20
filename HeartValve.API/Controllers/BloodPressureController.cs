using HeartValve.Shared.Data;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class BloodPressureController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var measurements = db.GetBloodPressureMeasurements(UserId, null, null).ToList();
            return Ok(measurements);
        }

        [HttpPost]
        public IHttpActionResult Post(GetBloodPressureMeasurements_Result measurement)
        {
            db.AddBloodPressureMeasurement(UserId, measurement.Systolic, measurement.Diastolic, measurement.Map, measurement.Pulse, measurement.ReadingDate);
            return Ok();
        }
    }
}