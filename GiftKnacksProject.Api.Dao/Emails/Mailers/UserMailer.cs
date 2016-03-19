using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RazorEngine;

namespace GiftKnacksProject.Api.Dao.Emails.Mailers
{ 
    public class UserMailer :  IUserMailer 	
	{
		public UserMailer()
		{
			
		}
		
        public Task ConfirmEmail(string email,string code)
        {
            var template = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/EmailTemplates/AfterRegisterEmail.html"), System.Text.Encoding.UTF8);
            var result=Razor.Parse(template, new {Email = email, Url = code});
            return SendEmail("noreply@knacksgifter.com", email, "KnacksGifter regstration", result, true);
        }

        public Task RecoveryPasswordEmail(string email, string code)
        {
            string template =
             @"<html>
                  <head>
                    <title>Hello @Model.Email</title>
                  </head>
                  <body>
                    Email: @Model.Url
                  </body>
                </html>";

            var result = Razor.Parse(template, new { Email = email, Url = code });
            return SendEmail("noreply@knacksgifter.com", email, "Valid Acc", result, true);
        }

        private  Task SendEmail(string from, string to, string subject, string body, bool isHtml)
        {
            SmtpClient mailClient = new SmtpClient("smtp.yandex.ru", 25);
            mailClient.Credentials = new NetworkCredential("noreply@knacksgifter.com", "rianon1990");
            mailClient.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;
        
            return mailClient.SendMailAsync(message);
        }
    }

  
}