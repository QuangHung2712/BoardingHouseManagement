using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels.Room
{
    public class CheckOutRoomReqModel : BaseEntity
    {
        public long? NewNumberElectric { get; set; }
        public long? NewNumberCountries { get; set; }
        public decimal MoneyPunish { get; set; }
        public string? Note { get; set; }
    }
}
