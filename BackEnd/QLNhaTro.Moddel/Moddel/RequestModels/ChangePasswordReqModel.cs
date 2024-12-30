using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels.Landlord
{
    public class ChangePasswordReqModel : BaseEntity
    {
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
    }
}
