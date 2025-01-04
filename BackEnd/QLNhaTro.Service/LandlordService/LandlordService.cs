using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
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
        public LandlordService(AppDbContext context)
        {
            _Context = context;
        }
        public long Login(LoginReqModels request)
        {
             return _Context.Landlords.Where(item=> item.Email == request.Email && item.Password == request.Password).Select(record => record.Id).FirstOrDefault();
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
            landlord.STK = input.STK;
            landlord.PaymentQRImageLink = SaveImgLocal(ImgQR, landlord.Id,landlord.PaymentQRImageLink, "Ảnh QR ngân hàng thanh toán");
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
                    Password = "defaultpassword",
                    STK = "Chưa có",
                    PaymentQRImageLink="Chưa có",
                    SampleContractLink= "Chưa có"
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
                landlord.PathAvatar = SaveImgLocal(ImgAvatar, landlord.Id, landlord.PathAvatar, "Avatar");
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
        public bool ForgotPassword(string inputEmail)
        {
            return _Context.Landlords.Any(item => item.Email != null && item.Email.ToLower() == inputEmail.ToLower());
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
                SDTZalo = infoContact.SDTZalo
            };

        }
        private string SaveImgLocal(IFormFile input,long userId,string PathImgQROld,string nameFile)
        {
            if (!string.IsNullOrEmpty(PathImgQROld))
            {
                File.Delete(PathImgQROld);
            }
            // Tạo GUID cho tên ảnh
            string fileName = nameFile + Path.GetExtension(input.FileName);

            // Đường dẫn thư mục lưu ảnh
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $@"D:\Code\BoardingHouseManagement\BoardingHouseManagement\FrontEnd\public\images\UserInformation\{userId}");

            // Kiểm tra và tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Đường dẫn đầy đủ của ảnh
            string filePath = Path.Combine(directoryPath, fileName);

            // Lưu file vào đường dẫn đã chỉ định
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                input.CopyTo(stream);
            }

            // Trả về đường dẫn ảnh đã lưu
            return filePath;
        }
        
    }
}
