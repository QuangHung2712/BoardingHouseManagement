using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class BillPaymentRequest
    {
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string BankCode { get; set; }
    }
}
