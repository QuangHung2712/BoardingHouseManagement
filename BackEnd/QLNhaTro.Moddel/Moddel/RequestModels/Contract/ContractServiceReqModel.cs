﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class ContractServiceReqModel
    {
        public long ServiceId { get; set; }
        public decimal Price { get; set; }
        public int? Number { get; set; }
        public bool IsOldNewNumber { get; set; }
        public int? CurrentNumber { get; set; }// Số ban đầu
    }
}
