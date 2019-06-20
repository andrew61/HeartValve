using Microsoft.AspNet.Identity;
using HeartValve.Shared.Data;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [Authorize]
    public class ApplicationController : ApiController
    {
        protected HeartValveEntities db = new HeartValveEntities();

        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }

        public string UserName
        {
            get { return User.Identity.GetUserName(); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}