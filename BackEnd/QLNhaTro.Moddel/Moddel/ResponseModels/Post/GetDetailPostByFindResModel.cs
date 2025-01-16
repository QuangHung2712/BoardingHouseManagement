using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetDetailPostByFindResModel : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CustomerName { get; set; }
        public decimal PriceRoom { get; set; }
        public string Gender { get; set; }
        public string MoTa { get; set; }
        public string SDTCustomer { get; set; }
        public string SDTZalo { get; set; }
        public List<string> PathImgRoom { get; set; }
        public string CustomerAvata { get; set; }
        public bool IsSave { get; set; }
    }
}
