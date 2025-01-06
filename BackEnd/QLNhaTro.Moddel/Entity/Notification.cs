using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Notification : BaseEntity
    {
        public long? LandlordId { get; set; }
        public virtual Landlord? Landlord { get; set; }
        public long? CustomerId { get; set; }
        public virtual Customers? Customer { get; set; }
        public string Content { get; set; }
        public long BillId { get; set; }
        public virtual Bill Bill { get; set; }
        public CommonEnums.NotificationType NotificationType { get; set; }
        public bool ReadStatus { get; set; }
    }
}
