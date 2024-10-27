using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetAllContractByTowerId : BaseEntity
    {
        public string CustomerName { get; set; }
        public string NumberOfRoom { get; set; }
        public DateTime StartDate { get; set; }
        public string PhoneCustomer { get; set; }
        public decimal Deposit { get; set; }
    }
}
