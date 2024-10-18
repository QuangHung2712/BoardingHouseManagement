using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace QLNhaTro.Service.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly AppDbContext _Context;
        private readonly AppSettings _EmailSettings;
        public EmailService( AppDbContext context, IOptions<AppSettings> options)
        {
            _Context = context;
            _EmailSettings = options.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_EmailSettings.EmailSetting.SenderName,_EmailSettings.EmailSetting.UserName));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") {Text = content };
            SmtpClient client = new SmtpClient();
            try
            {
                client.Connect(_EmailSettings.EmailSetting.SmtpServer, _EmailSettings.EmailSetting.SmtpPort, _EmailSettings.EmailSetting.EnableSsl);
                client.Authenticate(_EmailSettings.EmailSetting.UserName, _EmailSettings.EmailSetting.Password);
                await client.SendAsync(message);
                return;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("ERROR: " + ex);
                throw;
            }
        }
        public void ScheduleEmail()
        {
            
        }
    }
}
