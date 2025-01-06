using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class NameServiceBillResModel : BaseEntity
    {
        public string Name { get; set; }
        public long? OldNumber { get; set; } // Số cũ
        public long? NewNumber { get; set; } //Số mới
        public long UsageNumber { get; set; } //Số sử dụng
        public decimal UnitPrice { get; set; } //Đơn giá
        public bool IsOldNewNumber { get; set; }
    }
}
