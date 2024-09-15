using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Contract : BaseEntityIsDelete
    {
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PriceElectricity { get; set; }
        public decimal? PriceWater { get; set; }
        public decimal Deposit {  get; set; }
        public virtual ICollection<ServiceRoom> ServiceMotels { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string? Note { get; set; }
    }
}
