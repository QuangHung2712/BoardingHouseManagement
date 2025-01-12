using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetSaveRoomByCustomerResModel
    {
        public long RoomId { get; set; }
        public string TowerName { get; set; }
        public string TowerAddress { get; set; }
        public decimal Price { get; set; }
        public string Device { get; set; }
    }
}
