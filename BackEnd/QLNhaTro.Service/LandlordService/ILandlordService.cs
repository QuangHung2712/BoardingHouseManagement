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
        Task<LandlordResModel> GetDetail(long id);
        Task CreateLandlord(CreateEditLandlordReqModels input);
        Task UpdateLandlord(CreateEditLandlordReqModels input);
        Task DeleteLandlord(long id);
    }
}
