using HeartValve.Shared.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    public class ErrorController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(ErrorMessage errorMessage)
        {
            using (SmtpClient client = new SmtpClient("mailhost.musc.edu"))
            using (MailMessage message = new MailMessage("tachl_support@musc.edu", "tachl_support@musc.edu"))
            {
                message.Subject = errorMessage.Subject;
                message.Body = errorMessage.Body;
                message.IsBodyHtml = true;
                await client.SendMailAsync(message);
            }
            return Ok();
        }
    }
}