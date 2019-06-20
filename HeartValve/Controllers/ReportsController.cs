using HeartValve.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using SautinSoft;
using System.Diagnostics;
using System.Net.Mail;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using ImageMagick;




namespace HeartValve.Controllers
{
    [Authorize(Roles = "Admin,Provider")]
    public class ReportsController : ApplicationController
    {
        [HttpGet]
        public ActionResult Measurements()
        {
            var vm = new MeasurementsViewModel();

            var users = db.GetUsers().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList().OrderBy(x => x.LastName);
            
            vm.UserSelectList = new SelectList(users, "UserId", "DisplayName");
            vm.StartDate = DateTime.Now;
            vm.EndDate = DateTime.Now;
            
             
            return View(vm);
        }

        [HttpGet]
        public ActionResult ArchivedMeasurements()
        {
            var vm = new MeasurementsViewModel();
            var users = db.GetArchivedUsers().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList().OrderBy(x => x.LastName);
            vm.UserSelectList = new SelectList(users, "UserId", "DisplayName");
            vm.StartDate = DateTime.Now;
            vm.EndDate = DateTime.Now;

            return View(vm);
        }
        public ActionResult WeightChart(MeasurementsViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var weightGroups = db.GetWeightMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderBy(x =>x.ReadingDate);

            reportViewModel.WeightSeries = weightGroups.Select(x => x.Weight);
            reportViewModel.ReadingDate = weightGroups.Select(x => (x.ReadingDate).ToString("MM/dd"));

            reportViewModel.Thresholds.WeightMin = (int?)weightGroups.Select(x => x.WeightMin).FirstOrDefault();
            reportViewModel.Thresholds.WeightMax = (int?)weightGroups.Select(x => x.WeightMax).FirstOrDefault();

            return PartialView(reportViewModel);
        }

