using HeartValve.Shared.Data;
using System;
using System.Linq;
using System.Net.Mail;
using System.Collections.Generic;
using System.IO;
using SelectPdf;


namespace HeartValve.Services.NotificationService
{
    public class DailyAssessmentReport
    {

        public class TableItems {
            public string tableRow { get; set;}
            public int asnweredCount { get; set; }
            public int errorCount { get; set; }
            public int unAsnwerCount { get; set; }
            public string userName { get; set; }

        }

        public static void Main(string[] args)
        {
            using (var db = new HeartValveEntities())
            {
                String currentDate = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy")+" 12:30 PM - " + DateTime.Now.ToString("MM/dd/yyyy") + " 12:30 PM" ;
                //var report = db.GetDailyAssessmentReport(DateTime.Today.AddDays(-3), null).ToList();
                //var assessmentReport = db.GetDailyAssessmentReport(DateTime.Today.AddDays(-3), null).ToList();
                //var assessmentReportGroups = assessmentReport.GroupBy(x => x.UserId);

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

                    //var msg = new MailMessage("developer@spurstech.com", "burrouja@musc.edu,ricsu@musc.edu,quinnjay@musc.edu,patelsk@musc.edu");
                    var msg = new MailMessage("developer@spurstech.com", "burroughsjd@gmail.com");
                    //MailAddress addressBCC = new MailAddress("burroughsjd@gmail.com");
                    //msg.Bcc.Add(addressBCC);
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
                        msg.Body += "<th style='border: solid 1px #000; background: #eee; width: 200px;'>"+question.QuestionText+"</th>";

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
                                if (j+1 < dataSet.Count) {
                                    if (sQuestion.QuestionId == dataSet[j+1].QuestionId) {
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
                        else
                        {

                            tr.tableRow += "<td style='border: solid 1px #000;'> Incomplete Survey Session</td>";
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

                    foreach(TableItems item in tableRowList.OrderByDescending(t => t.errorCount)) {

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

                    String fileName = "Daily Assessment Report.pdf";
                    HtmlToPdf converter = new HtmlToPdf();
                    PdfDocument doc = converter.ConvertHtmlString("<html><body>" + msg.Body + "</body></html>");
                    doc.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));


                    msg.Attachments.Add(new Attachment(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)));
                    System.Diagnostics.Debug.WriteLine("The current file path " + AppDomain.CurrentDomain.BaseDirectory);

                    client.Send(msg);
                    //if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                    //{
                    //    System.Diagnostics.Debug.WriteLine("The File Exists");

                    //    File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                    //}


                }
            }
            
    }

    } //DailyAssessmentReport


}