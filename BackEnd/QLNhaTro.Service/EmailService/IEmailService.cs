using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.Service.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
        Task SendEmailCreate(string toEmail, string password, FeatureCode user);
        Task<int> SendEmailCode(string toEmail,string chucNang);
    }
}
