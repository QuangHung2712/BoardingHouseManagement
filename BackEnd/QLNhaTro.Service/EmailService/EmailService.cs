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
using static QLNhaTro.Commons.CommonEnums;

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
        public async Task SendEmailCreate(string toEmail,string password, FeatureCode user)
        {
            string content = $"Bạn đã tạo tài khoản trên hệ thông quản lý nhà trọ thành công. \n Mật khẩu của bạn là : {password}\n";
            if (user == FeatureCode.Landlord)
            {
                content += "Bạn vui lòng truy cập vào http://localhost:8080/vue/tower để đăng nhập";
            }
            else
            {
                content += "Bạn vui lòng truy cập vào http://localhost:8080/vue để đăng nhập";
            }

            await SendEmailAsync(toEmail, "Tạo tài khoản thành công", content);
        }
        public async Task<int> SendEmailCode(string toEmail,string chucNang)
        {
            Random random = new Random();
            int code = random.Next(100000, 1000000);
            string content = $"Bạn đang thực hiện {chucNang } trên website quản lý nhà trọ.Đây là mã bảo mật của bạn Mã bảo mật: {code}\n ";
            await SendEmailAsync(toEmail, "Mã bảo mật", content);
            return code;
        }
    }
}
