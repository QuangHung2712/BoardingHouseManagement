using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Bill : BaseEntityIsDelete
    {
        public long CustomerId { get; set; }
        public virtual Customers Customer { get; set; }
        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
        public long? NewElectricity { get; set; }
        public long? NewWater { get; set; }
        public DateOnly CreationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Status { get; set; }
        public string? Note { get; set; }
    }
}
