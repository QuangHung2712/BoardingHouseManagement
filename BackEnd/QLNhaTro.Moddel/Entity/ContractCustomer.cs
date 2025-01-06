using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class ContractCustomer : BaseEntity
    {
        public long ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        public long CustomerId { get; set; }
        public virtual Customers Customer { get; set; }
    }
}
