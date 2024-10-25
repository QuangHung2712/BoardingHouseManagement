using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _Context;

        public RoomService(AppDbContext context)
        {
            _Context = context;
        }
        public async Task<List<GetAllRoomResModel>> GellAllRoomByTower(long toweId)
        {
            var roomData = _Context.Rooms.Where(r=> !r.IsDeleted && r.TowerId == toweId
        }
    }
}
