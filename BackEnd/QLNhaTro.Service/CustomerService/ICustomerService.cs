using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.CustomerService
{
    public interface ICustomerService
    {
        List<CustomerResModel> GetCustomerByContract(long contractId);
        Task CreateEditCustomer(CreateEditCustomerReqModel input, long contractId);
        void DeteleCustomer(long contractId);
    }
}
