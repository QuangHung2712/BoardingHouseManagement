using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Landlord : BaseInfoPeoPle
    {
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public string? STK { get; set; } //Số tài khoản
        public string? SampleContractLink { get; set; }//Đường dẫn đến hợp đồng mẫu
        public string? PaymentQRImageLink { get; set; }//Đường đãn đến QR thanh toán
    }
}
