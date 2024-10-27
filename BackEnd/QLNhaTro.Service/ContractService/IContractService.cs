using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.ContractService
{
    public interface IContractService
    {
        Task<GetDetailContractResModel> GetDetail(long id);
        Task CreateEditContract(CreateEditContractReqModel input);
        Task DeleteContract(long Id);
        Task ContractExtension(ContractExtensionReqModel input);
        Task<List<GetAllContractByTowerId>> GetAllContractByTowerId(long towerId);
        string ExportWord(long contractId);
    }
}
