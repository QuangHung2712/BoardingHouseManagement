using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.RequestModels.Landlord;
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
        Task UpdateLandlord(CreateEditLandlordReqModels input);
        Task DeleteLandlord(long id);
        bool ForgotPassword(string inputEmail);
        Landlord GetById(long id);
        Task ChangePassword(ChangePasswordReqModel input);
    }
}
