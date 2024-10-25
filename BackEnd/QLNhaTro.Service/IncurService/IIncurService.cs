using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.IncurService
{
    public interface IIncurService
    {
        Task DeleteIncur(long Id);
        Task CreateEditIncur(CreateEditIncurReqModel input);
        Task<List<GetAllIncurResModel>> GetAllByTower(long towerId);
    }
}
