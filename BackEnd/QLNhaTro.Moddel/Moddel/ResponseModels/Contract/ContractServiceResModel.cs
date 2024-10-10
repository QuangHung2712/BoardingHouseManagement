using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class ContractServiceResModel 
    {
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
    }
}
