using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CreateEditServiceReqModel : BaseEntity
    {
        public long TowerId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string UnitOfCalculation { get; set; }
        public bool ApplyPriceServiceAllRoom { get; set; }
    }
}
