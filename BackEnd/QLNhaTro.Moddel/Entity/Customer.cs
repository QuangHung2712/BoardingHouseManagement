﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Customers : BaseInfoPeoPle
    {

        public bool IsRepresentative { get; set; }//Có phải là người đại diện không
        public string? Password{ get; set; }
    }
}
