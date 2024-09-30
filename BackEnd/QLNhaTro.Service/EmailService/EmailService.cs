using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

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
        public async Task<bool> CreateEditEmail(CreateEditEmailReqModels input)
        {
            if(input.Id <= 0)
            {
                try
                {
                    var email = new EmailUser
                    {
                        Id = input.Id,
                        LandlordId = input.LandlordId,
                        SmtpServer = input.SmtpServer,
                        SmtpPort = input.SmtpPort,
                            
                    };
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                    return false;
                    throw;
                }
            }
            return true;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var smtpClient = new SmtpClient(_EmailSettings.EmailSetting.SmtpServer)
            {
                Port = _EmailSettings.EmailSetting.SmtpPort,
                Credentials = new NetworkCredential(_EmailSettings.EmailSetting.UserName, _EmailSettings.EmailSetting.Password),
                EnableSsl = _EmailSettings.EmailSetting.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_EmailSettings.EmailSetting.UserName, _EmailSettings.EmailSetting.SenderName),
                Subject = subject,
                Body = content,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
