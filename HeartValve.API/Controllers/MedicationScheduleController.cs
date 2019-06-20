using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Models;
using HeartValve.Shared.StoredProcedures;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [RoutePrefix("api/MedicationSchedule")]
    public class MedicationScheduleController : ApplicationController
    {
        [HttpGet]
        [Route("Schedules")]
        public IHttpActionResult Get()
        {
            var schedules = db.GetMedicationSchedules(UserId).ToList();
            return Ok(schedules);
        }

        [HttpPost]
        [Route("Schedules")]
        public IHttpActionResult Post(List<MedicationScheduleType> schedules)
        {
            schedules.ForEach(x => x.UserId = UserId);

            var sp = new AddUpdateMedicationSchedules() { Schedules = schedules };
            db.Database.ExecuteStoredProcedure(sp);
            return Ok(db.GetMedicationSchedules(UserId).Where(x => x.GroupId == sp.GroupId).ToList());
        }

        [HttpPost]
        [Route("Schedules/Delete")]
        public IHttpActionResult Delete(List<MedicationScheduleType> schedules)
        {
            schedules.ForEach(x => x.UserId = UserId);

            var sp = new DeleteMedicationSchedules() { Schedules = schedules };
            db.Database.ExecuteStoredProcedure(sp);
            return Ok();
        }

        [HttpPost]
        [Route("Schedules/Inactive/{inactive}")]
        public IHttpActionResult PostInactive(List<MedicationScheduleType> schedules, bool inactive)
        {
            foreach (var schedule in schedules)
            {
                schedule.UserId = UserId;
            }

            var sp = new SetMedicationSchedulesInactive() { Schedules = schedules, Inactive = inactive };
            db.Database.ExecuteStoredProcedure(sp);
            return Ok();
        }

        [HttpGet]
        [Route("Templates/{frequency}")]
        public IHttpActionResult GetTemplates(int frequency)
        {
            var templates = db.GetMedicationScheduleTemplates(frequency).ToList();
            return Ok(templates);
        }

        [HttpGet]
        [Route("Template/{id}")]
        public IHttpActionResult GetTemplate(int id)
        {
            var template = db.GetMedicationScheduleTemplate(id).ToList();
            return Ok(template);
        }
    }
}