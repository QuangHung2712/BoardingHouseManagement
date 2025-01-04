using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllIncurResModel : BaseEntity
    {
        public long RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public bool StatusPay { get; set; }
    }
}
