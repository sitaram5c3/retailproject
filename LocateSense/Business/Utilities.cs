using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;


namespace LocateSense.Business
{
    public class Utilities
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void sendNewRegistrationEmail(string name, string email)
        {
            return;
            var emailfrom = System.Web.Configuration.WebConfigurationManager.AppSettings["email"];
            var emailHost = System.Web.Configuration.WebConfigurationManager.AppSettings["emailHost"];
            var emailPort = System.Web.Configuration.WebConfigurationManager.AppSettings["emailPort"];
            var emailUseDefaultCredentials = System.Web.Configuration.WebConfigurationManager.AppSettings["emailUseDefaultCredentials"];

            var password = email.Split('@')[0];
            password = password.Replace(".", "");
            password = password.Replace("-", "");
            password = password.Replace("_", "");

            MailMessage mail = new MailMessage(email, "user@hotmail.com");
            SmtpClient client = new SmtpClient();
            client.Port = int.Parse(emailPort);//25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = bool.Parse(emailUseDefaultCredentials);// false;
            client.Host = emailHost;// "smtp.google.com";
            mail.Subject = "Start Shopping Smarter Now - our new account password";
            mail.Body = "Thank you for signing up for Shopping Smart. Your Password is " + password;
            client.Send(mail);
        }
    }
}