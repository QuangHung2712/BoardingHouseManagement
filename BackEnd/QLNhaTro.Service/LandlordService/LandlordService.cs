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
using QLNhaTro.Moddel.Moddel.RequestModels.Landlord;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        public async Task UpdateLandlord(CreateEditLandlordReqModels input)
        {
            var landlord = _Context.Landlords.GetAvailableById(input.LandlordId);
            try
            {
                landlord = new Landlord
                {
                    FullName = input.FullName,
                    DoB = input.DoB,
                    PhoneNumber = input.PhoneNumber,
                    Email = input.Email,
                    CCCD = input.CCCD,
                    Address = input.Address,
                    SDTZalo=input.SDTZalo,
                };
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
    }
}
