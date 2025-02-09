﻿using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Incur : BaseEntityIsDelete
    {
        public long TowerId { get; set; }
        public virtual Tower Tower { get; set; }
        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public bool StatusPay { get; set; }
        public long? BillId { get; set; }
        public virtual Bill? Bill { get; set; }
    }
}
