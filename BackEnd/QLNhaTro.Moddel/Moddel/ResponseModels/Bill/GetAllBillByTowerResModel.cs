using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllBillByTowerResModel : BaseEntity
    {
        public string CustomerName { get; set; }
        public string NumberOfRoom { get; set; }
        public decimal TotalAmount { get; set; }
        public string Time { get; set; }
        public string PaymentDate { get; set; }
        public string Status { get; set; }
    }
}
