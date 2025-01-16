using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllPostByCustomerResModel : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }
        public List<string> PathImgRoom { get; set; }
    }
}
