using EntityFrameworkExtras.EF6;
using HeartValve.Models;
using HeartValve.Shared.Models;
using HeartValve.Shared.StoredProcedures;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace HeartValve.Controllers
{
    [Authorize(Roles = "Admin,Provider")]
    public class UsersController : ApplicationController
    {
        [HttpGet]
        public ActionResult AdminIndex()
        {
            var units = db.GetMedicationUnits().OrderBy(x => x.Unit).Select(x => new SelectListItem
            {
                Text = x.Unit,
                Value = x.Unit
            }).ToList();
            var dosageForms = db.GetMedicationDosageForms().OrderBy(x => x.DosageForm).Select(x => new SelectListItem
            {
                Text = x.DosageForm,
                Value = x.DosageForm
            }).ToList();
            var routes = db.GetMedicationRoutes().OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();
            var scheduleTypes = db.GetMedicationScheduleTypes().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ScheduleTypeId.ToString()
            }).ToList();

            ViewData["UnitSelectList"] = units;
            ViewData["DosageFormSelectList"] = dosageForms;
            ViewData["RouteSelectList"] = routes;
            ViewData["ScheduleTypeSelectList"] = scheduleTypes;

            return View();
        }

        public ActionResult Index()
        {

            var cuffsizes = db.GetBPMCuffSizeLU().Select(x => new BPCuffSize()
            {
                Size = x.Size,
                BPCuffSizeId = x.BpCuffSizeId
            }).ToList();

            ViewBag.BpCuffList = cuffsizes;

            return View();
        }
        public ActionResult ActiveUsers()
        {
            return View();
        }
        public ActionResult InactiveUsers()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
        {
            var users = db.GetEveryUsers().ToList();
            DataSourceResult result = users.AsQueryable().ToDataSourceResult(request, c => c);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AdminUsers_Read([DataSourceRequest]DataSourceRequest request)
        {
            var users = db.GetEveryUsersAdmin().ToList();
            DataSourceResult result = users.AsQueryable().ToDataSourceResult(request, c => c);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Users_ReadActive([DataSourceRequest]DataSourceRequest request)
        {
            var users = db.GetUsers().ToList().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                EnrollmentDate = x.EnrollmentDate,
                Email = x.Email,
                IsActive = x.IsActive,
                SystolicMinThreshold = x.SystolicMinThreshold,
                SystolicMaxThreshold = x.SystolicMaxThreshold,
                DiastolicMinThreshold = x.DiastolicMinThreshold,
                DiastolicMaxThreshold = x.DiastolicMaxThreshold,
                HeartRateMinThreshold = x.HeartRateMinThreshold,
                HeartRateMaxThreshold = x.HeartRateMaxThreshold,
                WeightMinThreshold = x.WeightMinThreshold,
                WeightMaxThreshold = x.WeightMaxThreshold,
                Sp02MinThreshold = x.SpO2MinThreshold,
                Sp02MaxThreshold = x.SpO2MaxThreshold,
                MRN = x.MRN
            });

            return Json(users.ToDataSourceResult(request));
        }

        public ActionResult Users_ReadInactive([DataSourceRequest]DataSourceRequest request)
        {
            var users = db.GetArchivedUsers().ToList().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                EnrollmentDate = x.EnrollmentDate,
                Email = x.Email,
                IsActive = x.IsActive,
                SystolicMinThreshold = x.SystolicMinThreshold,
                SystolicMaxThreshold = x.SystolicMaxThreshold,
                DiastolicMinThreshold = x.DiastolicMinThreshold,
                DiastolicMaxThreshold = x.DiastolicMaxThreshold,
                HeartRateMinThreshold = x.HeartRateMinThreshold,
                HeartRateMaxThreshold = x.HeartRateMaxThreshold,
                WeightMinThreshold = x.WeightMinThreshold,
                WeightMaxThreshold = x.WeightMaxThreshold,
                Sp02MinThreshold = x.SpO2MinThreshold,
                Sp02MaxThreshold = x.SpO2MaxThreshold,
                MRN = x.MRN
            });

            return Json(users.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Users_Create([DataSourceRequest]DataSourceRequest request, UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(vm.Password))
                {
                    ModelState.AddModelError("User", "The Password field is required.");
                }

                if (string.IsNullOrEmpty(vm.PasswordVerify))
                {
                    ModelState.AddModelError("User", "You must verify your password.");
                }

                if (vm.Password != vm.PasswordVerify)
                {
                    ModelState.AddModelError("User", "The passwords do not match.");
                }

                if (ModelState.IsValid)
                {
                    using (var ts = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new ApplicationUser() { UserName = vm.Email, Email = vm.Email };
                            IdentityResult result = UserManager.Create(user, vm.Password);

                            if (result.Succeeded)
                            {

                                db.AddUpdateUserAdmin(user.Id, vm.FirstName, vm.LastName, vm.PhoneNumber, vm.EnrollmentDate, vm.SystolicMinThreshold, vm.SystolicMaxThreshold,
                                vm.DiastolicMinThreshold, vm.DiastolicMaxThreshold, vm.HeartRateMinThreshold, vm.HeartRateMaxThreshold, vm.Sp02MinThreshold, vm.Sp02MaxThreshold,
                                vm.WeightMinThreshold, vm.WeightMaxThreshold, vm.MRN, true, vm.DeliveryDate, vm.BpCuffSizeId,false).SingleOrDefault();
                                ts.Commit();

                                vm.UserId = user.Id;
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
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Users_Update([DataSourceRequest]DataSourceRequest request, UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateUser(vm.UserId, vm.FirstName, vm.LastName, vm.PhoneNumber, vm.EnrollmentDate, vm.SystolicMinThreshold, vm.SystolicMaxThreshold,
                                vm.DiastolicMinThreshold, vm.DiastolicMaxThreshold, vm.HeartRateMinThreshold, vm.HeartRateMaxThreshold,
                                vm.Sp02MinThreshold, vm.Sp02MaxThreshold, vm.WeightMinThreshold, vm.WeightMaxThreshold, vm.MRN, vm.IsActive, vm.DeliveryDate, vm.BpCuffSizeId).SingleOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult AdminUsers_Update([DataSourceRequest]DataSourceRequest request, UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateUserAdmin(vm.UserId, vm.FirstName, vm.LastName, vm.PhoneNumber, vm.EnrollmentDate, vm.SystolicMinThreshold, vm.SystolicMaxThreshold,
                                vm.DiastolicMinThreshold, vm.DiastolicMaxThreshold, vm.HeartRateMinThreshold, vm.HeartRateMaxThreshold,
                                vm.Sp02MinThreshold, vm.Sp02MaxThreshold, vm.WeightMinThreshold, vm.WeightMaxThreshold, vm.MRN, vm.IsActive, vm.DeliveryDate, vm.BpCuffSizeId,vm.IsInStudy).SingleOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Users_Activate([DataSourceRequest]DataSourceRequest request, UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateUser(vm.UserId, vm.FirstName, vm.LastName, vm.PhoneNumber, vm.EnrollmentDate, vm.SystolicMinThreshold, vm.SystolicMaxThreshold,
                                vm.DiastolicMinThreshold, vm.DiastolicMaxThreshold, vm.HeartRateMinThreshold, vm.HeartRateMaxThreshold,
                                vm.Sp02MinThreshold, vm.Sp02MaxThreshold, vm.WeightMinThreshold, vm.WeightMaxThreshold, vm.MRN, true, vm.DeliveryDate, vm.BpCuffSizeId).SingleOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Users_Deactivate([DataSourceRequest]DataSourceRequest request, UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateUser(vm.UserId, vm.FirstName, vm.LastName, vm.PhoneNumber, vm.EnrollmentDate, vm.SystolicMinThreshold, vm.SystolicMaxThreshold,
                                vm.DiastolicMinThreshold, vm.DiastolicMaxThreshold, vm.HeartRateMinThreshold, vm.HeartRateMaxThreshold,
                                vm.Sp02MinThreshold, vm.Sp02MaxThreshold, vm.WeightMinThreshold, vm.WeightMaxThreshold, vm.MRN, false, vm.DeliveryDate, vm.BpCuffSizeId).SingleOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UserMedications_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var userMedications = db.GetUserMedications(userId).ToList().Select(x => new UserMedicationViewModel()
            {
                UserMedicationId = x.UserMedicationId,
                UserId = x.UserId,
                MedicationId = x.MedicationId,
                Medication = new MedicationViewModel()
                {
                    MedicationId = x.MedicationId,
                    BrandName = x.BrandName,
                    GenericName = x.GenericName,
                    Strength = x.Strength,
                    Unit = x.Unit,
                    DosageForm = x.DosageForm,
                    Route = x.Route
                },
                Quantity = x.Quantity,
                Strength = x.Strength,
                Unit = x.Unit,
                DosageForm = x.DosageForm,
                Route = x.Route,
                Dose = x.Dose,
                Frequency = x.Frequency,
                MedicationScheduleTypeId = x.MedicationScheduleTypeId,
                Indication = x.Indication,
                Instructions = x.Instructions
            });

            return Json(userMedications.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult UserMedications_Create([DataSourceRequest]DataSourceRequest request, UserMedicationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var userMedicationId = db.AddUpdateUserMedication(vm.UserMedicationId, vm.UserId, vm.MedicationId, vm.Quantity ?? decimal.Zero, vm.Strength, vm.Unit,
                    vm.DosageForm, vm.Route, vm.Dose, vm.Frequency, (int?)null, vm.MedicationScheduleTypeId, vm.Indication, vm.Instructions).SingleOrDefault();
                vm.UserMedicationId = userMedicationId.Value;
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UserMedications_Update([DataSourceRequest]DataSourceRequest request, UserMedicationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateUserMedication(vm.UserMedicationId, vm.UserId, vm.MedicationId, vm.Quantity ?? decimal.Zero, vm.Strength, vm.Unit,
                    vm.DosageForm, vm.Route, vm.Dose, vm.Frequency, (int?)null, vm.MedicationScheduleTypeId, vm.Indication, vm.Instructions).SingleOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UserSchedules_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var schedules = db.GetMedicationSchedules(userId).ToList().GroupBy(x => x.GroupId).Select(x => new ScheduleGroupViewModel()
            {
                UserId = userId,
                UserMedicationId = x.First().UserMedicationId.Value,
                UserMedication = new UserMedicationViewModel()
                {
                    UserMedicationId = x.First().UserMedicationId.Value,
                    Name = x.First().BrandName,
                    Strength = x.First().Strength,
                    Unit = x.First().Unit
                },
                ScheduleTypeId = x.First().ScheduleTypeId,
                StartDate = x.First().StartDate,
                EndDate = x.First().EndDate,
                ScheduleTime = DateTime.Today + x.First().ScheduleTime,
                Active = !x.First().Inactive,
                Dose = x.First().Dose,
                GroupId = x.Key.GetValueOrDefault(),
                Sunday = x.Any(y => y.DayOfWeek == 1),
                Monday = x.Any(y => y.DayOfWeek == 2),
                Tuesday = x.Any(y => y.DayOfWeek == 3),
                Wednesday = x.Any(y => y.DayOfWeek == 4),
                Thursday = x.Any(y => y.DayOfWeek == 5),
                Friday = x.Any(y => y.DayOfWeek == 6),
                Saturday = x.Any(y => y.DayOfWeek == 7)
            }).OrderBy(x => x.UserMedication.MedicationId);

            return Json(schedules.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult UserSchedules_Create([DataSourceRequest]DataSourceRequest request, ScheduleGroupViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var schedules = db.GetMedicationSchedules(vm.UserId).Where(x => x.GroupId == vm.GroupId).Select(x => new MedicationScheduleType()
                {
                    ScheduleId = x.ScheduleId,
                    UserId = x.UserId,
                    UserMedicationId = x.UserMedicationId.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek,
                    ScheduleTime = x.ScheduleTime,
                    GroupId = x.GroupId,
                    Dose = x.Dose
                }).ToList();

                foreach (var schedule in schedules)
                {
                    schedule.StartDate = vm.StartDate;
                    schedule.EndDate = vm.EndDate;
                    schedule.ScheduleTime = vm.ScheduleTime.Value.TimeOfDay;
                    schedule.Dose = vm.Dose;

                    switch ((int)schedule.DayOfWeek)
                    {
                        case 1:
                            schedule.DayOfWeek = vm.Sunday ? schedule.DayOfWeek : 0;
                            break;
                        case 2:
                            schedule.DayOfWeek = vm.Monday ? schedule.DayOfWeek : 0;
                            break;
                        case 3:
                            schedule.DayOfWeek = vm.Tuesday ? schedule.DayOfWeek : 0;
                            break;
                        case 4:
                            schedule.DayOfWeek = vm.Wednesday ? schedule.DayOfWeek : 0;
                            break;
                        case 5:
                            schedule.DayOfWeek = vm.Thursday ? schedule.DayOfWeek : 0;
                            break;
                        case 6:
                            schedule.DayOfWeek = vm.Friday ? schedule.DayOfWeek : 0;
                            break;
                        case 7:
                            schedule.DayOfWeek = vm.Saturday ? schedule.DayOfWeek : 0;
                            break;
                    }
                }

                if (vm.Sunday && !schedules.Any(x => x.DayOfWeek == 1))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedication.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 1,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Monday && !schedules.Any(x => x.DayOfWeek == 2))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedication.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 2,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Tuesday && !schedules.Any(x => x.DayOfWeek == 3))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedication.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 3,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Wednesday && !schedules.Any(x => x.DayOfWeek == 4))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedication.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 4,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Thursday && !schedules.Any(x => x.DayOfWeek == 5))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedication.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 5,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Friday && !schedules.Any(x => x.DayOfWeek == 6))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedication.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 6,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Saturday && !schedules.Any(x => x.DayOfWeek == 7))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedication.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 7,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                var sp = new AddUpdateMedicationSchedules() { Schedules = schedules };
                db.Database.ExecuteStoredProcedure(sp);

                vm.GroupId = sp.GroupId;
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UserSchedules_Update([DataSourceRequest]DataSourceRequest request, ScheduleGroupViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var schedules = db.GetMedicationSchedules(vm.UserId).Where(x => x.GroupId == vm.GroupId).Select(x => new MedicationScheduleType()
                {
                    ScheduleId = x.ScheduleId,
                    UserId = x.UserId,
                    UserMedicationId = x.UserMedicationId.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek,
                    ScheduleTime = x.ScheduleTime,
                    GroupId = x.GroupId,
                    Dose = x.Dose
                }).ToList();

                foreach (var schedule in schedules)
                {
                    schedule.StartDate = vm.StartDate;
                    schedule.EndDate = vm.EndDate;
                    schedule.ScheduleTime = vm.ScheduleTime.Value.TimeOfDay;
                    schedule.Dose = vm.Dose;

                    switch ((int)schedule.DayOfWeek)
                    {
                        case 1:
                            schedule.DayOfWeek = vm.Sunday ? schedule.DayOfWeek : 0;
                            break;
                        case 2:
                            schedule.DayOfWeek = vm.Monday ? schedule.DayOfWeek : 0;
                            break;
                        case 3:
                            schedule.DayOfWeek = vm.Tuesday ? schedule.DayOfWeek : 0;
                            break;
                        case 4:
                            schedule.DayOfWeek = vm.Wednesday ? schedule.DayOfWeek : 0;
                            break;
                        case 5:
                            schedule.DayOfWeek = vm.Thursday ? schedule.DayOfWeek : 0;
                            break;
                        case 6:
                            schedule.DayOfWeek = vm.Friday ? schedule.DayOfWeek : 0;
                            break;
                        case 7:
                            schedule.DayOfWeek = vm.Saturday ? schedule.DayOfWeek : 0;
                            break;
                    }
                }

                if (vm.Sunday && !schedules.Any(x => x.DayOfWeek == 1))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 1,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Monday && !schedules.Any(x => x.DayOfWeek == 2))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 2,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Tuesday && !schedules.Any(x => x.DayOfWeek == 3))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 3,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Wednesday && !schedules.Any(x => x.DayOfWeek == 4))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 4,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Thursday && !schedules.Any(x => x.DayOfWeek == 5))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 5,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Friday && !schedules.Any(x => x.DayOfWeek == 6))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 6,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                if (vm.Saturday && !schedules.Any(x => x.DayOfWeek == 7))
                {
                    schedules.Add(new MedicationScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 7,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        Dose = vm.Dose
                    });
                }

                var sp = new AddUpdateMedicationSchedules() { Schedules = schedules };
                db.Database.ExecuteStoredProcedure(sp);

                var sp2 = new SetMedicationSchedulesInactive() { Schedules = schedules, Inactive = !vm.Active };
                db.Database.ExecuteStoredProcedure(sp2);

                vm.GroupId = sp.GroupId;
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult Medications()
        {
            var medications = db.GetMedications().Select(x => new MedicationViewModel()
            {
                MedicationId = x.MedicationId,
                BrandName = x.BrandName,
                GenericName = x.GenericName,
                Strength = x.Strength,
                Unit = x.Unit,
                DosageForm = x.DosageForm,
                Route = x.Route
            });

            return Json(medications, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserMedications(string userId)
        {
            var userMedications = db.GetUserMedications(userId).Where(x => x.MedicationScheduleTypeId == 1).Select(x => new UserMedicationViewModel()
            {
                UserMedicationId = x.UserMedicationId,
                UserId = x.UserId,
                MedicationId = x.MedicationId,
                Name = x.BrandName,
                Quantity = x.Quantity,
                Strength = x.Strength,
                Unit = x.Unit,
                DosageForm = x.DosageForm,
                Route = x.Route,
                Frequency = x.Frequency,
                Indication = x.Indication,
                Instructions = x.Instructions
            });

            return Json(userMedications, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ScheduleSummaryGrid(string userId)
        {
            var summaryTable = new DataTable();
            var scheduleTimes = db.GetMedicationScheduleSummary(userId).ToList().GroupBy(x => x.ScheduleTime);

            summaryTable.Columns.Add("Medication", typeof(string)).Caption = "Medication";

            foreach (var group in scheduleTimes)
            {
                summaryTable.Columns.Add("Schedule_" + group.Key.ToString("hhmmss"), typeof(string)).Caption = (DateTime.Today.Date + group.Key).ToString("h:mm tt");
            }

            summaryTable.Columns.Add("Indication", typeof(string)).Caption = "Indication";
            summaryTable.Columns.Add("Instructions", typeof(string)).Caption = "Instructions";

            return PartialView(summaryTable);
        }

        public ActionResult ScheduleSummary_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var summaryTable = new DataTable();
            var summary = db.GetMedicationScheduleSummary(userId).ToList();
            var userMedications = db.GetUserMedications(userId).Where(x => x.MedicationScheduleTypeId == 2).ToList();
            var scheduleTimes = summary.GroupBy(x => x.ScheduleTime);

            summaryTable.Columns.Add("ID", typeof(int));
            summaryTable.Columns.Add("Medication", typeof(string)).Caption = "Medication";

            foreach (var group in scheduleTimes)
            {
                summaryTable.Columns.Add("Schedule_" + group.Key.ToString("hhmmss"), typeof(string)).Caption = (DateTime.Today.Date + group.Key).ToString("h:mm tt");
            }

            summaryTable.Columns.Add("Indication", typeof(string)).Caption = "Indication";
            summaryTable.Columns.Add("Instructions", typeof(string)).Caption = "Instructions";

            foreach (var schedule in summary)
            {
                var existingRow = summaryTable.Select("ID = " + schedule.UserMedicationId).SingleOrDefault();

                if (existingRow != null)
                {
                    existingRow["Schedule_" + schedule.ScheduleTime.ToString("hhmmss")] = schedule.Dose;
                }
                else
                {
                    var row = summaryTable.NewRow();

                    row["ID"] = schedule.UserMedicationId;
                    row["Medication"] = string.Format("{0}<br/><i>{1}</i><br/>{2} {3} {4}", schedule.GenericName, schedule.BrandName, schedule.Strength, schedule.Unit, schedule.DosageForm);
                    row["Indication"] = schedule.Indication;
                    row["Instructions"] = schedule.Instructions;
                    row["Schedule_" + schedule.ScheduleTime.ToString("hhmmss")] = schedule.Dose;
                    summaryTable.Rows.Add(row);
                }
            }

            foreach (var medication in userMedications)
            {
                var row = summaryTable.NewRow();

                row["ID"] = medication.UserMedicationId;
                row["Medication"] = string.Format("{0}<br/><i>{1}</i><br/>{2} {3} {4}", medication.GenericName, medication.BrandName, medication.Strength, medication.Unit, medication.DosageForm);
                row["Indication"] = medication.Indication;
                row["Instructions"] = medication.Instructions;
                summaryTable.Rows.Add(row);
            }

            summaryTable.Columns.RemoveAt(0);

            return Json(summaryTable.ToDataSourceResult(request));
        }

        public ActionResult GetBpCuffSize([DataSourceRequest]DataSourceRequest request)
        {
            var sizes = db.GetBPMCuffSizeLU().Select(x => new BPCuffSize()
            {
                Size = x.Size,
                BPCuffSizeId = x.BpCuffSizeId
            }).ToList();

            return Json(sizes, JsonRequestBehavior.AllowGet);
        }
    }
}