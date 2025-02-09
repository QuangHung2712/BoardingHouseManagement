﻿using Microsoft.AspNetCore.Http;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.LandlordService
{
    public interface ILandlordService
    {
        long Login(LoginReqModels request);
        LandlordResModel GetDetail(long id);
        Task CreateLandlord(CreateEditLandlordReqModels input);
        Task UpdateLandlord(CreateEditLandlordReqModels input, IFormFile ImgQR);
        Task DeleteLandlord(long id);
        Task<long> ForgotPassword(string inputEmail);
        Landlord GetById(long id);
        Task ChangePassword(ChangePasswordReqModel input);
        GetInfoPaymentResModel GetInfoPayment(long id);
        Task UpdateInfoPayment(UpdateInfoPaymentReqModel input, IFormFile ImgQR);
        ContactInfoResModel GetContactInfo(long id);
        Task MonthlyReport();
        void CheckCode(int input, long landlordId);

    }
}
