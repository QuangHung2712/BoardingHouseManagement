﻿using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QLNhaTro.Commons;
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
using static System.Net.Mime.MediaTypeNames;
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
        public Task<List<GetAllContractByTowerId>> GetAllContractByTowerId(long towerId) 
        {
            var contract = _Context.Contracts.Where(record => record.Room.TowerId == towerId).Select(item => new GetAllContractByTowerId
            {
                Id = item.Id,
                NumberOfRoom = item.Room.Name,
                CustomerName = string.Join(",",_Customer.GetCustomerByContract(item.Id).Select(c=> c.FullName)),
                PhoneCustomer = string.Join(",", _Customer.GetCustomerByContract(item.Id).Select(c => c.PhoneNumber)),
                StartDate = item.StartDate,
                Deposit = item.Deposit,
            }).ToListAsync();
            if (contract == null) throw new NotFoundException(nameof(towerId));
            return contract;
        }
        public async Task<GetDetailContractResModel> GetDetail(long id)
        {
            try
            {
                var contractData = await _Context.Contracts
                    .Where(x => x.Id == id && !x.IsDeleted)
                    .Include(s=>s.ServiceMotels)
                    .Select(record => new GetDetailContractResModel
                    {
                        Id = record.Id,
                        RoomId = record.RoomId,
                        RoomName = record.Room.Name,
                        StartDate = record.StartDate,
                        EndDate = record.EndDate,
                        Deposit = record.Deposit,
                        TerminationDate = record.TerminationDate,
                        Note = record.Note,
                        ServiceMotels = record.ServiceMotels.Select(x => new ContractServiceResModel
                        {
                            ServiceId = x.ServiceId,
                            ServiceName = x.Service.Name,
                            Price = x.Price,
                            Number = x.Number
                        }).ToList(),
                        Customer = _Customer.GetCustomerByContract(record.Id),
                    }).FirstOrDefaultAsync();
                if (contractData == null) throw new NotFoundException(nameof(id));
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
                !_Context.Services.Any(item => item.Id == service.ServiceId && !item.IsDeleted));
            if (invalidService) throw new NotFoundException(nameof(input.Services));

            //Kiểm tra phòng có tồn tại không
            //if (!_Context.Room.Any(item => item.Id == input.RoomId)) throw new NotFoundException(nameof(input.RoomId));
            _Context.Rooms.IsGetAvailableById(input.RoomId);

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
                    _Context.Contracts.Add(contract);
                    await _Context.SaveChangesAsync();

                    var contractService = input.Services.Select(s => new ServiceRoom
                    {
                        ContractId = contract.Id,
                        ServiceId = s.ServiceId,
                        Price = s.Price,
                        Number = s.Number,
                    }).ToList();
                    var serviceRoomTask =  _Context.ServiceRooms.AddRangeAsync(contractService);

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
                    Contract contractUpdate = _Context.Contracts.GetAvailableById(input.Id);
                    contractUpdate.RoomId = input.RoomId;
                    contractUpdate.Deposit = input.Deposit;
                    contractUpdate.Note = input.Note;
                    _Context.Update(contractUpdate);

                    //Thực hiện xóa các dịch vụ của phòng đó và add lại
                    var service = _Context.ServiceRooms.Where(item=>item.ContractId == input.Id).ToList();
                    _Context.ServiceRooms.RemoveRange(service);
                    var serviceRoom = input.Services.Select(s => new ServiceRoom
                    {
                        ContractId = contractUpdate.Id,
                        ServiceId = s.ServiceId,
                        Price = s.Price,
                        Number = s.Number,
                    });
                    var serviceRoomTask = _Context.ServiceRooms.AddRangeAsync(serviceRoom);

                    var invalidCustomer = input.Customer.Any(customer =>
                        !_Context.Customers.Any(item => item.Id == customer.Id && !item.IsDeleted));
                    if (invalidCustomer) throw new NotFoundException(nameof(input.Customer));
                    var customerRemove = _Context.Customers.Where(item => input.Customer.Any(data => data.Id == item.Id)).ToList();
                    _Context.Customers.RemoveRange(customerRemove);
                    var customer = input.Customer.Select(c => _Customer.CreateEditCustomer(c, contractUpdate.Id));

                    await Task.WhenAll(serviceRoomTask, Task.WhenAll(customer));

                    await _Context.SaveChangesAsync();
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
            _Context.Contracts.Delete(Id);
            _Customer.DeteleCustomer(Id);
            await _Context.SaveChangesAsync();
        }
        public async Task ContractExtension(ContractExtensionReqModel input)
        {
            Contract contract = _Context.Contracts.GetAvailableById(input.ContractId);
            DateTime timeNew = contract.EndDate.AddMonths(input.ExtensionPeriod);
            contract.EndDate = timeNew;
            if(input.Deposit != null)
            {
                contract.Deposit = input.Deposit.Value;
            }
            _Context.Contracts.Update(contract);
            await _Context.SaveChangesAsync();
        }
        public string ExportWord(long contractId)
        {
            string SampleContract = "D:\\Word\\HopDong_Mau.docx";
            string outputPath = "D:\\Word\\output_contract.docx";

            var contractData = _Context.Contracts.GetAvailableById(contractId);

            System.IO.File.Copy(SampleContract, outputPath, true);
            using (WordprocessingDocument doc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

               /* foreach (var text in body.Descendants<Text>)
                {
                    text.Text = text.Text.Replace("{name}", contractData.name)
                                         .Replace("{position}", contractData.position)
                                         .Replace("{contractType}", contractData.contractType.ToString())
                                         .Replace("{departments}", contractData.Departments)
                                         .Replace("{StartDate}", contractData.FromDate)
                                         .Replace("{EndDate}", contractData.ToDate);
                }
                doc.MainDocumentPart.Document.Save();*/
            }
            return outputPath;
        }
    }
}
