using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CreateEditContractReqModel : BaseEntity
    {
        public List<CreateEditCustomerReqModel> Customers { get; set; }
        public long RoomId { get; set; }
        public int ContractPeriod { get; set; }
        public decimal Deposit { get; set; }
        public List<ContractServiceReqModel>? Services { get; set; }
        public string? Note { get; set; }
    }
}
