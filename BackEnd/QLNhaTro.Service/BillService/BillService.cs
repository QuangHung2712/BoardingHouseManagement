using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Service.EmailService;
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
        private readonly IEmailService _Email;

        public BillService(AppDbContext context, IEmailService email)
        {
            _Context = context;
            _Email = email;
        }
        public async Task SubmitRequesInformation()
        {
            var contractStillValid = _Context.Contracts.Where(c=> c.TerminationDate == null && c.UserEnterInformation).Include(r => r.Room)
                .Select(c => new
                {
                    Id = c.Id,
                    RoomId = c.RoomId,
                    PriceRoom = c.Room.PriceRoom
                }).ToList();
            foreach (var contract in contractStillValid) 
            {
                var customer = _Context.Customers.Where(c=> c.ContractId == contract.Id && c.IsRepresentative).FirstOrDefault();
                var ServiceFees = _Context.ServiceRooms.Where(sr => sr.ContractId == contract.Id).ToList();
                if(customer==null || ServiceFees == null) throw new NotFoundException("Có hợp đồng không có khách hàng hoặc dịch vụ");
                Bill newbill = new Bill() 
                {
                    CustomerId = customer.Id,
                    RoomId = contract.RoomId,
                    CreationDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = CommonEnums.StatusBill.ChuaDienThongTin,
                    PriceRoom = contract.PriceRoom,
                    TotalAmount = 0,
                };
                _Context.Bills.Add(newbill);
                await _Context.SaveChangesAsync();
                #region Gửi yêu cầu nhập thông tin đến khách thuê
                string link = $"http://localhost:8080/vue/enterbill/{CommonFunctions.Decrypt(newbill.Id.ToString())}";
                string EmailContent = $"Xin chào {customer.FullName}. Đã bắt đầu một tháng mới bạn vui lòng vào link này để nhập các thông tin cần thiết để tạo hóa đơn \nĐây là link: {link}";
                await _Email.SendEmailAsync(customer.Email, "Nhập hóa đơn tháng mới", EmailContent);
                #endregion
            };  
        }
    }
}
