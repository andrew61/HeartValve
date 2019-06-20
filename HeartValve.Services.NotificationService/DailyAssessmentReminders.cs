using HeartValve.Shared.Data;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Linq;


namespace HeartValve.Services.NotificationService
{
    public class DailyAssessmentReminders
    {
        public static void Main(string[] args)
        {
            using (var db = new HeartValveEntities())
            {
                var assessments = db.GetDailyAssessmentsNotTaken().Where(x => x.TokenTypeId == 1 ).ToList();

                if (assessments.Any())
                {
                    var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
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

                                Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}" +"Device Token:  "+ apnsNotification.DeviceToken );

                                
                                    System.Diagnostics.Debug.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}" + "Device Token: " + apnsNotification.DeviceToken);
                            }
                            else
                            {
                                Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                                System.Diagnostics.Debug.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                            }

                            return true;
                        });
                    };

                    apnsBroker.OnNotificationSucceeded += (notification) =>
                    {
                        Console.WriteLine("Apple Notification Sent!");
                    };

                    apnsBroker.Start();

                    foreach (var assessment in assessments)
                    {
                        apnsBroker.QueueNotification(new ApnsNotification
                        {
                            DeviceToken = assessment.Token,
                            Payload = JObject.Parse("{\"aps\":{\"alert\":{\"title\":\"Daily Assessment Reminder\",\"body\":\"It's time to take your daily assessment!  Please open the Heart Valve app to begin.\"},\"sound\":\"dailyreminder.m4a\"}}")
                        });
                    }

                    apnsBroker.Stop();
                }
            }
        }
    }
}