using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class SaveRoom : BaseEntity
    {
        public long RoomId { get; set; }
        public Room Room { get; set; }
        public long CustomerId { get; set; }
        public Customers Customer { get; set; }
    }
}
