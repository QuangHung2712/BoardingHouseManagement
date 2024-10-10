using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Customers : BaseInfoPeoPle
    {
        public long ContractId { get; set; }
        public virtual Contract Contract { get; set; }
    }
}
