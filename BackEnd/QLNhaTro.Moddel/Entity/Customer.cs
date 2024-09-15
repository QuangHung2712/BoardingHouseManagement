using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Customer : BaseInfoPeoPle
    {
        public long TowerId { get; set; }
        public virtual Tower  Tower{ get; set; }
    }
}
