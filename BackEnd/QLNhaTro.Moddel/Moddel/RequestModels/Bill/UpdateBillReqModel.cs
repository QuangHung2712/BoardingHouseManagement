using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class UpdateBillReqModel : BaseEntity
    {
        public List<ServiceUpdateBillReqModel> Service { get; set; }
        public decimal PriceRoom { get; set; }
        public decimal SumArises { get; set; }
    }
}
