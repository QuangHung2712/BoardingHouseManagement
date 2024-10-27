using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
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
        public Task<List<GetAllRoomResModel>> GellAllRoomByTower(long toweId)
        {
            var roomData = _Context.Rooms.Where(r => !r.IsDeleted && r.TowerId == toweId).Select(r => new GetAllRoomResModel
            {
                Id = r.Id,
                NumberOfRoom = r.Name,
                PriceRoom = r.PriceRoom,
                Status = r.Status,
            }).ToListAsync();
            return roomData;
        }
        public async Task<GetRoomDetailByIdResModel> GetDetailRoomById(long roomId)
        {
            var roomdata = _Context.Rooms.Where(record => record.Id == roomId && !record.IsDeleted).Select(item => new GetRoomDetailByIdResModel 
            {
                Id = item.Id,
                NumberOfRoom = item.Name,
                //khách hàng
                NoPStaying = item.NoPStaying,
                PriceRoom  = item.PriceRoom,
                NumberElectric = item.NumberElectric,
                NumberCountries = item.NumberCountries,
                Note = item.Note,
                Status = item.Status,
            }).FirstOrDefault();
            if (roomdata == null) throw new NotFoundException(nameof(roomId));
            return roomdata;
        }
        public async Task CreateEditRoom(CreateEditRoomReqModel input)
        {
            if (input.Id <= 0)
            {
                try
                {
                    Room newRoom = new Room
                    {
                        TowerId = input.TowerId,
                        Name = input.NumberOfRoom,
                        Equipment = input.Equipment,
                        NoPStaying = input.NoPStaying,
                        PriceRoom = input.PriceRoom,
                        NumberElectric = input.NumberElectric,
                        NumberCountries = input.NumberCountries,
                        Note = input.Note,
                        Status = false,
                    };
                    _Context.Rooms.Add(newRoom);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
            else
            {
                try
                {
                    var room = _Context.Rooms.GetAvailableById(input.Id);
                    room.Name = input.NumberOfRoom;
                    room.Equipment = input.Equipment;
                    room.NoPStaying = input.NoPStaying;
                    room.PriceRoom = input.PriceRoom;
                    room.NumberElectric = input.NumberElectric;
                    room.NumberCountries = input.NumberCountries;
                    room.Note = input.Note;
                    _Context.Rooms.Update(room);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
        public async Task DeleteRoom(long roomId)
        {
            _Context.Rooms.Delete(roomId);
            DeleteImgRoomByRoomId(roomId);
            await _Context.SaveChangesAsync();
        }
        private void DeleteImgRoomByRoomId(long roomId)
        {
            var imgRoom = _Context.ImgRooms.Where(record => record.RoomId == roomId).ToList();
            _Context.ImgRooms.RemoveRange(imgRoom);            
        }
        public async Task FineNewCustomers(long roomId)
        {
            var room = _Context.Rooms.GetAvailableById(roomId);
            room.Status = true;
            _Context.Rooms.Update(room);
            await _Context.SaveChangesAsync();
        }
    }
}
