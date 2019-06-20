using HeartValve.Shared.Models;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = db.GetUser(UserId).SingleOrDefault();
            return Ok(user);
        }

        [HttpGet]
        [Route("Enrollment")]
        public IHttpActionResult GetEnrollment()
        {
            var user = db.GetUserWithSurvey(UserId).SingleOrDefault();
            return Ok(user);
        }

        [HttpGet]
        [Route("ActivationStatus")]
        public IHttpActionResult GetUserActivationStatus()
        {
            var activationStatus = db.GetUserActivationStatus(UserId).SingleOrDefault();
            return Ok(activationStatus);
        }

        [HttpPost]
        [Route("APNSToken")]
        public IHttpActionResult PostAPNSToken(DeviceToken token)
        {
            db.AddDeviceToken(UserId, 1, token.Token);
            return Ok();
        }

        [HttpPost]
        [Route("GCMToken")]
        public IHttpActionResult PostGCMToken(DeviceToken token)
        {
            db.AddDeviceToken(UserId, 2, token.Token);
            return Ok();
        }
    }
}