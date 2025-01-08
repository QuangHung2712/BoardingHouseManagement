using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CreateEditTowerReqModel : BaseEntity
    {
        public string TowerName { get; set; }
        public string Address { get; set; }
        public long LandlordId { get; set; }
        public bool UserEnterInformation { get; set; }
    }
}
