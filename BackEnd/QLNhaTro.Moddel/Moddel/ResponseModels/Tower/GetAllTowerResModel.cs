using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllTowerResModel : BaseEntity
    {
        public string TowerName { get; set; }
        public string Address { get; set; }
        public int SumRoom { get; set; }
        public int RoomRented { get; set; }
        public int RoomStillEmpty { get; set; }
    }
}
