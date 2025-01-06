using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class AriseBillResModel : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
