using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllServiceResModel : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string UnitOfCalculation { get; set; }
        public bool IsOldNewNumber { get; set; }
    }
}
