using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace QLNhaTro.Service.BillService
{
    public class BillService : IBillService
    {
        private readonly AppDbContext _Context;

        public BillService(AppDbContext context)
        {
            _Context = context;
        }
        public async Task SubmitRequesInformation(long towerId)
        {
            var contractStillValid = _Context.Contracts.Where(c=> c.TerminationDate == null).Include(r => r.Room).ToList();
            foreach (var contract in contractStillValid) 
            {
                var customer = _Context.Customers.Where(c=> c.ContractId == contract.Id && c.IsRepresentative).FirstOrDefault();
                var ServiceFees = _Context.ServiceRooms.Where(sr => sr.ContractId == contract.Id);
                if(customer==null || ServiceFees == null) throw new NotFoundException("Có hợp đồng không có khách hàng hoặc dịch vụ");
                Bill newbill = new Bill() 
                {
                    CustomerId = customer.Id,
                    RoomId = contract.RoomId,
                    CreationDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = CommonEnums.StatusBill.ChuaDienThongTin,
                    PriceRoom = contract.Room.PriceRoom,
                    TotalAmount = 0,
                };
                _Context.Bills.Add(newbill);                
            }
        }
    }
}
