using Microsoft.AspNetCore.Http;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.RoomService
{
    public interface IRoomService
    {
        Task<List<GetAllRoomResModel>> GellAllRoomByTower(long toweId);
        Task<GetRoomDetailByIdResModel> GetDetailRoomById(long roomId);
        Task CreateEditRoom(CreateEditRoomReqModel input, List<IFormFile> imgs);
        void DeleteRoom(long roomId);
        Task FineNewCustomers(long roomId);
        Task CheckOut(CheckOutRoomReqModel input);
        Task ChangeRoom(ChangeRoomReqModel input);
        Task<List<GetDropDownRoom>> GetDropDownRooms(long towerId);
    }
}
