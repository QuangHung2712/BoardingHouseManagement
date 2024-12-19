using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.TowerService
{
    public interface ITowerService
    {
        Task<List<GetAllTowerResModel>> GetAllTowerByLandlordId(long LandlordId);
        Task CreateEditTower(CreateEditTowerReqModel input);
        void DeleteTower(long Id);
    }
}
