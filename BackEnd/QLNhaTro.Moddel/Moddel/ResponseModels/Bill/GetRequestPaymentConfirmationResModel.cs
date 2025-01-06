using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetRequestPaymentConfirmationResModel : BaseEntity
    {
        public string NumberOfRoom { get; set; }
        public decimal SoTien { get; set; }
        public string NgayThanhToan { get; set; }
        public long BillId { get; set; }
    }
}
