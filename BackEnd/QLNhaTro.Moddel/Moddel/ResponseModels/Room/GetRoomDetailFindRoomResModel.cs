using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetRoomDetailFindRoomResModel
    {
        public string TowerName { get; set; }
        public string TowerAddress { get; set; }
        public string LandlordName { get; set; }
        public decimal PriceRoom { get; set; }
        public string Device { get; set; }
        public string MoTa { get; set; }
        public string SDTLandlord { get; set; }
        public string SDTZalo { get; set; }
        public List<string> PathImgRoom { get; set; }
        public List<GetAllServiceResModel> Service { get; set; }
        public string LandlordAvata { get; set; }
    }
}
