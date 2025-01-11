using Microsoft.AspNetCore.Http;
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
        Task CreateCustomer(CreateEditCustomerReqModel input);
        string GetCustomerNameByContract(long contractId);
        Task UpdateCustomer(CreateEditCustomerReqModel input, IFormFile avatar);
        string GetCustomerPhoneByContract(long contractId);
        void DeteleCustomer(long contractId);
        List<ViewBillByEmailResModel> ViewBillByEmail(string email);
        CustomerResModel GetByDetail(long id);
        long Login(string email, string password);
        Task ChangePassword(ChangePasswordReqModel input);
        CustomerResModel GetInfoUser(long id);
    }
}
