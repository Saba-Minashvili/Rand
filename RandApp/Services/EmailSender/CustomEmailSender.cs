using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RandApp.Services.EmailSender
{
    public class CustomEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com", //or another email sender provider
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("randclothingbrand@gmail.com", "Vapa)4)5@003")
                };

                return client.SendMailAsync("randclothingbrand@gmail.com", email, subject, htmlMessage);
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
    }
}
