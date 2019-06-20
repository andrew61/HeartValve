using System.Configuration;
using System.Net.Mail;

namespace HeartValve.Mailer
{
    public static class MailerClient
    {
        private static string smtp = ConfigurationManager.AppSettings["SMTP"];
        private static string address = "tachl_support@musc.edu";
        public static void SendMail(string mailTo, string subject, string body)
        {
            SmtpClient client = new SmtpClient(smtp);
            MailAddress from = new MailAddress(address);
            MailAddress to = new MailAddress(mailTo);
            MailMessage message = new MailMessage(from, to);

            message.Subject = subject;
            message.Body = body;

            client.Send(message);
            message.Dispose();
        }
    }
}