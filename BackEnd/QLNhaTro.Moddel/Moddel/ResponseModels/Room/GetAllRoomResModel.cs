using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllRoomResModel : BaseEntity
    {
        public string NumberOfRoom { get; set; }
        public string? CustomerName { get; set; }
        public long floor { get; set; }
        public decimal PriceRoom { get; set; }
        //trạng thái tìm khách mới
        public bool Status { get; set; }
    }
}
