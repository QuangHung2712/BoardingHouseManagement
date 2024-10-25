using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class ContractExtensionReqModel 
    {
        public long ContractId { get; set; }
        public int ExtensionPeriod{ get; set; }
        public decimal? Deposit { get; set; }
    }
}
