using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class SaveNews : BaseEntity
    {
        public long NewId { get; set; }
        public SharedResidents New { get; set; }
        public long CustomerId { get; set; }
        public Customers Customer { get; set; }
    }
}
