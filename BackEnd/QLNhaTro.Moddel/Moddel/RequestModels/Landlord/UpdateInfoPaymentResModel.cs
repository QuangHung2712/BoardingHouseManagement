﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class UpdateInfoPaymentReqModel : BaseEntity
    {
        public string bank { get; set; }
        public string STK { get; set; }
    }
}
