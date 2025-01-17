using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.ContractService;
using QLNhaTro.Service.EmailService;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static QLNhaTro.Commons.CommonEnums;

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
            var contractStillValid = _Context.Contracts.Where(c => c.TerminationDate == null && c.UserEnterInformation).Include(r => r.Room)
                .Select(c => new
                {
                    Id = c.Id,
                    RoomId = c.RoomId,
                    PriceRoom = c.Room.PriceRoom
                }).ToList();
            foreach (var contract in contractStillValid)
            {
                var customer = _Context.ContractCustomers.Where(c => c.ContractId == contract.Id && c.IsRepresentative).Select(record=>record.Customer).FirstOrDefault();
                var ServiceFees = _Context.ServiceRooms.Where(sr => sr.ContractId == contract.Id).ToList();
                if (customer == null) throw new NotFoundException("Có hợp đồng không có khách hàng");
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

                var arise = _Context.Incurs.Where(item =>(item.RoomId == newbill.RoomId && item.StatusPay == false) && !item.IsDeleted).ToList();
                foreach (var item in arise)
                {
                    item.BillId = newbill.Id;
                }
                _Context.Incurs.UpdateRange(arise);
                await _Context.SaveChangesAsync();
                #region Gửi yêu cầu nhập thông tin đến khách thuê
                string link = $"http://localhost:8080/vue/enterbill/{CommonFunctions.Encryption(newbill.Id.ToString())}";
                string EmailContent = $"Xin chào {customer.FullName}. Đã bắt đầu một tháng mới bạn vui lòng vào link này để nhập các thông tin cần thiết để tạo hóa đơn \nĐây là link: {link}";
                await _Email.SendEmailAsync(customer.Email, "Nhập hóa đơn tháng mới", EmailContent);
                #endregion
            };
        }
        public GetInfoBillResModel GetInfoBill(string Id)
        {
            var idbill = CommonFunctions.Decrypt(Id);
            Bill infoBill = _Context.Bills.GetAvailableById(long.Parse(CommonFunctions.Decrypt(Id)));
            var contractid = _Context.Contracts.Where(item=> item.RoomId == infoBill.RoomId && item.TerminationDate == null).Select(item=>item.Id).FirstOrDefault();
            GetInfoBillResModel result = new GetInfoBillResModel();
            if(infoBill.Status == StatusBill.ChuaDienThongTin)
            {
                var roomDetails = _Context.Rooms
                    .Where(r => r.Id == infoBill.RoomId)
                    .Include(r => r.Tower) // Bao gồm thông tin Tower
                    .Select(r => new
                    {
                        Room = new
                        {
                            r.Id,
                            r.Name,
                            r.PriceRoom,
                        },
                        Tower = new
                        {
                            r.Tower.Id,
                            r.Tower.Address,
                        },
                    }).FirstOrDefault();
                if (roomDetails == null) throw new NotFoundException("Phòng");
                result = new GetInfoBillResModel
                {
                    Id = infoBill.Id,
                    NumberOfRoom = roomDetails.Room.Name,
                    PriceRoom = roomDetails.Room.PriceRoom,
                    Date = infoBill.PriceRoom != 0 ? infoBill.CreationDate.AddMonths(-1).ToString("MM/yyyy") : infoBill.CreationDate.ToString("MM/yyyy"),
                    AddressTower = roomDetails.Tower.Address,
                    Status = infoBill.Status,

                };
                result.Service = _Context.ServiceRooms.Where(item => item.ContractId == contractid).Include(s => s.Service).Select(record => new NameServiceBillResModel
                {
                    Id = record.ServiceId,
                    Name = record.Service.Name,
                    UnitPrice = record.Service.UnitPrice,
                    UsageNumber = record.Number,
                    IsOldNewNumber = record.IsOldNewNumber,
                    OldNumber = 0,
                }).ToList();
            }
            else
            {
                var roomDetails = _Context.Rooms
                    .Where(r => r.Id == infoBill.RoomId)
                    .Include(r => r.Tower) // Bao gồm thông tin Tower
                    .ThenInclude(t => t.Landlord) // Bao gồm thông tin Landlord thông qua Tower
                    .Select(r => new
                    {
                        Room = new
                        {
                            r.Id,
                            r.Name,
                            r.PriceRoom,
                        },
                        Tower = new
                        {
                            r.Tower.Id,
                            r.Tower.Address,
                            r.Tower.LandlordId
                        },
                        Landlord = new
                        {
                            r.Tower.Landlord.STK,
                            r.Tower.Landlord.PaymentQRImageLink
                        }
                    }).FirstOrDefault();
                    if (roomDetails == null) throw new NotFoundException("Phòng");
                    result = new GetInfoBillResModel
                    {
                        Id = infoBill.Id,
                        NumberOfRoom = roomDetails.Room.Name,
                        PriceRoom = roomDetails.Room.PriceRoom,
                        Date = infoBill.PriceRoom != 0 ? infoBill.CreationDate.AddMonths(-1).ToString("MM/yyyy") : infoBill.CreationDate.ToString("MM/yyyy"),
                        AddressTower = roomDetails.Tower.Address,
                        STK = roomDetails.Landlord.STK,
                        PathImgQRPay = CommonFunctions.ConverPathIMG(roomDetails.Landlord.PaymentQRImageLink),
                        Status = infoBill.Status,
                        Amount = infoBill.TotalAmount,
                    };
                result.Service = _Context.ServiceInvoiceDetails.Where(item => item.BillId == infoBill.Id).Include(s => s.Service).Select(record => new NameServiceBillResModel
                {
                    Id = record.ServiceId,
                    Name = record.Service.Name,
                    UnitPrice = record.UnitPrice,
                    UsageNumber = record.UsageNumber,
                    OldNumber = record.OldNumber,
                    NewNumber = record.NewNumber,
                    IsOldNewNumber = record.Service.IsOldNewNumber,
                }).ToList();
            }
            result.Arises = _Context.Incurs.Where(item=> item.BillId == infoBill.Id && !item.IsDeleted).Select(record => new AriseBillResModel
            {
                Id = record.Id,
                Amount = record.Amount, 
                Reason = record.Reason,
            }).ToList();
            return result;
        }
        public async Task CalculateInvoice(GetInfoBillResModel input)
        {
            var bill = _Context.Bills.GetAvailableById(input.Id);
            var contractService = input.Service.Select(s => new ServiceInvoiceDetails
            {
                ServiceId = s.Id,
                BillId = input.Id,
                OldNumber = s.OldNumber,
                NewNumber = s.NewNumber,
                UnitPrice = s.UnitPrice,
                UsageNumber = s.IsOldNewNumber == false ? s.UsageNumber : s.NewNumber.Value - s.OldNumber.Value,
            }).ToList();
            await _Context.ServiceInvoiceDetails.AddRangeAsync(contractService);
            bill.Status = StatusBill.ChuaThanhToan;
            bill.CreationDate = DateOnly.FromDateTime(DateTime.Now);
            bill.TotalAmount = bill.PriceRoom + input.Arises.Sum(item => item.Amount) + contractService.Sum(item => item.UnitPrice * item.UsageNumber);
            _Context.Bills.Update(bill);
            await _Context.SaveChangesAsync();
        }
        public async Task PayBill(long id)
        {
            var infoBill = _Context.Bills.Where(item=> item.Id == id).FirstOrDefault();
            if(infoBill == null)
            {
                throw new Exception("Hoá đơn không tồn tại");
            }
            var Landlord = _Context.Rooms
                    .Where(r => r.Id == infoBill.RoomId)
                    .Include(r => r.Tower) // Bao gồm thông tin Tower
                    .ThenInclude(t => t.Landlord) // Bao gồm thông tin Landlord thông qua Tower
                    .Select(r => new
                    {
                        Room = new
                        {
                            r.Id,
                            r.Name,
                        },
                        Tower = new
                        {
                            r.Tower.Id,
                            r.Tower.LandlordId
                        },
                        Landlord = new
                        {
                            r.Tower.Landlord.Id,
                            r.Tower.Landlord.Email,
                        }
                    }).FirstOrDefault();
            if(Landlord == null)
            {
                throw new Exception("Chủ nhà không tồn tại");
            }
            //Thêm thông báo 
            string content = $"Phòng {Landlord.Room.Name} đã thanh toán hoá đơn tháng {infoBill.CreationDate.AddMonths(-1).ToString("MM/yyyy")} vào ngày {DateTime.Now} với số tiền {infoBill.TotalAmount}. Bạn thử kiểm tra tài khoản xem đã nhận được số tiền chưa. ";
            Notification notification = new Notification() 
            {
                LandlordId = Landlord.Landlord.Id,
                Content = content ,
                NotificationType = NotificationType.XacNhanHoaDon,
                BillId = infoBill.Id,
                ReadStatus = false,
            };
            _Context.Notifications.Add(notification);

            var arise = _Context.Incurs.Where(item=> item.BillId == infoBill.Id && !item.IsDeleted).ToList();
            if(arise != null)
            {
                foreach (var item in arise)
                {
                    item.StatusPay = true;
                }
                _Context.Incurs.UpdateRange(arise);
            } 
            //Cập nhận thông tin hoá đơn
            infoBill.PaymentDate = DateTime.Now;
            infoBill.Status = StatusBill.ChuaXacNhanThanhToan;
            _Context.Bills.Update(infoBill);

            await _Context.SaveChangesAsync();

            await _Email.SendEmailAsync(Landlord.Landlord.Email, $"Xác nhận thanh toán hoá của phòng {Landlord.Room.Name}", content + "Bạn vui lòng vào hệ thống để xác nhận thanh toán.");
        }
        public List<GetRequestPaymentConfirmationResModel> GetRequestPayment(long landlordId)
        {
            var result = _Context.Notifications.Include(n => n.Bill).Where(item=> item.Bill.Room.TowerId == landlordId && !item.ReadStatus).Select(record => new GetRequestPaymentConfirmationResModel
            {
                Id = record.Id,
                BillId= record.BillId,
                NgayThanhToan = record.Bill.PaymentDate.ToString("HH:mm - dd/MM/yyyy"),
                NumberOfRoom = record.Bill.Room.Name,
                SoTien = record.Bill.TotalAmount,
            }).ToList();
            return result;
        }
        public async Task RefusePay(RefusePayReqModel input)
        {
            var bill = _Context.Bills.Where(item => item.Id == input.BillId).Include(item=> item.Customer).FirstOrDefault();
            var notifications = _Context.Notifications.Where(item => item.Id == input.Id).FirstOrDefault();
            string link = $"http://localhost:8080/vue/enterbill/{CommonFunctions.Encryption(bill.Id.ToString())}";
            string content = $"Hoá đơn phòng {input.NumberOfRoom} được thanh toán vào ngày {bill.PaymentDate} đã bị từ chối do chủ nhà chưa nhận được tiền. " +
                $"Bạn hãy kiểm tra lại xem giao dịch đã được thực hiện thành công hay chưa." +
                $"Kiểm tra xong bạn vui lòng truy cập vào đường link {link} để xác nhận thanh toán lại. Xin cảm ơn!";
            await _Email.SendEmailAsync(bill.Customer.Email, "Từ chối thanh toán hoá đơn", content);
            bill.Status = StatusBill.ChuaThanhToan;
            _Context.Bills.Update(bill);
            notifications.ReadStatus = true;
            _Context.Notifications.Update(notifications);

            await _Context.SaveChangesAsync();
            
        }
        public async Task AcceptPayments(RefusePayReqModel input)
        {
            var bill = _Context.Bills.Where(item => item.Id == input.BillId).Include(item => item.Customer).FirstOrDefault();
            var notifications = _Context.Notifications.Where(item => item.Id == input.Id).FirstOrDefault();
            var arise = _Context.Incurs.Where(item => item.RoomId == bill.RoomId);

            string content = $"Hoá đơn phòng {input.NumberOfRoom} được thanh toán vào ngày {bill.PaymentDate} đã thanh toán thành công. ";
            await _Email.SendEmailAsync(bill.Customer.Email, "Thanh toán hoá đơn thành công", content);
            bill.Status = StatusBill.DaXacNhanThanhToan;
            _Context.Bills.Update(bill);
            notifications.ReadStatus = true;
            _Context.Notifications.Update(notifications);

            await _Context.SaveChangesAsync();

        }
        public List<GetAllBillByTowerResModel> GetAll(long towerId)
        {
            var billData = _Context.Bills.Include(b => b.Room).Where(item => item.Room.TowerId == towerId && !item.IsDeleted && item.Status != StatusBill.ChuaDienThongTin).OrderByDescending(item=> item.CreationDate)
                .Select(record => new GetAllBillByTowerResModel
                {
                    Id = record.Id,
                    NumberOfRoom = record.Room.Name,
                    PaymentDate = record.PaymentDate.ToString("dd/MM/yyyy"),
                    Time = record.PriceRoom != 0 ? record.CreationDate.AddMonths(-1).ToString("MM/yyyy") : record.CreationDate.ToString("MM/yyyy"),
                    TotalAmount = record.TotalAmount,
                    CustomerName = record.Customer.FullName,
                    Status = record.Status.GetDescription(),
                }).ToList();
            return billData;
        }
        public async Task DeleteBill(long billId) 
        {
            var bill = _Context.Bills.GetAvailableById(billId);
            if(bill.Status != StatusBill.ChuaThanhToan)
            {
                throw new Exception("Hoá đơn đã được thanh toán không thể xoá");
            }
            bill.IsDeleted = true;
            _Context.Bills.Update(bill);
            await _Context.SaveChangesAsync();
        }
        public GetDetailBillResModel GetDetail(long billId) 
        {
            Bill infoBill = _Context.Bills.Where(item => item.Id == billId && !item.IsDeleted).Include(item => item.Customer).FirstOrDefault();
            var contractid = _Context.Contracts.Where(item => item.RoomId == infoBill.RoomId && item.TerminationDate == null).Select(item => item.Id).FirstOrDefault();
            var roomDetails = _Context.Rooms
                   .Where(r => r.Id == infoBill.RoomId)
                   .Include(r => r.Tower) // Bao gồm thông tin Tower
                   .Select(r => new
                   {
                       Room = new
                       {
                           r.Id,
                           r.Name,
                           r.PriceRoom,
                       },
                       Tower = new
                       {
                           r.Tower.Id,
                           r.Tower.Address,
                           r.Tower.LandlordId
                       },
                   }).FirstOrDefault();
            if (roomDetails == null) throw new NotFoundException("Phòng");
            var result = new GetDetailBillResModel
            {
                Id = infoBill.Id,
                NumberOfRoom = roomDetails.Room.Name,
                PriceRoom = infoBill.PriceRoom,
                Date = infoBill.PriceRoom != 0 ? infoBill.CreationDate.AddMonths(-1).ToString("MM/yyyy") : infoBill.CreationDate.ToString("MM/yyyy"),
                Status = infoBill.Status,
                Amount = infoBill.TotalAmount,
                DatePayment = infoBill.PaymentDate.ToString("HH:mm - dd/MM/yyyy"),
                CustomerName = infoBill.Customer.FullName,
                
            };
            result.Service = _Context.ServiceInvoiceDetails.Where(item => item.BillId == infoBill.Id).Include(s => s.Service).Select(record => new NameServiceBillResModel
            {
                Id = record.ServiceId,
                Name = record.Service.Name,
                UnitPrice = record.UnitPrice,
                UsageNumber = record.UsageNumber,
                OldNumber = record.OldNumber,
                NewNumber = record.NewNumber,
            }).ToList();
            result.Arises = _Context.Incurs.Where(item => item.BillId == infoBill.Id && !item.IsDeleted).Select(record => new AriseBillResModel
            {
                Id = record.Id,
                Amount = record.Amount,
                Reason = record.Reason,
            }).ToList();
            return result;
        }
        public async Task UpdateBill(UpdateBillReqModel input)
        {
            // Lấy danh sách ServiceInvoiceDetails theo BillId và ServiceId
            var serviceInvoices = _Context.ServiceInvoiceDetails
                .Where(item => item.BillId == input.Id &&
                               input.Service.Select(s => s.Id).Contains(item.ServiceId))
                .ToList();
            var infoBill = _Context.Bills.GetAvailableById(input.Id);
            // Kiểm tra nếu không có bản ghi nào được tìm thấy
            if (serviceInvoices == null || !serviceInvoices.Any())
            {
                throw new Exception("Không tìm thấy dữ liệu để cập nhật.");
            }

            // Thực hiện cập nhật thông tin
            foreach (var invoice in serviceInvoices)
            {
                var inputService = input.Service.Where(s => s.Id == invoice.ServiceId).FirstOrDefault();
                if (inputService != null)
                {
                    invoice.OldNumber = inputService.OldNumber ?? invoice.OldNumber;
                    invoice.NewNumber = inputService.NewNumber ?? invoice.NewNumber;
                    invoice.NewNumber = inputService.NewNumber;
                    invoice.UsageNumber = inputService.NewNumber != null ? inputService.NewNumber.Value - inputService.OldNumber.Value : inputService.UsageNumber;
                    invoice.UnitPrice = inputService.UnitPrice;
                }
            }
            _Context.ServiceInvoiceDetails.UpdateRange(serviceInvoices);

            //Cập nhật hóa đơn
            infoBill.TotalAmount = infoBill.PriceRoom + input.SumArises + serviceInvoices.Sum(item => item.UnitPrice * item.UsageNumber);
            _Context.Bills.Update(infoBill);
            await _Context.SaveChangesAsync();
        }
        public List<CalculateRoomResModel> CalculateRoom(long towerId)
        {
            DateTime currentDate = DateTime.Now;
            var contractStillValid = _Context.Contracts.Include(r => r.Room).Where(c => c.TerminationDate == null && c.Room.TowerId == towerId && !c.Room.IsDeleted && !c.IsDeleted)
               .Where(r=> !_Context.Bills.Any(b=> b.RoomId == r.RoomId && b.CreationDate.Month ==currentDate.Month && b.CreationDate.Year == currentDate.Year && !b.IsDeleted))
               .Select(record => new CalculateRoomResModel
               {
                   ContractId = record.Id,
                   RoomId = record.RoomId,
                   NumberOfRoom = record.Room.Name,
                   PriceRoom = record.Room.PriceRoom,
                   
               }).ToList();
            if (contractStillValid.Count <= 0) throw new Exception("Tất cả các phòng đều có hóa đơn");
            foreach (var room in contractStillValid)
            {
                var OldNumberService = _Context.ServiceInvoiceDetails.Include(item => item.Bills)
                        .Where(item => item.Bills.RoomId == room.RoomId && !item.Bills.IsDeleted && item.NewNumber != null)
                        .OrderByDescending(b => b.Bills.CreationDate)
                        .Select(record => new { NewNumber = record.NewNumber,ServiceId = record.ServiceId } )
                        .ToList();
                var service = _Context.ServiceRooms.Where(item => item.ContractId == room.ContractId).Include(s => s.Service).Select(s => new ServiceCalculateRoomResModel
                {
                    Id = s.ServiceId,
                    Name = s.Service.Name,
                    UnitPrice = s.Price,
                    UsageNumber = s.Number,
                    IsOldNewNumber = s.IsOldNewNumber,
                    OldNumber = s.CurrentNumber,
                }).ToList();
                room.Services = service;
            }    

            return contractStillValid;
        }
        public async Task SendInvoice(List<CalculateRoomResModel> input)
        {
            foreach(var room in input)
            {
                var customer = _Context.ContractCustomers.Include(c => c.Customer)
                    .Where(item=> item.ContractId == room.ContractId && item.IsRepresentative == true)
                    .Select(record=> new
                    {
                        Id = record.CustomerId,
                        FullName = record.Customer.FullName,
                        Email = record.Customer.Email,
                    }).FirstOrDefault();
                if (customer == null) throw new NotFoundException($"Không có người đại diện của phòng {room.NumberOfRoom}");
                Bill newBiil = new Bill 
                {
                    CustomerId = customer.Id,
                    RoomId = room.RoomId,
                    CreationDate = DateOnly.FromDateTime(DateTime.Now),
                    PriceRoom = room.PriceRoom,
                    Status = StatusBill.ChuaThanhToan,
                    TotalAmount = 0
                };
                _Context.Bills.Add(newBiil);
                await _Context.SaveChangesAsync();

                /* var service = room.Services.Select(item=>new ServiceInvoiceDetails
                 {
                     BillId = newBiil.Id,
                     ServiceId = item.Id,
                     OldNumber = item.OldNumber,
                     NewNumber = item.NewNumber,
                     UnitPrice = item.UnitPrice,
                     UsageNumber = !item.IsOldNewNumber ? item.UsageNumber : item.NewNumber.Value - item.OldNumber.Value,
                 });*/

                //thêm các ServiceInvoiceDetails và cập nhận số dùng hiện tại vào bảng ServiceRooms
                decimal totalService = 0;
                foreach (var s in room.Services)
                {
                    ServiceInvoiceDetails service = new ServiceInvoiceDetails();
                    if (s.IsOldNewNumber == true)
                    {
                        var serviceRoom = _Context.ServiceRooms.Where(item=> item.ServiceId == s.Id && item.ContractId == room.ContractId).FirstOrDefault();
                        serviceRoom.CurrentNumber = (int)s.NewNumber;
                        _Context.ServiceRooms.Update(serviceRoom);
                    }
                    service.BillId = newBiil.Id;
                    service.ServiceId = s.Id;
                    service.OldNumber = s.OldNumber;
                    service.NewNumber = s.NewNumber;
                    service.UnitPrice = s.UnitPrice;
                    service.UsageNumber = !s.IsOldNewNumber ? s.UsageNumber : s.NewNumber.Value - s.OldNumber.Value;
                    totalService += service.UnitPrice * service.UsageNumber;
                    _Context.ServiceInvoiceDetails.Add(service);
                }

                var arise = _Context.Incurs.Where(item => (item.RoomId == room.RoomId && item.StatusPay == false) && !item.IsDeleted).ToList();
                foreach (var item in arise)
                {
                    item.BillId = newBiil.Id;
                }
                _Context.Incurs.UpdateRange(arise);

                newBiil.TotalAmount = newBiil.PriceRoom + arise.Sum(item => item.Amount) + totalService;
                _Context.Bills.Update(newBiil);
                
                await _Context.SaveChangesAsync();

                #region Gửi hóa đơn đến khách thuê
                string link = $"http://localhost:8080/vue/enterbill/{CommonFunctions.Encryption(newBiil.Id.ToString())}";
                string EmailContent = $"Xin chào {customer.FullName}. Đã có hóa đơn tháng {newBiil.CreationDate.AddMonths(-1).ToString("MM/yyyy")} của phòng {room.NumberOfRoom}.Bạn vui lòng truy cập vào link để xem và thanh toán hóa đơn \nĐây là link: {link}";
                await _Email.SendEmailAsync(customer.Email, "Nhập hóa đơn tháng mới", EmailContent);
                #endregion
            }
        }
    }
}
