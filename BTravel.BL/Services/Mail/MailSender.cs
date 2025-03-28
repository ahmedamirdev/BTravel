using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.BL.Services.Mail
{
    public static class MailSender
    {
        private static string _fromEmail = "sportlink.egy@gmail.com";
        private static string _password = "zuwatungloiiawfs";
        private static string _host = "smtp.gmail.com";
        private static bool _enableSSl = true;
        private static bool _useDefaultCredentials = false;
        private static int _port = 587;

        public static void SendMail(string to, string subject, string body, bool isBodyHTML = true)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                MailAddress fromAddress = new MailAddress(_fromEmail, "BTravelMATE");

                message.From = fromAddress;
                var toList = to.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < toList.Length; i++)
                {
                    message.To.Add(toList[i]);
                }
                message.Subject = subject;
                message.IsBodyHtml = isBodyHTML;
                message.Body = body;

                smtpClient.Host = _host;
                smtpClient.Port = _port;
                smtpClient.EnableSsl = _enableSSl;
                smtpClient.UseDefaultCredentials = _useDefaultCredentials;
                smtpClient.Credentials = new System.Net.NetworkCredential(_fromEmail, _password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
    }
}