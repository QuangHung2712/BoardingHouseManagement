﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class RefusePayReqModel : BaseEntity
    {
        public long BillId { get; set; }
        public string NumberOfRoom { get; set; }

    }
}
