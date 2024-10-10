﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.BillService
{
    public interface IBillService
    {
        string CreatePaymentUrl(decimal amount, string orderId, string orderInfo, string bankCode = "");
    }
}
