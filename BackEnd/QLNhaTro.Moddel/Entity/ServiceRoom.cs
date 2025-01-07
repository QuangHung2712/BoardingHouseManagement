using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class ServiceRoom: BaseEntity
    {
        public long ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        public long ServiceId { get; set; }
        public virtual Services Service { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
        public int CurrentNumber { get; set; } // Số ban đầu
        public bool IsOldNewNumber { get; set; }
    }
}
