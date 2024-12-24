using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class SharedResidents : BaseMasterData
    {
        public string Address { get; set; }
        public int Gender { get; set; }
        public decimal PriceRoom { get; set; }
        public string Describe { get; set; }
        public long CustomerId { get; set; }
        public Customers Customer { get; set; }
    }
}
