using HeartValve.Shared.Data;
using PushSharp.Apple;
using System;
using System.Linq;

namespace HeartValve.Services.NotificationService
{
    public class APNSMessages
    {
        public static void Main(string[] args)
        {
            using (var db = new HeartValveEntities())
            {
                var users = db.GetUsers().ToList();
                var medicationsNotTaken = db.GetUserMedicationsNotTaken(null).Where(x => x.ScheduleDate > DateTime.Now.AddMinutes(-15)).ToList();

                if (medicationsNotTaken.Any())
                {
                    var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                        Shared.Properties.Resources.ApplePushCert, Shared.Properties.Settings.Default.ApplePushPW);
                    var apnsBroker = new ApnsServiceBroker(config);

                    apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                    {
                        aggregateEx.Handle(ex =>
                        {
                            if (ex is ApnsNotificationException)
                            {
                                var notificationException = (ApnsNotificationException)ex;
                                var apnsNotification = notificationException.Notification;
                                var statusCode = notificationException.ErrorStatusCode;

                                //Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                            }
                            else
                            {
                                //Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                            }

                            return true;
                        });
                    };

                    apnsBroker.OnNotificationSucceeded += (notification) =>
                    {
                        Console.WriteLine("Apple Notification Sent!");
                    };

                    apnsBroker.Start();

                    //foreach (var medicationNotTaken in medicationsNotTaken)
                    //{
                    //    var user = users.SingleOrDefault(x => x.UserId.Equals(medicationNotTaken.UserId));

                    //    if (!string.IsNullOrEmpty(user.APNSToken))
                    //    {
                    //        apnsBroker.QueueNotification(new ApnsNotification
                    //        {
                    //            DeviceToken = user.APNSToken,
                    //            Payload = JObject.Parse("{\"aps\":{\"alert\":{\"title\":\"Medication Reminder\",\"body\":\"It's time to take your medication!\"}},\"userMedicationId\":\"" + medicationNotTaken.UserMedicationId + "\",\"dose\":\"" + medicationNotTaken.Dose + "\",\"scheduleId\":\"" + medicationNotTaken.ScheduleId + "\",\"scheduleDate\":\"" + medicationNotTaken.ScheduleDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\"}")
                    //        });
                    //    }
                    //}

                    apnsBroker.Stop();
                }
            }
        }
    }
}