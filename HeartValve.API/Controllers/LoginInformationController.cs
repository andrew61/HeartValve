using HeartValve.Shared.Data;
using System.Web.Http;
using HeartValve.API.Models;

namespace HeartValve.API.Controllers
{
    public class LoginInformationController : ApplicationController
    {
        [HttpPost]
        public IHttpActionResult Post(LoginInformationViewModel info)
        {
            db.AddLoginInformation(UserName, info.Time, info.Longitude, info.Latitude, info.Model, info.OS, info.Network, info.PhoneType, info.AppVersion);
            return Ok();
        }
    }
}