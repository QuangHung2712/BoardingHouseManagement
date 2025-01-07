using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class CalculateRoomResModel
    {
        public long ContractId { get; set; }
        public long RoomId { get; set; }
        public string NumberOfRoom { get; set; }
        public decimal PriceRoom { get; set; }
        public List<ServiceCalculateRoomResModel> Services { get; set; }
    }
}
