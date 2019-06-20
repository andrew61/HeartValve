using HeartValve.Shared.Data;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class OxygenSaturationController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var measurements = db.GetOxygenSaturation(UserId, null, null).ToList();
            return Ok(measurements);
        }

        [HttpPost]
        public IHttpActionResult Post(GetOxygenSaturation_Result measurement)
        {
            db.AddOxygenSaturation(UserId, measurement.SpO2, measurement.HeartRate, measurement.ReadingDate);
            return Ok();
        }
    }
}