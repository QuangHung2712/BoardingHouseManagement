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
        public long CustomerId { get; set; }
        public long RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Deposit { get; set; }
        public List<ContractServiceReqModel> Services { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string? Note { get; set; }
    }
}
