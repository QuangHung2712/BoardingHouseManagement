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
        public async Task<List<CustomerResModel>> GetCustomerByContract(long contractId)
        {
            var customerData = _Context.Customer.Where(c=>c.ContractId == contractId && !c.IsDeleted).Select(c=> CustomerResModel.Mapping(c)).ToList();
            if (customerData.Count == 0) throw new NotFoundException(nameof(contractId));
            return customerData;
        }
        public async Task CreateEditCustomer(CreateEditCustomerReqModel input,long contractId)
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
                        ContractId = contractId,

                    };
                    _Context.Customer.Add(result);
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
                var customer = _Context.Customer.Where(c=>c.Id == input.Id && !c.IsDeleted).FirstOrDefault();
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
                    ContractId = contractId,
                };
                _Context.Customer.Update(customer);
                await _Context.SaveChangesAsync();
            }
        }
        public async Task DeteleCustomer(long iD)
        {
            var customerData = _Context.Customer.Where(c=>c.Id == iD && !c.IsDeleted).FirstOrDefault();
            if (customerData == null) throw new NotFoundException(nameof(iD));
            customerData.IsDeleted = true;
            _Context.Customer.Update(customerData);
            await _Context.SaveChangesAsync();
        }
    }
}
