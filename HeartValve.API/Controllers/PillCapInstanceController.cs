using HeartValve.Shared.Models;
using System.Linq;
using System.Transactions;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class PillCapInstanceController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var instances = db.GetPillCapInstances(UserId).ToList();
            return Ok(instances);
        }

        [HttpPost]
        public IHttpActionResult Post(PillCapInstance instance)
        {
            var pillCapId = db.AddPillCap(instance.SerialNumber).SingleOrDefault();
            db.AddUpdatePillCapInstance(instance.PillCapInstanceId, UserId, pillCapId, instance.PillCapName, instance.StartDate, instance.EndDate);
            return Ok();
        }
    }
}