using HeartValve.Shared.Data;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Linq;

namespace HeartValve.Services.NotificationService
{
    public class GCMMessages
    {
        public static void Main(string[] args)
        {
            using (var db = new HeartValveEntities())
            {
                var users = db.GetUsers().ToList();
                var medicationsNotTaken = db.GetUserMedicationsNotTaken(null).Where(x => x.ScheduleDate > DateTime.Now.AddMinutes(-15)).ToList();

                if (medicationsNotTaken.Any())
                {
                    var config = new GcmConfiguration(Shared.Properties.Settings.Default.GooglePushSenderID, Shared.Properties.Settings.Default.GooglePushAuthToken, null);
                    var gcmBroker = new GcmServiceBroker(config);

                    gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
                    {

                        aggregateEx.Handle(ex =>
                        {

                            if (ex is GcmNotificationException)
                            {
                                var notificationException = (GcmNotificationException)ex;
                                var gcmNotification = notificationException.Notification;
                                var description = notificationException.Description;

                                //Console.WriteLine($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                            }
                            else if (ex is GcmMulticastResultException)
                            {
                                var multicastException = (GcmMulticastResultException)ex;

                                foreach (var succeededNotification in multicastException.Succeeded)
                                {
                                    //Console.WriteLine($"GCM Notification Failed: ID={succeededNotification.MessageId}");
                                }

                                foreach (var failedKvp in multicastException.Failed)
                                {
                                    var n = failedKvp.Key;
                                    var e = failedKvp.Value;

                                    //Console.WriteLine($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Description}");
                                }

                            }
                            else if (ex is DeviceSubscriptionExpiredException)
                            {
                                var expiredException = (DeviceSubscriptionExpiredException)ex;

                                var oldId = expiredException.OldSubscriptionId;
                                var newId = expiredException.NewSubscriptionId;

                                //Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                                if (!string.IsNullOrWhiteSpace(newId))
                                {
                                    //Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                                }
                            }
                            else if (ex is RetryAfterException)
                            {
                                var retryException = (RetryAfterException)ex;
                                //Console.WriteLine($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                            }
                            else
                            {
                                //Console.WriteLine("GCM Notification Failed for some unknown reason");
                            }

                            return true;
                        });
                    };

                    gcmBroker.OnNotificationSucceeded += (notification) =>
                    {
                        Console.WriteLine("GCM Notification Sent!");
                    };

                    gcmBroker.Start();

                    //foreach (var medicationNotTaken in medicationsNotTaken)
                    //{
                    //    var user = users.SingleOrDefault(x => x.UserId.Equals(medicationNotTaken.UserId));

                    //    if (!string.IsNullOrEmpty(user.GCMToken))
                    //    {
                    //        gcmBroker.QueueNotification(new GcmNotification
                    //        {
                    //            RegistrationIds = new List<string> {
                    //                user.GCMToken
                    //            },
                    //            Notification = JObject.Parse("{ \"body\" : \"It's time to take your medication!\", \"title\" : \"HeartValve\" }")
                    //        });
                    //    }
                    //}

                    gcmBroker.Stop();
                }
            }
        }
    }
}