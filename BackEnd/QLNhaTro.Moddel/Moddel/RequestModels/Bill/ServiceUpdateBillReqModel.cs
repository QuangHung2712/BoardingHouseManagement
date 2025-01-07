using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class ServiceUpdateBillReqModel : BaseEntity
    {
        public long? OldNumber { get; set; }
        public long? NewNumber { get; set; }
        public long UsageNumber { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
