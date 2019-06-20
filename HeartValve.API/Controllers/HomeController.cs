using HeartValve.API.Security;
using System;
using System.Web.Mvc;

namespace HeartValve.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            string id = Guid.NewGuid().ToString();
            ViewData["id"] = id;
            ViewData["secret"] = Helper.GetHash(id);

            return View();
        }
    }
}
