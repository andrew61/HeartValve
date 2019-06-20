using System;
using System.Linq;
using System.Web.Mvc;
using HeartValve.Models;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HeartValve.Shared.Data;





namespace HeartValve.Controllers
{
    [Authorize(Roles = "Admin,Provider")]
    public class DeviceRequestController : ApplicationController
    {
        private ApplicationSignInManager _signInManager;
        // GET: DeviceRequest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestDevice()
        {

            RequestDeviceModel model = new RequestDeviceModel();
            return View(model);
        }

        public ActionResult RequestCheck()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(RequestDeviceModel model)
        {

            String providerEmail = model.email == null ? User.Identity.Name : model.email;

            model.Password = "Hv!1234";
            model.ConfirmPassword = "Hv!1234";
            var emailIncrement = "hv" + db.GetEmailIncrement().SingleOrDefault() + "@gmail.com";

            using (var ts = db.Database.BeginTransaction())
            {
                try
                {
                    var user = new ApplicationUser() { UserName = emailIncrement, Email = emailIncrement };
                    IdentityResult result = UserManager.Create(user, model.Password);

                    if (result.Succeeded)
                    {
                        db.AddUpdateUser(user.Id, model.firstName, model.lastName, null, model.enrollDate, null, null,
                        null, null, null, null, null, null,
                        null, null, model.mrn, true, model.deliveryDate, model.bpCuffSizeId).SingleOrDefault();
    
                        using (var client = new SmtpClient("smtp.gmail.com"))
                        {
                            client.Host = "smtp.gmail.com";
                            client.Port = 587;
                            client.EnableSsl = true;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new System.Net.NetworkCredential("spurstech.send@gmail.com", "$pur$tech1234");
                            using (MailMessage message = new MailMessage(providerEmail, "wildersp@musc.edu"))
                            {
                                message.To.Add("burrouja@musc.edu");
                                message.To.Add("patelsk@musc.edu");
                                message.Subject = "Heart Valve Equipment Request";
                                message.Body = "HeartValve user " + model.firstName + " " + model.lastName + "(" + emailIncrement + ") needs equipment assigned and deliveried by \n " + model.deliveryDate;
                                message.IsBodyHtml = true;
                                message.Priority = MailPriority.Normal;
                                client.Send(message);
                            }

                        }

                        db.AddRequestCheck(user.Id, providerEmail, DateTime.Now, model.deliveryDate.AddHours(-4), model.deliveryLocation, model.enrollDate.AddHours(-4), null, null, null, model.bpCuffSizeId, emailIncrement);
                        ts.Commit();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("User", error);
                        }
                    }
                }
                catch (Exception)
                {
                    ts.Rollback();
                    ModelState.AddModelError("Patient", "An unexpected error has occurred.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Content(emailIncrement);

        }


        public ActionResult RequestCheck_Read([DataSourceRequest]DataSourceRequest request)
        {

            var requestchecks = db.GetRequestCheck().ToList();

            DataSourceResult result = requestchecks.AsQueryable().ToDataSourceResult(request, c => c);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult RequestCheck_Update([DataSourceRequest]DataSourceRequest request, RequestCheckModel type)
        {
            if (ModelState.IsValid)
            {
                if (type.IsDelivered == true)
                {

                    db.AddUpdateRequestCheckDeliveryStatus(type.Id, UserId, DateTime.Now, type.IsDelivered);

                }
                else
                {
                    db.AddUpdateRequestCheck(type.Id, type.DeliveryDate, type.EnrollmentDate);

                }

            }
            return Json(new[] { type }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult RequestCheckDeliveryStatus_Update([DataSourceRequest]DataSourceRequest request, RequestCheckModel type)
        {

            if (ModelState.IsValid)
            {
                db.AddUpdateRequestCheckDeliveryStatus(type.Id, UserId, DateTime.Now, type.IsDelivered);
            }
            return Json(new[] { type }.ToDataSourceResult(request, ModelState));

        }

        public ActionResult GetBpCuffSize([DataSourceRequest]DataSourceRequest request)
        {
            var sizes = db.GetBPMCuffSizeLU().Select(x => new SelectListItem()
            {
                Text = x.Size,
                Value = x.BpCuffSizeId.ToString()
            }).ToList();

            return Json(sizes, JsonRequestBehavior.AllowGet);
        }
    }
}
