using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;
using HeartValve.Mailer;

namespace HeartValve
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["MailOnError"]))
            {
                string body = "";

                body = "An error has occured in the Heart Valve Portal.  Please see below for details.\r\n\r\n";
                try
                {
                    if (User != null)
                    {
                        body += "User: " + User.Identity.Name.ToString() + "\r\n";
                    }
                    if (Session != null)
                    {
                        body += "SessionID: " + Session.SessionID.ToString() + "\r\n";
                    }
                }
                catch (HttpException e2)
                {
                    //not in an http session?
                    System.Diagnostics.Debug.WriteLine(e2);


                }
                if (HttpContext.Current != null)
                {
                    body += "URL: " + HttpContext.Current.Request.Url.AbsoluteUri + "\r\n";
                    body += "Page: " + HttpContext.Current.Request.Url.AbsolutePath + "\r\n";
                }
                body += "\r\n";

                Exception exc = Server.GetLastError();

                while (exc != null)
                {
                    body += "[" + exc.GetType().Name + "]\r\n";
                    body += "Message: " + exc.Message + "\r\n";
                    body += "Source: " + exc.Source + "\r\n";
                    body += "Stack Trace: " + exc.StackTrace + "\r\n\r\n";

                    exc = exc.InnerException;
                }
                var serverError = Server.GetLastError() as HttpException;

                if (null != serverError)
                {
                    int errorCode = serverError.GetHttpCode();

                    if (404 != errorCode)
                    {
                        MailerClient.SendMail(ConfigurationManager.AppSettings["ErrorMailTo"], "Heart Valve Error Report", body);

                    }
                }
            }
        }
    }
}