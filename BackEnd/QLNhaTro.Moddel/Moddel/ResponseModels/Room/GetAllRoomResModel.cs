﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllRoomResModel : BaseEntity
    {
        public string Equipment { get; set; }
        //Số người ở
        public string? CustomerName { get; set; }
        public int NoPStaying { get; set; }
        public decimal PriceRoom { get; set; }
        public long? NumberElectric { get; set; }
        public long? NumberCountries { get; set; }
        public string? Note { get; set; }

        //trạng thái tìm khách mới
        public bool Status { get; set; }
    }
}
