using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class SearchRoomResModel : BaseEntity
    {
        public string TowerName{ get; set; }
        public decimal Price { get; set; }
        public string TowerAddress { get; set; }
        public string Device { get; set; }
        public List<string> IMG { get; set; }
    }
}
