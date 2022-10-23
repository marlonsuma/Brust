using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NiQ_Donor_Tracking_System
{
    public static class EMailHelper
    {
        public static string SendEmail(string ToMail,string Body,string Subject)
        {
            string statusmsg = string.Empty;
            try
            {
                var host = System.Configuration.ConfigurationManager.AppSettings["SMTP_Host"];
                var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTP_Port"]);
                var fromAddress = System.Configuration.ConfigurationManager.AppSettings["SMTP_FromAddress"];
                var fromName = System.Configuration.ConfigurationManager.AppSettings["SMTP_FromName"];
                var userName = System.Configuration.ConfigurationManager.AppSettings["SMTP_UserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["SMTP_Password"];
                var enableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SMTP_EnableSsl"]);
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient(host, port);
                message.From = new MailAddress(fromAddress, fromName);
                message.To.Add(new MailAddress(ToMail));
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = Body;
                smtp.Port = port;
                smtp.Host = host; //for gmail host  
                smtp.EnableSsl = enableSsl;
                //smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(userName, password);
               // smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                statusmsg = "Delivered";
            }
            catch (Exception ex)
            {
                statusmsg = "Delivery Failed - " + ex.Message;
            }
            return statusmsg;
        }
    }
}