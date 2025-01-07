using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetInfoPaymentResModel
    {
        public string Bank { get; set; }
        public string STK { get; set; }
        public string PaymentQRImageLink { get; set; }
    }
}
