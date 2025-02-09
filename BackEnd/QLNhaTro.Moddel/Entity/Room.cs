﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Room : BaseMasterData
    {
        public long TowerId { get; set; }
        public virtual Tower Tower { get; set; }
        public string Equipment { get; set; }
        public int NoPStaying { get; set; }//Số người ở
        public decimal PriceRoom { get; set; }
        public long? NumberElectric { get; set; }
        public long? NumberCountries { get; set; }
        public string? Note { get; set; }
        public bool StatusNewCustomer { get; set; } //trạng thái tìm khách mới
    }
}
