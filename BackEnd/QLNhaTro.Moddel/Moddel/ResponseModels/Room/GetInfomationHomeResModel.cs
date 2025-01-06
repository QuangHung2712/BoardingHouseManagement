using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetInfomationHomeResModel
    {
        public int RoomUnpaid { get; set; }
        public string InfoRoomUnpaid { get; set; }
        public int RoomPaid { get; set; }
        public string InfoRoomPaid { get; set; }
        public int RoomNoInvoice { get; set; }
        public string InfoRoomNoInvoice { get; set; }
        public int RoomAvailable { get; set; }
        public string InfoRoomAvailable { get; set; }
        public int RoomExpireContract { get; set; }
        public string InfoRoomExpireContract { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
