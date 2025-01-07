using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class ServiceInvoiceDetails : BaseEntity
    {
        public long ServiceId { get; set; }
        public Services Service { get; set; }
        public long BillId { get; set; }
        public Bill Bills { get; set; }
        public long? OldNumber { get; set; } // Số cũ
        public long? NewNumber { get; set; } //Số mới
        public long UsageNumber { get; set; } //Số sử dụng
        public decimal UnitPrice { get; set; } //Đơn giá
        public string? PathImg { get; set; }
    }
}
