using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.CustomerService;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Contract = QLNhaTro.Moddel.Entity.Contract;

namespace QLNhaTro.Service.ContractService
{
    public class ContractService : IContractService
    {
        private readonly AppDbContext _Context;
        private readonly ICustomerService _Customer;

        public ContractService(AppDbContext context, ICustomerService customer)
        {
            _Context = context;
            _Customer = customer;
        }
        public async Task<ContractResModel> GetDetail(long id)
        {
            try
            {
                var contractData = _Context.Contract
                    .Where(x => x.Id == id && !x.IsDeleted)
                    .Select(record => new ContractResModel
                    {
                        Id = record.Id,
                        RoomId = record.RoomId,
                        RoomName = record.Room.Name,
                        StartDate = record.StartDate,
                        EndDate = record.EndDate,
                        Deposit = record.Deposit,
                        TerminationDate = record.TerminationDate,
                        Note = record.Note,
                        ServiceMotels = _Context.ServiceRoom.Where(s=>s.ContractId == record.Id).Select(x => new ContractServiceResModel
                        {
                            ServiceId = x.ServiceId,
                            ServiceName = x.Service.Name,
                            Price = x.Price,
                            Number = x.Number
                        }).ToList(),

                    }).FirstOrDefault();
                if (contractData == null) throw new NotFoundException(nameof(ContractResModel.Id));
                return contractData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task CreateEditContract(CreateEditContractReqModel input)
        {
            //Kiểm tra xem dịch vụ có tồn tại không
            var invalidService = input.Services.Any(service =>
                !_Context.Service.Any(item => item.Id == service.ServiceId && !item.IsDeleted));

            if (invalidService) throw new NotFoundException(nameof(input.Services));

            //Kiểm tra phòng có tồn tại không
            if (!_Context.Room.Any(item => item.Id == input.RoomId)) throw new NotFoundException(nameof(input.RoomId));

            if (input.Id <= 0)
            {
                try
                {
                    var contract = new Contract
                    {
                        RoomId = input.RoomId,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(input.ContractPeriod),
                        Deposit = input.Deposit,
                        Note = input.Note,
                    };
                    _Context.Contract.Add(contract);
                    await _Context.SaveChangesAsync();

                    var contractService = input.Services.Select(s => new ServiceRoom
                    {
                        ContractId = contract.Id,
                        ServiceId = s.ServiceId,
                        Price = s.Price,
                        Number = s.Number,
                    }).ToList();
                    var serviceRoomTask =  _Context.ServiceRoom.AddRangeAsync(contractService);

                    var customer = input.Customer.Select(c => _Customer.CreateEditCustomer(c, contract.Id));
                    await Task.WhenAll(serviceRoomTask,Task.WhenAll(customer));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            else
            {
                try
                {

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }
        public async Task DeleteContract(long Id)
        {
            var contractData = _Context.Contract.Where(c=> c.Id == Id && !c.IsDeleted).FirstOrDefault();
            if (contractData == null) throw new NotFoundException(nameof(Id));
            contractData.IsDeleted = true;
            _Context.Contract.Update(contractData);
            await _Context.SaveChangesAsync();
        }
    }
}
