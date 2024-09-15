using QLNhaTro.Moddel.Moddel.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.LandlordService
{
    public interface ILandlordService
    {
        Task<bool> CreateEditLandlord(CreateEditLandlordReqModels input);
        Task<bool> DeleteLandlord(long id);
    }
}
