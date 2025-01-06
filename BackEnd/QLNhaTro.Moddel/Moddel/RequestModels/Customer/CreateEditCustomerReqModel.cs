using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CreateEditCustomerReqModel : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string CCCD { get; set; }
        public string Address { get; set; }
        public bool IsRepresentative { get; set; }//Có phải là người đại diện không
    }
}
