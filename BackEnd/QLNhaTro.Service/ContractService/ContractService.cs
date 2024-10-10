using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
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

        public ContractService(AppDbContext context)
        {
            _Context = context;
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
            foreach(var service in input.Services)
            {
                if (!_Context.Service.Any(item => item.Id == service.ServiceId && item.IsDeleted)) throw new NotFoundException(nameof(service .ServiceId));
            }
            if (!_Context.Room.Any(item => item.Id == input.RoomId)) throw new NotFoundException(nameof(input.RoomId));
            if (input.Id <= 0)
            {
                try
                {
                    var contract = new Contract
                    {
                        RoomId = input.RoomId,
                        StartDate = DateTime.Now,
                    };
                }
                catch (Exception ex)
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
