using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetInfoBillResModel : BaseEntity
    {
        public List<NameServiceBillResModel> Service { get; set; }
        public List<AriseBillResModel> Arises { get; set; }
        public string NumberOfRoom { get; set; }
        public decimal PriceRoom { get; set; }
        public string Date { get; set; }
        public string AddressTower { get; set; }
        public string? STK { get; set; }
        public string? PathImgQRPay { get; set; }
        public decimal? Amount { get; set; }
        public StatusBill Status { get; set; }
    }
}
