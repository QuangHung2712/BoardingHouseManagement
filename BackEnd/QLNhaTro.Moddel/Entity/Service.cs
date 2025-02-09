﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Services : BaseMasterData
    {
        public long TowerId { get; set; }
        public virtual Tower Tower { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive { get; set; }
        public string UnitOfCalculation { get; set; } //Đơn vị tính
        public bool IsOldNewNumber { get; set; } // Có cần nhập số mới và cũ không
    }
}
