using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Models;
using HeartValve.Shared.StoredProcedures;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [RoutePrefix("api/MedicationAlert")]
    public class MedicationAlertController : ApplicationController
    {
        [HttpGet]
        [Route("Alerts")]
        public IHttpActionResult Get()
        {
            var alerts = db.GetMedicationAlerts(UserId).ToList();
            return Ok(alerts);
        }

        [HttpGet]
        [Route("Alerts/{groupId}")]
        public IHttpActionResult Get(int groupId)
        {
            var alerts = db.GetMedicationAlertsByGroupId(groupId).ToList();
            return Ok(alerts);
        }

        [HttpPost]
        [Route("Alerts")]
        public IHttpActionResult Post(List<MedicationAlertType> alerts)
        {
            alerts.ForEach(x => x.UserId = UserId);

            var sp = new AddUpdateMedicationAlerts() { Alerts = alerts };
            db.Database.ExecuteStoredProcedure(sp);
            return Ok();
        }
    }
}