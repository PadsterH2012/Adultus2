using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using Adultus.Models;

namespace Adultus.Helpers
{
    public class EmailService
    {
        SmtpClient client = new SmtpClient();

        public void SetUpSMTP()
        {
            client = new SmtpClient("mail.adultus.co.uk");
            //If you need to authenticate
            client.Credentials = new NetworkCredential("noreply@bastiontech.com", "Phalanx12");
        }

        public void SendConfirmationEmail(string userId)
        {
            SetUpSMTP();
            SqlHelper.DbContext();
            Users u = SqlHelper.GetUser(userId);
            var confirmationGuid = u.Id;
            var verifyUrl = HttpContext.Current.Request.Url.GetLeftPart
                                (UriPartial.Authority) + "/Account/Confirm/" + confirmationGuid;

            MailMessage mailMessage = new MailMessage();
            MailAddress mailAddress = new MailAddress("noreply@adultus.co.uk");
            mailMessage.From = mailAddress;
            mailMessage.To.Add(u.Email);
            mailMessage.Subject = "Confirmation";
            mailMessage.Body = "<html><head><meta content=\"text/html; charset = utf - 8\" /></head><body><p>Dear " + u.UserName +
                ", </p><p>To verify your account, please click the following link:</p>"
                + "<p><a href=\"" + verifyUrl + "\" target=\"_blank\">" + verifyUrl
                + " </a></p><div>Best regards,</div><div>Adultus Team</div><p>Do not forward "
                + "this email. The verify link is private. Your random password is " + u.Password + " . Please change this once you have set up your account.</p></body></html>";

            mailMessage.IsBodyHtml = true;

            //client.EnableSsl = true;

            client.Send(mailMessage);
        }
    }
}