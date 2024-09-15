using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    //Toà nhà
    public class Tower : BaseMasterData
    {
        public string Address { get; set; }
        public long LandlordId { get; set; }
        public virtual Landlord Landlord { get; set; }
    }
}
