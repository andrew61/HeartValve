using HeartValve.Shared.Data;
using System.Linq;
using System.Web.Http;
using HeartValve.Shared.Models;

namespace HeartValve.API.Controllers
{
    public class WeightController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var measurements = db.GetWeightMeasurements(UserId, null, null).ToList();
            return Ok(measurements);
        }

        [HttpPost]
        public IHttpActionResult Post(GetWeightMeasurementsChart_Result measurement)
        {
            db.AddWeightMeasurement(UserId, measurement.Weight, measurement.ReadingDate);
            return Ok();
        }
    }
}