﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CreateEditIncurReqModel : BaseEntity
    {
        public long TowerId { get; set; }
        public long RoomId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
