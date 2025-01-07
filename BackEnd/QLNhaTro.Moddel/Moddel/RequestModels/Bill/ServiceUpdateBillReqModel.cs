using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class ServiceUpdateBillReqModel
    {
        public long ServiceId { get; set; }
        public long? OldNumber { get; set; }
        public long? NewNumber { get; set; }
        public long UsageNumber { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
