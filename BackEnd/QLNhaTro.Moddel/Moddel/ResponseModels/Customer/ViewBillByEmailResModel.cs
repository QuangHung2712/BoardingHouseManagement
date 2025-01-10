using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class ViewBillByEmailResModel
    {
        public string AddressTower { get; set; }
        public string RoomName { get; set; }
        public string BillID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentDate { get; set; }
        public string Time { get; set; }
    }
}
