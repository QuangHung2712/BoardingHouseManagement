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
        Task CreateEditRoom(CreateEditRoomReqModel input, List<IFormFile> ImgRoom);
        void DeleteRoom(long roomId);
        Task FineNewCustomers(long roomId);
        Task CancelFineNewCustomers(long roomId);
        Task<string> CheckOut(CheckOutRoomReqModel input);
        Task ChangeRoom(ChangeRoomReqModel input);
        Task<List<GetDropDownRoom>> GetDropDownRooms(long towerId);
        Task<List<GetDropDownRoom>> GetRoomNoContract(long towerId);
        GetInfomationHomeResModel GetInfoHome(long towerId);
        List<GetInfoCheckOutRoomResModel> GetInfoCheckout(long roomId);
        List<SearchRoomResModel> SearchRoom(string address, decimal priceForm, decimal priceArrive, long customerId);
        GetRoomDetailFindRoomResModel GetRoomDetailFindRoom(long Id, long customerId);
        Task SaveRoom(long customerId, long roomId, bool status);

    }
}
