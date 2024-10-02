using QLNhaTro.Commons;
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
        //Số người ở
        public int NoPStaying { get; set; }
        public decimal PriceRoom { get; set; }
        public long? NumberElectric { get; set; }
        public long? NumberCountries { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }
    }
}
