using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels.Room
{
    public class ChangeRoomReqModel
    {
        public long RoomIdOld { get; set; }
        public long RoomIdNew { get; set; }
        public DateTime TimesChange { get; set; }
    }
}
