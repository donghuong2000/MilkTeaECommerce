using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Utility
{
     public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;
        public EmailSender(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value; //Get fromt Json
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailOptions.DisplayName, _emailOptions.Mail));
                message.To.Add(new MailboxAddress(email));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = htmlMessage };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port);
                    await client.AuthenticateAsync(_emailOptions.Mail, _emailOptions.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(e.Message);
            }
        }
        
    }
}
