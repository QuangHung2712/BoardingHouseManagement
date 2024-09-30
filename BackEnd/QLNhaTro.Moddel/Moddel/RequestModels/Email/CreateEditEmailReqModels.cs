using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CreateEditEmailReqModels
    {
        public long Id { get; set; }
        public long LandlordId { get; set; }
        public string Email { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string EmailPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}
