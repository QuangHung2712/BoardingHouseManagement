using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _Context;

        public CustomerService(AppDbContext context)
        {
            _Context = context;
        }
        public List<CustomerResModel> GetCustomerByContract(long contractId)
        {
            var customerData = _Context.ContractCustomers.Where(c=>c.ContractId == contractId && !c.Contract.IsDeleted).Select(c=> CustomerResModel.Mapping(c.Customer)).ToList();
            return customerData;
        }
        public string GetCustomerNameByContract(long contractId)
        {
            var customerData = _Context.ContractCustomers.Where(c => c.ContractId == contractId && !c.Contract.IsDeleted).Select(c => c.Customer.FullName).ToList();
            return string.Join(", ", customerData);
        }
        public string GetCustomerPhoneByContract(long contractId)
        {
            var customerData = _Context.ContractCustomers.Where(c => c.ContractId == contractId && !c.Contract.IsDeleted).Select(c => c.Customer.PhoneNumber).ToList();
            return string.Join(", ", customerData);
        }
        public async Task CreateEditCustomer(CreateEditCustomerReqModel input)
        {
            if (input.Id <= 0)
            {
                try
                {
                    var result = new Customers
                    {
                        Id = input.Id,
                        FullName = input.FullName,
                        DoB = input.DoB,
                        PhoneNumber = input.PhoneNumber,
                        Email = input.Email,
                        Address = input.Address,
                        CCCD = input.CCCD,

                    };
                    _Context.Customers.Add(result);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            else 
            {
                var customer = _Context.Customers.Where(c=>c.Id == input.Id && !c.IsDeleted).FirstOrDefault();
                if (customer == null) 
                {
                    throw new NotFoundException(nameof(input.FullName));
                }
                customer = new Customers
                {
                    FullName = input.FullName,
                    DoB = input.DoB,
                    PhoneNumber = input.PhoneNumber,
                    Email = input.Email,
                    Address = input.Address,
                    CCCD = input.CCCD,
                };
                _Context.Customers.Update(customer);
                await _Context.SaveChangesAsync();
            }
        }
        public void DeteleCustomer(long contractId)
        {
            
        }
    }
}
