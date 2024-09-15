using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class ServiceRoom: BaseEntityIsDelete
    {
        public long ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        public long ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }

    }
}
