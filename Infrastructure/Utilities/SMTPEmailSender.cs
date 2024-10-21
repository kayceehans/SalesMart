using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SalesMart.Domain.DataTransferObject;
using Microsoft.Extensions.Options;

namespace SalesMart.Infrastructure.Utilities
{
    public class SMTPEmailSender: ISMTPEmailSender
    {
        private readonly SMTPSettingsDto _emailSettings;
        public SMTPEmailSender(IOptions<SMTPSettingsDto> emailSettings)
        {

            _emailSettings = emailSettings.Value;

        }
        public async Task<Tuple<string, bool>> Email(EmailSenderDto request)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_emailSettings.FromMailAddress);
                message.To.Add(new MailAddress(request.EmailTo));
                message.Subject = request.Subject;
                message.IsBodyHtml = true; //to make message body as html
                message.Body = request.Body;
                smtp.Port = _emailSettings.Port;
                smtp.Host = _emailSettings.Host; // "smtp.gmail.com"; //for gmail host
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailSettings.FromMailAddress, _emailSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                return new Tuple<string, bool>("Welcome email sent", true);
            }
            catch (Exception ex) { return new Tuple<string, bool>($"Error: {ex.Message} sent", false); }
        }
    }
}
