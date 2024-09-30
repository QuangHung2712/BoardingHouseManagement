using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class EmailUser : BaseEntity
    {
        public long LandlordId { get; set; }
        public virtual Landlord Landlord { get; set; }
        public string SmtpServer { get; set; }
        public int  SmtpPort { get; set; }
        public string EmailPassword { get; set;}
        public bool EnableSsl { get; set; }
    }
}
