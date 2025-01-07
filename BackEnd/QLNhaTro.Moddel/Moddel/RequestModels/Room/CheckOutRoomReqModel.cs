using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CheckOutRoomReqModel : BaseEntity
    {
        public List<GetInfoCheckOutRoomResModel> Service { get; set; }
        public decimal? MoneyPunish { get; set; }
        public string? Note { get; set; }
    }
}
