using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HeartValve.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace HeartValve.Controllers
{
    [Authorize(Roles = "Admin,Provider")]
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
        {

            var users = db.GetUserMeasurementAvg().ToList().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                SBPAvg = x.SBPAvg,
                DBPAvg = x.DBPAvg,
                PulseAvg = x.PulseAvg,
                MapAvg = x.MapAvg,
                WeightAvg = x.WeightAvg,
                SpO2Avg = x.SpO2Avg,
                DiastolicBreached = x.DiastolicBreached,
                SystolicBreached = x.SystolicBreached,
                HeartRateBreached = x.HeartRateBreached,
                WeightBreached = x.WeightBreached,
                SpO2Breached = x.SpO2Breached,
                DiastolicMinThreshold = x.DiastolicMinThreshold,
                DiastolicMaxThreshold = x.DiastolicMaxThreshold,
                SystolicMinThreshold = x.SystolicMinThreshold,
                SystolicMaxThreshold = x.SystolicMaxThreshold,
                HeartRateMinThreshold = x.HeartRateMinThreshold,
                HeartRateMaxThreshold = x.HeartRateMaxThreshold,
                WeightMinThreshold = x.WeightMinThreshold,
                WeightMaxThreshold = x.WeightMaxThreshold,
                Sp02MinThreshold = x.SpO2MinThreshold,
                Sp02MaxThreshold = x.SpO2MaxThreshold,
                IsActive = x.IsActive
            });

            DataSourceResult result = users.AsQueryable().ToDataSourceResult(request, c => c);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _TabStripBP(MeasurementsViewModel reportViewModel, string userId)
        {
            var bpGroups = db.GetBPMeasurementsChart(userId, DateTime.Now.AddDays(-31), null).ToList();

            reportViewModel.SBPSeries = bpGroups.Select(x => x.SBP);
            reportViewModel.DBPSeries = bpGroups.Select(x => x.DBP);
            reportViewModel.HeartRateSeries = bpGroups.Select(x => x.HR);

            return View(reportViewModel);
        }

        public ActionResult BPData_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {

            var bpReadings = db.GetBloodPressureMeasurements(userId,DateTime.Now.AddDays(-31),null).Select(x => new BloodPressureViewModel
            {
                BloodPressureId = x.BloodPressureId,
                UserId = x.UserId,
                Systolic = x.Systolic,
                Diastolic = x.Diastolic,
                ReadingDate = x.ReadingDate,
                HeartRate = x.Pulse
            }).ToList();

            DataSourceResult result = bpReadings.AsQueryable().ToDataSourceResult(request, c => c);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BP_Chart_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {

            var bpGroups = db.GetBPMeasurementsChart(userId, DateTime.Now.AddDays(-31), null).Select(x => new BloodPressureViewModel
            {
                Systolic = x.SBP,
                Diastolic = x.DBP,
                ReadingDate = x.ReadingDate,
                HeartRate = x.HR
            }).ToList();

            DataSourceResult result = bpGroups.AsQueryable().ToDataSourceResult(request, c => c);
            return Json(bpGroups);
        }

        public ActionResult _TabStripWeight(MeasurementsViewModel reportViewModel, string userId)
        {
            var bpGroups = db.GetWeightMeasurementsChart(userId, DateTime.Now.AddDays(-31), null).ToList();

            reportViewModel.WeightSeries = bpGroups.Select(x => x.Weight);

            return View(reportViewModel);
        }

        public ActionResult WeightData_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {

            var weightReadings = db.GetWeightMeasurements(userId, DateTime.Now.AddDays(-31), null).Select(x => new WeightViewModel
            {
                WeightId = x.WeightId,
                UserId = x.UserId,
                Weight = x.Weight,
                ReadingDate = x.ReadingDate,
            }).ToList();

            DataSourceResult result = weightReadings.AsQueryable().ToDataSourceResult(request, c => c);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Weight_Chart_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {

            var weightGroups = db.GetWeightMeasurementsChart(userId, DateTime.Now.AddDays(-31), null).Select(x => new WeightViewModel
            {
                Weight = x.Weight,
                ReadingDate = x.ReadingDate,
            }).ToList();

            return Json(weightGroups);
        }

        public ActionResult _TabStripSPO2(MeasurementsViewModel reportViewModel, string userId)
        {
            var bpGroups = db.GetOxygenSaturationChart(userId, DateTime.Now.AddDays(-31), null).ToList();

            reportViewModel.Spo2Series = bpGroups.Select(x => x.Spo2);

            return View(reportViewModel);
        }

        public ActionResult SPO2Data_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {

            var weightReadings = db.GetOxygenSaturation(userId, DateTime.Now.AddDays(-31), null).Select(x => new OxygenSaturationViewModel
            {
                OxygenSaturationId = x.OxygenSaturationId,
                UserId = x.UserId,
                SpO2 = x.SpO2,
                ReadingDate = x.ReadingDate,
            }).ToList();

            DataSourceResult result = weightReadings.AsQueryable().ToDataSourceResult(request, c => c);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SPO2_Chart_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {

            var spo2Groups = db.GetOxygenSaturationChart(userId, DateTime.Now.AddDays(-31), null).Select(x => new OxygenSaturationViewModel
            {
                SpO2 = x.Spo2,
                ReadingDate = x.ReadingDate,
            }).ToList();

            return Json(spo2Groups);
        }
    }
}