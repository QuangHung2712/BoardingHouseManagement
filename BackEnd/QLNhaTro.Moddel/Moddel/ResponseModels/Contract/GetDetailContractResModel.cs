using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetDetailContractResModel :BaseEntity
    {
        public List<CustomerResModel> Customers { get; set; }
        public long RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Deposit { get; set; }
        public List<ContractServiceResModel> Services { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string? Note { get; set; }

    }
}
