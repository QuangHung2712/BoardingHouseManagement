﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Landlord : BaseInfoPeoPle
    {
        public bool IsActive { get; set; }
        public string Password { get; set; }
    }
}
