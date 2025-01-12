using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.EmailService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonConstants;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.Service.LandlordService
{
    public class LandlordService : ILandlordService
    {
        private readonly AppDbContext _Context;
        private readonly IEmailService _EmailService;
        private readonly IMemoryCache _memoryCache;
        public LandlordService(AppDbContext context, IEmailService emailService, IMemoryCache memoryCache)
        {
            _Context = context;
            _EmailService = emailService;
            _memoryCache = memoryCache;
        }
        public long Login(LoginReqModels request)
        {
             return _Context.Landlords.Where(item=> item.Email == request.Email && item.Password == request.Password && !item.IsDeleted && item.IsActive).Select(record => record.Id).FirstOrDefault();
        }
        public LandlordResModel GetDetail(long id)
        {
            try
            {
                Landlord landlord = _Context.Landlords.GetAvailableById(id);

                return LandlordResModel.Mapping(landlord);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public GetInfoPaymentResModel GetInfoPayment(long id) 
        {
            var landlord =  _Context.Landlords.Where(item => item.Id == id && !item.IsDeleted).Select(record => new GetInfoPaymentResModel
            {
                Bank = record.Bank,
                PaymentQRImageLink = CommonFunctions.ConverPathIMG(record.PaymentQRImageLink),
                STK = record.STK
            }).FirstOrDefault();
            if (landlord == null) 
            {
                throw new NotFoundException("Chủ nhà không tồn tại");
            }
            return landlord;
        }
        public async Task UpdateInfoPayment(UpdateInfoPaymentReqModel input,IFormFile ImgQR)
        {
            var landlord = _Context.Landlords.GetAvailableById(input.Id);
            landlord.Bank = input.bank;
            landlord.STK = input.STK;
            landlord.PaymentQRImageLink = CommonFunctions.SaveImgLocal(ImgQR, landlord.Id,landlord.PaymentQRImageLink, "Ảnh QR ngân hàng thanh toán",FeatureCode.Landlord);
            _Context.Landlords.Update(landlord);
            await _Context.SaveChangesAsync();
        }
        public async Task CreateLandlord(CreateEditLandlordReqModels input)
        {
            try
            {
                bool IsEmail = _Context.Landlords.Any(item => item.Email != null && item.Email.ToLower() == input.Email.ToLower());
                if (IsEmail)
                {
                    throw new AlreadyExistException(nameof(input.Email));
                }
                var landlord = new Landlord
                {
                    FullName = input.FullName,
                    DoB = input.DoB,
                    PhoneNumber = input.PhoneNumber,
                    Email = input.Email,
                    CCCD = input.CCCD,
                    Address = input.Address,
                    SDTZalo = input.SDTZalo,
                    Password = CommonConstants.DefaultValue.DEFAULT_PASSWORD,
                    PathAvatar = CommonConstants.DefaultValue.DEFAULT_BASE_Directory_IMG,
                    Bank = "Chưa có",
                };
                _Context.Landlords.Add(landlord);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task UpdateLandlord(CreateEditLandlordReqModels input, IFormFile ImgAvatar)
        {
            var landlord = _Context.Landlords.GetAvailableById(input.LandlordId);
            try
            {
                landlord.FullName = input.FullName;
                landlord.DoB = input.DoB;
                landlord.PhoneNumber = input.PhoneNumber;
                landlord.Email = input.Email;
                landlord.CCCD = input.CCCD;
                landlord.Address = input.Address;
                landlord.SDTZalo = input.SDTZalo;
                landlord.PathAvatar = CommonFunctions.SaveImgLocal(ImgAvatar, landlord.Id, landlord.PathAvatar, "Avatar",FeatureCode.Landlord);
                _Context.Landlords.Update(landlord);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task DeleteLandlord(long id)
        {
            var landlord = _Context.Landlords.Where(record => record.Id == id && record.IsDeleted == false).FirstOrDefault();
            if (landlord == null) throw new NotFoundException(nameof(id));
            try
            {
                landlord.IsDeleted = true;
                _Context.Landlords.Update(landlord);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<long> ForgotPassword(string inputEmail)
        {
            var landlord =  _Context.Landlords.Where(item => item.Email != null && item.Email.ToLower() == inputEmail.ToLower()).FirstOrDefault();
            if(landlord == null)
            {
                throw new Exception("Email không tồn tại");
            }
            var cacheKey = $"PasswordReset_{landlord.Id}";
            int code = await _EmailService.SendEmailCode(landlord.Email, "Quên mật khẩu");
            _memoryCache.Set(cacheKey, code, DateTimeOffset.UtcNow.AddMinutes(10));
            return landlord.Id;

        }
        public void CheckCode(int input,long landlordId)
        {
            var cacheKey = $"PasswordReset_{landlordId}";
            _memoryCache.TryGetValue(cacheKey, out string? resetCode);
            if(resetCode != input.ToString())
            {
                throw new Exception("Mã xác nhận không đúng hoặc đã hết hạn sử dụng");
            }
        }
        public Landlord GetById(long id)
        {
            try
            {
                Landlord landlordQuery = _Context.Landlords.GetAvailableById(id);
                if (landlordQuery == null) throw new NotFoundException("Tài khoản chủ nhà không tồn tại");
                return landlordQuery;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task ChangePassword(ChangePasswordReqModel input)
        {
            var landlord = _Context.Landlords.GetAvailableById(input.Id);
            if(landlord.Password != input.PasswordOld)
            {
                throw new Exception("Mật khẩu không chính xác");
            }
            landlord.Password = input.PasswordNew;
            _Context.Landlords.Update(landlord);
            await _Context.SaveChangesAsync();
        }
        public ContactInfoResModel GetContactInfo(long id)
        {
            var infoContact = _Context.Landlords.GetAvailableById(id);
            return new  ContactInfoResModel
            {
                PathAvatar = CommonFunctions.ConverPathIMG(infoContact.PathAvatar),
                PhoneNumber = infoContact.PhoneNumber,
                SDTZalo = infoContact.SDTZalo,
                FullName = infoContact.FullName,
            };

        }
        
        public async Task MonthlyReport()
        {
            var date = DateTime.Now.ToString("MM/yyyy");
            var landlords = _Context.Landlords.Where(item => item.IsActive && !item.IsDeleted).ToList();
            foreach(var landlord in landlords)
            {
                string content = $"Đây là báo tháng {date}. Các thông tin được lấy đến ngày 20";
                decimal Total = 0;
                var Towers = _Context.Towers.Where(item => item.LandlordId == landlord.Id && !item.IsDeleted).ToList();
                foreach(var tower in Towers)
                {
                    var occupiedRoomIds = _Context.Contracts
                       .Where(contract =>
                           contract.TerminationDate == null && !contract.IsDeleted) // Hợp đồng vẫn còn hiệu lực
                       .Select(contract => contract.RoomId)
                       .Distinct();
                    var availableRooms = _Context.Rooms
                    .Where(room => !occupiedRoomIds.Contains(room.Id) && room.TowerId == tower.Id).Select(item => new
                    {
                        name = item.Name,
                    }).ToList();
                   

                    var bills = _Context.Bills.Include(item => item.Room).Where(item => item.CreationDate.Month == DateTime.Now.Month && item.Room.TowerId == tower.Id).ToList();
                    var RoomUnpaid = bills.Where(item => item.Status == CommonEnums.StatusBill.DaXacNhanThanhToan).Select(item => new { item.Room.Name }).ToList();
                    var RoomPaid = bills.Where(item => item.Status == CommonEnums.StatusBill.ChuaThanhToan || item.Status == CommonEnums.StatusBill.ChuaXacNhanThanhToan).Select(item => new { item.Room.Name }).ToList();

                    //Phòng chưa thanh toán
                    long TotalRoomUnpaid = RoomUnpaid.Count;
                    string InfoRoomUnpaid = string.Join(", ", RoomUnpaid.Select(item => item.Name));

                    //Phòng đã thanh toán
                    long TotalRoomPaid = RoomPaid.Count;
                    string InfoRoomPaid = string.Join(", ", RoomPaid.Select(item => item.Name));

                    //Phòng chưa có hóa đơn
                    long RoomNoInvoice = bills.Count - RoomUnpaid.Count - RoomPaid.Count;
                    string InfoRoomNoInvoice = string.Join(", ", (bills.Where(item => item.Status == CommonEnums.StatusBill.ChuaDienThongTin).Select(item => new { item.Room.Name }).ToList()).Select(item => item.Name));

                    //Phòng còn trống
                    int RoomAvailable = availableRooms.Count();
                    string InfoRoomAvailable = string.Join(", ", availableRooms.Select(item => item.name));

                    long RoomExpireContract = 5;
                    string InfoRoomExpireContract = "";
                    decimal TotalAmount = bills.Where(item => item.Status == CommonEnums.StatusBill.DaXacNhanThanhToan).Sum(item => item.TotalAmount);
                    Total += TotalAmount;
                    content += $"Đây là báo cáo của tòa nhà {tower.Name} ở địa {tower.Address} " +
                                $"      Số phòng chưa thanh toán: {TotalRoomUnpaid} đó là các phòng {InfoRoomUnpaid}" +
                                $"      Sô phòng đã thánh toán: {TotalRoomUnpaid} đó là các phòng {InfoRoomUnpaid}" +
                                $"      Số phòng chưa có hóa đơn: {RoomAvailable} đó là các phòng {InfoRoomAvailable}" +
                                $"      Số phòng còn trống: {RoomAvailable} đó là các phòng {InfoRoomAvailable}" +
                                $"      Số phòng sắp hết hợp đồng {RoomExpireContract} đó là các phòng {InfoRoomExpireContract}" +
                                $"      Tổng số tiền thu được ở tòa nhà này là {TotalAmount}";
                }
                content += $"Tổng số tiền bạn đã thu được trong tháng {date} là: {Total}VND";
                await _EmailService.SendEmailAsync(landlord.Email, $"Báo cáo tháng {date}", content);
            }
        }

    }
}