        public ActionResult WeightMeasurements_Read([DataSourceRequest]DataSourceRequest request, MeasurementsViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var weightGroups = db.GetWeightMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderByDescending(x => x.ReadingDate);

            reportViewModel.WeightTable.Columns.Add("Weight", typeof(Decimal)).Caption = "Weight";
            reportViewModel.WeightTable.Columns.Add("ReadingDate", typeof(string)).Caption = "ReadingDate";
            ViewData["userid"] = userId;

            foreach (var Group in weightGroups)
            {
                var row = reportViewModel.WeightTable.NewRow();
                row[0] = Group.Weight;
                row[1] = Convert.ToDateTime(Group.ReadingDate.ToString()).ToString("MM/dd/yyyy hh:mm tt");

                reportViewModel.WeightTable.Rows.Add(row);
            }
            return Json(reportViewModel.WeightTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BloodPressureChart(MeasurementsViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var bpGroups = db.GetBPMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderBy(x => x.ReadingDate);

            reportViewModel.SBPSeries = bpGroups.Select(x => x.SBP);
            reportViewModel.DBPSeries = bpGroups.Select(x => x.DBP);
            reportViewModel.HRSeries = bpGroups.Select(x => x.HR);
            //reportViewModel.ReadingDate = bpGroups.Select(x => x.ReadingDate);
            reportViewModel.ReadingDate = bpGroups.Select(x => (x.ReadingDate).ToString("MM/dd"));

            reportViewModel.Thresholds.SystolicMin = (int?)bpGroups.Select(x => x.SystolicMin).FirstOrDefault();
            reportViewModel.Thresholds.SystolicMax = (int?)bpGroups.Select(x => x.SystolicMax).FirstOrDefault();
            reportViewModel.Thresholds.DiastolicMin = (int?)bpGroups.Select(x => x.DiastolicMin).FirstOrDefault();
            reportViewModel.Thresholds.DiastolicMax = (int?)bpGroups.Select(x => x.DiastolicMax).FirstOrDefault();

            return PartialView(reportViewModel);
        }
       
        public ActionResult BPMeasurements_Read([DataSourceRequest]DataSourceRequest request, MeasurementsViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var bpGroups = db.GetBPMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderByDescending(x => x.ReadingDate);

            reportViewModel.BloodPressureTable.Columns.Add("SBP", typeof(int)).Caption = "SBP";
            reportViewModel.BloodPressureTable.Columns.Add("DBP", typeof(int)).Caption = "DBP";
            reportViewModel.BloodPressureTable.Columns.Add("HR", typeof(int)).Caption = "HR";

            reportViewModel.BloodPressureTable.Columns.Add("ReadingDate", typeof(string)).Caption = "ReadingDate";

            foreach (var Group in bpGroups)
            {
                var row = reportViewModel.BloodPressureTable.NewRow();
                row[0] = Group.SBP;
                row[1] = Group.DBP;
                row[2] = Group.HR;
                row[3] = Convert.ToDateTime(Group.ReadingDate.ToString()).ToString("MM/dd/yyyy hh:mm tt");

                reportViewModel.BloodPressureTable.Rows.Add(row);
            }
            return Json(reportViewModel.BloodPressureTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }        

        public ActionResult OxygenSaturationChart(MeasurementsViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var oxyGroups = db.GetOxygenSaturationChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderBy(x => x.ReadingDate);

            reportViewModel.Spo2Series = oxyGroups.Select(x => x.Spo2);
            reportViewModel.HeartRateSeries = oxyGroups.Select(x => x.HeartRate);
            reportViewModel.ReadingDate = oxyGroups.Select(x => (x.ReadingDate).ToString("MM/dd"));
            reportViewModel.Thresholds.Sp02Min = (int?)oxyGroups.Select(x => x.Sp02Min).FirstOrDefault();
            reportViewModel.Thresholds.Sp02Max = (int?)oxyGroups.Select(x => x.Sp02Max).FirstOrDefault();
            reportViewModel.Thresholds.HeartRateMin = (int?)oxyGroups.Select(x => x.HeartRateMin).FirstOrDefault();
            reportViewModel.Thresholds.HeartRateMax = (int?)oxyGroups.Select(x => x.HeartRateMax).FirstOrDefault();
            return PartialView(reportViewModel);
        }
             
        public ActionResult OxygenSaturation_Read([DataSourceRequest]DataSourceRequest request, MeasurementsViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var oxyGroups = db.GetOxygenSaturationChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderByDescending(x => x.ReadingDate);

            reportViewModel.OxygenSaturationTable.Columns.Add("Spo2", typeof(int)).Caption = "Spo2";
            reportViewModel.OxygenSaturationTable.Columns.Add("HeartRate", typeof(int)).Caption = "HeartRate";
            reportViewModel.OxygenSaturationTable.Columns.Add("ReadingDate", typeof(string)).Caption = "ReadingDate";


            foreach (var Group in oxyGroups)
            {
                var row = reportViewModel.OxygenSaturationTable.NewRow();
                row[0] = Group.Spo2;
                row[1] = Group.HeartRate;
                row[2] = Convert.ToDateTime(Group.ReadingDate.ToString()).ToString("MM/dd/yyyy hh:mm tt");

                reportViewModel.OxygenSaturationTable.Rows.Add(row);
            }
            return Json(reportViewModel.OxygenSaturationTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSummaryReport(string userId, string StartDate, string EndDate, string UserName)
        {

            String fileName = UserName+" Measurement Report.xlsx";
            String contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Create the file using the FileInfo object
            var file = new FileInfo(fileName);
            using (var package = new ExcelPackage(file))
            {
                
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Measurement Report " + DateTime.Now);

                // --------- Data and styling goes here -------------- //
                // Add some formatting to the worksheet
                worksheet.TabColor = System.Drawing.Color.Blue;
                worksheet.DefaultRowHeight = 12;
                worksheet.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());
                worksheet.Row(1).Height = 20;
                worksheet.Row(2).Height = 20;

                // Formats and Styles The First Row Header
                worksheet.Cells[1, 1, 1, 9].Merge = true;
                worksheet.Cells[1, 1].Value = UserName + "Measurement Report";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue);
                worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
                worksheet.Cells[1, 1].Style.ShrinkToFit = false;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Second Header
                worksheet.Cells[2, 1].Value = "Date";
                worksheet.Cells[2, 2].Value = "Weight Transmission Time";
                worksheet.Cells[2, 3].Value = "Weight";
                worksheet.Cells[2, 4].Value = "BP Transmission Time";
                worksheet.Cells[2, 5].Value = "Systolic";
                worksheet.Cells[2, 6].Value = "Diastolic";
                worksheet.Cells[2, 7].Value = "Heart Rate";
                worksheet.Cells[2, 8].Value = "Ox Transmission Time";
                worksheet.Cells[2, 9].Value = "Oxygen Saturation";

                //Formats and Styles The Second Row Header;
                using (var range = worksheet.Cells[2, 1, 2, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                    range.Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
                    range.Style.ShrinkToFit = false;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                ///Gets Weight Data from DB
                var weightGroups = db.GetWeightMeasurements(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().Select(x => new WeightViewModel
                {
                    UserId = x.UserId,
                    Weight = x.Weight,
                    ReadingDate = x.ReadingDate,
                }).ToList();

                ///Gets BloodPressure Data from DB
                var bpGroups = db.GetBloodPressureMeasurements(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().Select(x => new BloodPressureViewModel
                {
                    UserId = x.UserId,
                    Systolic = x.Systolic,
                    Diastolic = x.Diastolic,
                    Map = x.Map,
                    ReadingDate = x.ReadingDate,
                }).ToList();

                ///Gets Oxygen Saturation Data from DB
                var oxyGroups = db.GetOxygenSaturation(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().Select(x => new OxygenSaturationViewModel
                {
                    UserId = x.UserId,
                    SpO2 = x.SpO2,
                    ReadingDate = x.ReadingDate,
                }).ToList();

                // Keep track of the row that we're starting on, but start with row three
                int rowNumber = 3;

                DateTime startDate = Convert.ToDateTime(StartDate);
                int DateDifference = ((int)(Convert.ToDateTime(EndDate) - Convert.ToDateTime(StartDate)).TotalDays);
                Boolean hasValue = false;

                for (int i = 0; i <= DateDifference;) // Loop through days starting with the StartDate and ending in the EndDate.
                {
                  
                    var currentDateWeight = weightGroups.FindAll(p => p.ReadingDate.Date == startDate.AddDays(i).Date);
                    var currentDateBp = bpGroups.FindAll(p => p.ReadingDate.Date == startDate.AddDays(i).Date);
                    var currentDateOxy = oxyGroups.FindAll(p => p.ReadingDate.Date == startDate.AddDays(i).Date);

                    int rowNumberTemp = rowNumber;

                    if (currentDateWeight.Count > 0)
                    {
                        hasValue = true;
                        foreach (var weightValue in currentDateWeight)
                        {
                            worksheet.Cells[rowNumberTemp, 2].Value = weightValue.ReadingDate.ToString("HH:mm:ss");
                            worksheet.Cells[rowNumberTemp, 3].Value = weightValue.Weight.ToString();
                            worksheet.Cells[rowNumberTemp, 1, rowNumberTemp, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            rowNumberTemp += 1;

                        }
                        //reset row
                        rowNumberTemp = rowNumber;
                    }
                    if (currentDateBp.Count > 0)
                    {
                        hasValue = true;
                        foreach (var bpValue in currentDateBp)
                        {
                            worksheet.Cells[rowNumberTemp, 4].Value = bpValue.ReadingDate.ToString("HH:mm:ss");
                            worksheet.Cells[rowNumberTemp, 5].Value = bpValue.Systolic.ToString();
                            worksheet.Cells[rowNumberTemp, 6].Value = bpValue.Diastolic.ToString();
                            worksheet.Cells[rowNumberTemp, 7].Value = bpValue.Map.ToString();
                            worksheet.Cells[rowNumberTemp, 1, rowNumberTemp, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowNumberTemp += 1;

                        }
                        //reset row
                        rowNumberTemp = rowNumber;
                    }

                    if (currentDateOxy.Count > 0)
                    {
                        hasValue = true;
                        foreach (var oxyValue in currentDateOxy)
                        {
                            worksheet.Cells[rowNumberTemp, 8].Value = oxyValue.ReadingDate.ToString("HH:mm:ss");
                            worksheet.Cells[rowNumberTemp, 9].Value = oxyValue.SpO2.ToString();
                            worksheet.Cells[rowNumberTemp, 1, rowNumberTemp, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowNumberTemp += 1;
                        }
                        //reset row
                        rowNumberTemp = rowNumber;
                    }
                    if (hasValue)
                    {
                        //Set the Date for column one                 
                        worksheet.Cells[rowNumber, 1].Value = startDate.AddDays(i).ToString("MM/dd/yyyy");

                        // Align Rows Horizontal
                        worksheet.Cells[rowNumber, 1, rowNumber, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowNumber = Math.Max(Math.Max(currentDateWeight.Count, currentDateBp.Count), currentDateOxy.Count) + rowNumber;
                        hasValue = false;
                    }
                    i++;
                }
             
                //Fit the columns according to its content
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();
                worksheet.Column(3).AutoFit();
                worksheet.Column(4).AutoFit();
                worksheet.Column(5).AutoFit();
                worksheet.Column(6).AutoFit();
                worksheet.Column(7).AutoFit();
                worksheet.Column(8).AutoFit();
                worksheet.Column(9).AutoFit();

                // Set some document properties
                package.Workbook.Properties.Title = "User Measurement Report";
                package.Workbook.Properties.Author = "Tachl";
                package.Workbook.Properties.Company = "Tachl";

                return File(package.GetAsByteArray(), contentType, fileName);

            }
        }

        [HttpGet]
        public ActionResult SurveyResponses()
        {
            var vm = new SurveyResponsesViewModel();
            var users = db.GetUsers().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList().OrderBy(x => x.LastName);
            vm.UserSelectList = new SelectList(users, "UserId", "DisplayName");
            vm.StartDate = DateTime.Now;
            vm.EndDate = DateTime.Now;

            return View(vm);
        }

        [HttpGet]
        public ActionResult ArchivedSurveyResponses()
        {
            var vm = new SurveyResponsesViewModel();
            var users = db.GetArchivedUsers().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList().OrderBy(x => x.LastName);
            vm.UserSelectList = new SelectList(users, "UserId", "DisplayName");
            vm.StartDate = DateTime.Now;
            vm.EndDate = DateTime.Now;

            return View(vm);
        }

        public ActionResult SurveyResponses([DataSourceRequest]DataSourceRequest request, string userId, string StartDate, string EndDate)
        {
            var surveyanswer = db.GetSurveyResponseReport(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderByDescending(x => x.AnswerDate);

            return Json(surveyanswer.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveExcelSurveyResponses(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);

            //String fileName = UserName + " Survey Responses Report.xlsx";
            //String contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //// Create the file using the FileInfo object
            //var file = new FileInfo(fileName);
            //using (var package = new ExcelPackage(file))
            //{
            //    // add a new worksheet to the empty workbook
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Survey Responses Report " + DateTime.Now);

            //    // --------- Data and styling goes here -------------- //
            //    // Add some formatting to the worksheet
            //    worksheet.TabColor = System.Drawing.Color.Blue;
            //    worksheet.DefaultRowHeight = 12;
            //    worksheet.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());
            //    worksheet.Row(1).Height = 20;
            //    worksheet.Row(2).Height = 20;

            //    // Formats and Styles The First Row Header
            //    worksheet.Cells[1, 1, 1, 4].Merge = true;
            //    worksheet.Cells[1, 1].Value = UserName + "Survey Responses Report";
            //    worksheet.Cells[1, 1].Style.Font.Bold = true;
            //    worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue);
            //    worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
            //    worksheet.Cells[1, 1].Style.ShrinkToFit = false;
            //    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //    // Second Header
            //    worksheet.Cells[2, 1].Value = "Date";
            //    worksheet.Cells[2, 2].Value = "Question";
            //    worksheet.Cells[2, 3].Value = "Answer";
            //    worksheet.Cells[2, 4].Value = "Submission Date";

            //    //Formats and Styles The Second Row Header;
            //    using (var range = worksheet.Cells[2, 1, 2, 4])
            //    {
            //        range.Style.Font.Bold = true;
            //        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
            //        range.Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
            //        range.Style.ShrinkToFit = false;
            //        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //    }

            //    var surveyRespnses = db.GetSurveyResponseReport(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderByDescending(x => x.AnswerDate);


            //    // Keep track of the row that we're starting on, but start with row three
            //    int rowNumber = 3;

            //    DateTime startDate = Convert.ToDateTime(StartDate);
            //    int DateDifference = ((int)(Convert.ToDateTime(EndDate) - Convert.ToDateTime(StartDate)).TotalDays);

            //    for (int i = 0; i <= DateDifference;) // Loop through days starting with the StartDate and ending in the EndDate.
            //    {

            //        //Set the Date for column one                 
            //        worksheet.Cells[rowNumber, 1].Value = startDate.AddDays(i).ToString("MM/dd/yyyy");

            //        // Align Rows Horizontal
            //        worksheet.Cells[rowNumber, 1, rowNumber, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //        //var currentDateWeight = weightGroups.FindAll(p => p.ReadingDate.Date == startDate.AddDays(i).Date);
            //        //if (currentDateWeight.Count > 0)
            //        //{
            //        //    worksheet.Cells[rowNumber, 2].Value = string.Join(", ", currentDateWeight.Select(x => x.Weight.ToString()).ToArray());
            //        //}

            //        rowNumber += 1;
            //        i++;
            //    }

            //    //Fit the columns according to its content
            //    worksheet.Column(1).AutoFit();
            //    worksheet.Column(2).AutoFit();
            //    worksheet.Column(3).AutoFit();
            //    worksheet.Column(4).AutoFit();
            //    worksheet.Column(5).AutoFit();
            //    worksheet.Column(6).AutoFit();

            //    // Set some document properties
            //    package.Workbook.Properties.Title = "User Measurement Report";
            //    package.Workbook.Properties.Author = "Tachl";
            //    package.Workbook.Properties.Company = "Tachl";

            //    return File(package.GetAsByteArray(), contentType, fileName);

            //}
        }

        [HttpGet]
        public ActionResult SaveExcelSurveyResponses(string userId, string StartDate, string EndDate, string UserName)
        {

            String fileName = UserName + " Survey Responses Report.xlsx";
            String contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Create the file using the FileInfo object
            var file = new FileInfo(fileName);
            using (var package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Survey Responses Report " + DateTime.Now);

                // --------- Data and styling goes here -------------- //
                // Add some formatting to the worksheet
                worksheet.TabColor = System.Drawing.Color.Blue;
                worksheet.DefaultRowHeight = 12;
                worksheet.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());
                worksheet.Row(1).Height = 20;
                worksheet.Row(2).Height = 20;

                // Formats and Styles The First Row Header
                worksheet.Cells[1, 1, 1, 4].Merge = true;
                worksheet.Cells[1, 1].Value = UserName + "Survey Responses Report";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue);
                worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
                worksheet.Cells[1, 1].Style.ShrinkToFit = false;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Second Header
                worksheet.Cells[2, 1].Value = "Date";
                worksheet.Cells[2, 2].Value = "Question";
                worksheet.Cells[2, 3].Value = "Answer";
                worksheet.Cells[2, 4].Value = "Submission Date";

                //Formats and Styles The Second Row Header;
                using (var range = worksheet.Cells[2, 1, 2, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                    range.Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
                    range.Style.ShrinkToFit = false;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                var surveyResponses = db.GetSurveyResponseReport(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderByDescending(x => x.AnswerDate);


                // Keep track of the row that we're starting on, but start with row three
                int rowNumber = 3;


                DateTime srAnswerDate = surveyResponses.Select(x => x.AnswerDate).FirstOrDefault();
                foreach (var surveyResponse in surveyResponses) {
                    if (srAnswerDate == null || rowNumber == 3)
                    {
                        //Set the Date for date column one                 
                        worksheet.Cells[rowNumber, 1].Value = surveyResponse.AnswerDate.ToString("MM/dd/yyyy");
                        worksheet.Cells[rowNumber, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowNumber, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    }
                    else if (srAnswerDate.Date != surveyResponse.AnswerDate.Date) {
                        rowNumber += 1;
                        worksheet.Cells[rowNumber, 1].Value = surveyResponse.AnswerDate.ToString("MM/dd/yyyy");
                        worksheet.Cells[rowNumber, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowNumber, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    }
                    else {
                        worksheet.Cells[rowNumber, 1].Value = "";
                    }
                   
                    worksheet.Cells[rowNumber, 2].Value = surveyResponse.QuestionText;
                    worksheet.Cells[rowNumber, 3].Value = surveyResponse.AnswerText;
                    worksheet.Cells[rowNumber, 4].Value = surveyResponse.AnswerDate.ToString("h:mm:ss tt");

                    // Align Rows Horizontal
                    worksheet.Cells[rowNumber, 1, rowNumber, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowNumber += 1;
                    srAnswerDate = surveyResponse.AnswerDate;
                }

                //Fit the columns according to its content
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();
                worksheet.Column(3).AutoFit();
                worksheet.Column(4).AutoFit();

                // Set some document properties
                package.Workbook.Properties.Title = "User Measurement Report";
                package.Workbook.Properties.Author = "Tachl";
                package.Workbook.Properties.Company = "Tachl";

                return File(package.GetAsByteArray(), contentType, fileName);

            }
        }

        [HttpPost]
        public ActionResult SaveSurveyResponses(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            var pdffile = File(fileContents, contentType, fileName);

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/Ghostscript/");
            //string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");

            // Set ghostscript directory
            MagickNET.SetGhostscriptDirectory(path);

            MagickReadSettings settings = new MagickReadSettings();
            // Settings the density to 200 dpi will create an image with a better quality
            settings.Density = new Density(200);

            using (MagickImageCollection images = new MagickImageCollection())
            {
                // Add all the pages of the pdf file to the collection
                images.Read(fileContents, settings);

                // Create new image that appends all the pages horizontally
                using (IMagickImage verticalImages = images.AppendVertically())
                {
                    // Sets the output format to jpeg
                    verticalImages.Format = MagickFormat.Jpeg;
                    // Create byte array that contains a jpeg file
                    byte[] data = verticalImages.ToByteArray();
                    System.IO.File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images/" + "SurveyImage.jpeg"), data);
                    //Response.Clear();
                    //MemoryStream ms = new MemoryStream(data);
                    //Response.ContentType = "image/jpeg";
                    //Response.AddHeader("content-disposition", "filename="+ DateTime.Today.ToString("MM/dd/yyyy") + " "+fileName+".jpg");
                    //Response.Buffer = true;
                    //ms.WriteTo(Response.OutputStream);
                    //Response.Flush();
                    //Response.End();
                }
            }

            ///never return/downloaded via client
            return Redirect("~/images/" + fileName);

        }

        [HttpGet]
        public ActionResult GenerateReport()
        {
            String currentDate = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy") + " 12:30 PM - " + DateTime.Now.ToString("MM/dd/yyyy") + " 12:30 PM";

            var surveyReport = db.GetSurveyReport(3).ToList();
            var surveyReportGroups = surveyReport.GroupBy(x => x.UserId);
            var surveyQuestionHD = db.GetSurveyQuestionsForNotificationService().ToList();

            // Move the image question to the left two indexs.
            var imageQuestion = surveyQuestionHD[8];
            surveyQuestionHD.RemoveAt(8);
            surveyQuestionHD.Insert(6, imageQuestion);


            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("spurstech.send@gmail.com", "$pur$tech1234");

                var msg = new MailMessage("developer@spurstech.com", User.Identity.Name);

                msg.Subject = "Heart Valve - Daily Assessment Report";
                msg.IsBodyHtml = true;

                msg.Body = "<table style='width: 100%;'>";
                msg.Body += "<tr><td style='background-color: #eee; color: #222; width: 100%; padding: 20px 0 30px 20px; font-family: Georgia; font-weight: normal;'>";
                msg.Body += "<h1>Daily Assessment Report</h1>";
                msg.Body += "<span style='color: #555; font: 12px Verdana;'>Automatic Notification - " + currentDate + "</span>";
                msg.Body += "</td></tr>";
                msg.Body += "</table><br/><br/>";
                msg.Body += "<table cellspacing='0' cellpadding='10' style='border: solid 1px #aaa; font: normal 12px Tahoma;'>";
                msg.Body += "<tr>";
                msg.Body += "<th style='border: solid 1px #000; background: #eee; width: 200px;'>Patient</th>";

                foreach (var question in surveyQuestionHD)
                {
                    msg.Body += "<th style='border: solid 1px #000; background: #eee; width: 200px;'>" + question.QuestionText + "</th>";

                }

                msg.Body += "<th style='border: solid 1px #000; background: #eee; width: 200px;'> Survey Time Span</th>";

                msg.Body += "</tr>";


                List<TableItems> tableRowList = new List<TableItems>();
                List<String> UncompletedSurveyUsers = new List<String>();

                foreach (var group in surveyReportGroups)
                {
                    TableItems tr = new TableItems();
                    tr.errorCount = 0;
                    tr.unAsnwerCount = 0;
                    tr.asnweredCount = 0;

                    tr.userName = group.First().LastName + ", " + group.First().FirstName;
                    tr.tableRow += "<tr>";
                    tr.tableRow += "<td style='border: solid 1px #000;'>" + group.First().LastName + ", " + group.First().FirstName + "</td>";


                    bool isSelected = false;
                    var dataSet = group.ToList();

                    //Iterates through survey questions
                    foreach (var sQuestion in surveyQuestionHD)
                    {
                        isSelected = false;

                        //Populates td.
                        for (int j = 0; j < dataSet.Count; j++)
                        {
                            if (j + 1 < dataSet.Count)
                            {
                                if (sQuestion.QuestionId == dataSet[j + 1].QuestionId)
                                {
                                    continue;
                                }
                            }

                            // Checks if question i is equal to the questionID.
                            if (sQuestion.QuestionId == dataSet[j].QuestionId)
                            {
                                if (dataSet[j].HasCompleted == true ? true : false)
                                {
                                    if (dataSet[j].QuestionId == 1 && dataSet[j].OptionValue == 1)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red; width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + " </strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 2 && (dataSet[j].OptionValue == 3 || dataSet[j].OptionValue == 4))
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                        "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 3 && dataSet[j].OptionValue == 1)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 4 && dataSet[j].OptionValue == 1)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 5 && dataSet[j].OptionValue == 0)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 6 && dataSet[j].OptionValue == 1)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 7 && dataSet[j].OptionValue == 1)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 8 && (dataSet[j].OptionValue == 4 || dataSet[j].OptionValue == 5))
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 9)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + "Check portal for image!" + "</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else if (dataSet[j].QuestionId == 10)
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: red;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + "Check portal for image!</strong></td>";
                                        tr.errorCount += 1;
                                        isSelected = true;
                                        continue;
                                    }
                                    else
                                    {
                                        tr.tableRow += "<td style='border: solid 1px #000; background: green;width: 200px; text-align:center; font-size:1.5em;'>" +
                                         "<strong>" + dataSet[j].AnswerText + "</strong></td>";
                                        tr.asnweredCount += 1;
                                        isSelected = true;
                                    }
                                }

                            }//End of check Id.

                        }//Populates td.

                        //checks if the question was not asked during the Daily Assessment
                        if (isSelected == false)
                        {
                            tr.tableRow += "<td style='border: solid 1px #000; background: #fff;width: 200px'> <strong><div style='text-align:center; font-size:1.5em;width:20px;height:20px;'>N/A</strong></td>";
                            tr.unAsnwerCount += 1;
                        }
                    }

                    if (group.First().EndDate != null)
                    {
                        TimeSpan span = group.First().EndDate.Value - group.First().StartDate.Value;
                        string totalTime = String.Format("{0} minutes, {1} seconds",
                        span.Minutes, span.Seconds);

                        tr.tableRow += "<td style='border: solid 1px #000;'>" + "<strong>Time Span:</strong> " + totalTime + "</br><strong>Start Time:</strong> " + group.First().StartDate.Value.ToString("MM/dd/yyyy hh:mm tt") + "</br> <strong>End Time:</strong> " + group.First().EndDate.Value.ToString("MM/dd/yyyy hh:mm tt") + "</td>";
                        tr.tableRow += "</tr>";
                        if (tr.unAsnwerCount < 10)
                        {
                            tableRowList.Add(tr);
                        }
                        else
                        {
                            UncompletedSurveyUsers.Add(tr.userName + "(" + " Last Transmission: " + group.First().EndDate.Value.ToString("MM/dd/yyyy hh:mm tt") + ")");
                        }
                    }
                    else {

                        tr.tableRow += "<td style='border: solid 1px #000;'> Survey Ended(User Error)</td>";
                        tr.tableRow += "</tr>";
                        if (tr.unAsnwerCount < 10)
                        {
                            tableRowList.Add(tr);
                        }
                        else
                        {
                            UncompletedSurveyUsers.Add(tr.userName + "(" + "Last Transmission: Incomplete Survey Session)");
                        }
                    }
                }// for loop for users.

                foreach (TableItems item in tableRowList.OrderByDescending(t => t.errorCount))
                {

                    msg.Body += item.tableRow;
                }

                msg.Body += "</table><h2 style='text-decoration:underline;'>No Survey Transmission</h2>";
                msg.Body += "<div> <ul type='1'>";

                foreach (String names in UncompletedSurveyUsers)
                {
                    if (names != "Zzztest, Beverly" && !names.Contains("tachl"))
                    {
                        msg.Body += "<li><h3>" + names + "</h3></li>";
                    }
                }
                msg.Body += "</ul></div>";

                msg.Priority = MailPriority.Normal;
                client.Send(msg);
            }
            return RedirectToAction("Index", "Home");
        }

        public class TableItems
        {
            public string tableRow { get; set; }
            public int asnweredCount { get; set; }
            public int errorCount { get; set; }
            public int unAsnwerCount { get; set; }
            public string userName { get; set; }
        }
    }
}

//[HttpGet]
//public ActionResult SaveExcelSurveyResponses(string userId, string StartDate, string EndDate, string UserName)
//{

//    String fileName = UserName + " Survey Responses Report.xlsx";
//    String contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

//    // Create the file using the FileInfo object
//    var file = new FileInfo(fileName);
//    using (var package = new ExcelPackage(file))
//    {
//        // add a new worksheet to the empty workbook
//        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Survey Responses Report " + DateTime.Now);

//        // --------- Data and styling goes here -------------- //
//        // Add some formatting to the worksheet
//        worksheet.TabColor = System.Drawing.Color.Blue;
//        worksheet.DefaultRowHeight = 12;
//        worksheet.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());
//        worksheet.Row(1).Height = 20;
//        worksheet.Row(2).Height = 20;

//        // Formats and Styles The First Row Header
//        worksheet.Cells[1, 1, 1, 4].Merge = true;
//        worksheet.Cells[1, 1].Value = UserName + "Survey Responses Report";
//        worksheet.Cells[1, 1].Style.Font.Bold = true;
//        worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
//        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue);
//        worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
//        worksheet.Cells[1, 1].Style.ShrinkToFit = false;
//        worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

//        //Second Header               
//        worksheet.Cells[2, 1].Value = "Questions";
//        worksheet.Cells[2, 2].Value = "Date";

//        //Formats and Styles The Second Row Header;
//        using (var range = worksheet.Cells[2, 1, 2, 2])
//        {
//            range.Style.Font.Bold = true;
//            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
//            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
//            range.Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
//            range.Style.ShrinkToFit = false;
//            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//        }


//        var questions = db.GetSurveyQuestions(3).OrderBy(x => x.QuestionOrder).ToList();

//        // Keep track of the row that we're starting on, but start with row three
//        int rowNumber = 3;
//        int tempRow = rowNumber;
//        DateTime startDate = Convert.ToDateTime(StartDate);
//        int DateDifference = ((int)(Convert.ToDateTime(EndDate) - Convert.ToDateTime(StartDate)).TotalDays);

//        for (int i = 0; i > questions.Count; i++)
//        {
//            worksheet.Cells[tempRow, 1].Value = questions[i].QuestionText;
//            tempRow += 1;
//        }

//        var surveyResponses = db.GetSurveyResponseReport(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().OrderByDescending(x => x.AnswerDate);


//        for (int i = 0; i <= DateDifference;) // Loop through days starting with the StartDate and ending in the EndDate.
//        {


//            var currentSurveyDate = surveyResponses.Where(p => p.AnswerDate == startDate.AddDays(i).Date).ToList();

//            if (currentSurveyDate.Count > 0)
//            {
//                foreach (var value in currentSurveyDate)
//                {

//                }

//            }





//            DateTime srAnswerDate = surveyResponses.Select(x => x.AnswerDate).FirstOrDefault();
//            foreach (var surveyResponse in surveyResponses)
//            {
//                if (srAnswerDate == null || rowNumber == 3)
//                {
//                    //Set the Date for date column one                 
//                    worksheet.Cells[rowNumber, 1].Value = surveyResponse.AnswerDate.ToString("MM/dd/yyyy");
//                    worksheet.Cells[rowNumber, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
//                    worksheet.Cells[rowNumber, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

//                }
//                else if (srAnswerDate.Date != surveyResponse.AnswerDate.Date)
//                {
//                    rowNumber += 1;
//                    worksheet.Cells[rowNumber, 1].Value = surveyResponse.AnswerDate.ToString("MM/dd/yyyy");
//                    worksheet.Cells[rowNumber, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
//                    worksheet.Cells[rowNumber, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
//                }
//                else
//                {
//                    worksheet.Cells[rowNumber, 1].Value = "";
//                }

//                worksheet.Cells[rowNumber, 2].Value = surveyResponse.QuestionText;
//                worksheet.Cells[rowNumber, 3].Value = surveyResponse.AnswerText;
//                worksheet.Cells[rowNumber, 4].Value = surveyResponse.AnswerDate.ToString("h:mm:ss tt");

//                // Align Rows Horizontal
//                worksheet.Cells[rowNumber, 1, rowNumber, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
//                rowNumber += 1;
//                srAnswerDate = surveyResponse.AnswerDate;
//            }

//            //Fit the columns according to its content
//            worksheet.Column(1).AutoFit();
//            worksheet.Column(2).AutoFit();
//            worksheet.Column(3).AutoFit();
//            worksheet.Column(4).AutoFit();

//            // Set some document properties
//            package.Workbook.Properties.Title = "User Measurement Report";
//            package.Workbook.Properties.Author = "Tachl";
//            package.Workbook.Properties.Company = "Tachl";

//            return File(package.GetAsByteArray(), contentType, fileName);

//        }
//    }
//}