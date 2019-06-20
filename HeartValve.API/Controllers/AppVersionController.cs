using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class AppVersionController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var appVersion = db.GetAppVersion().SingleOrDefault();
            if (appVersion == null)
            {
                return NotFound();
            }
            return Ok(appVersion);
        }
    }
}