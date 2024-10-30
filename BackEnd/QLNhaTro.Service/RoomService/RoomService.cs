using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                CustomerName = GetCustomerByRoom(r.Id),
                PriceRoom = r.PriceRoom,
                Status = r.Status,
            }).ToListAsync();
            return roomData;
        }
        private string GetCustomerByRoom(long roomId)
        {
            var data = _Context.Contracts.Where(record=> record.RoomId == roomId)
                                .SelectMany(record=> record.Customers)
                                .Select(item=> item.FullName).ToList();
            return string.Join(", ", data);
        }
        public async Task<GetRoomDetailByIdResModel> GetDetailRoomById(long roomId)
        {
            var roomdata = await _Context.Rooms.Where(record => record.Id == roomId && !record.IsDeleted).Select(item => new GetRoomDetailByIdResModel 
            {
                Id = item.Id,
                NumberOfRoom = item.Name,
                CustomerName = _Context.Contracts.Where(c=> c.RoomId == roomId)
                                                .SelectMany(c=> c.Customers)
                                                .Select(c=> CustomerResModel.Mapping(c))
                                                .ToList(),
                NoPStaying = item.NoPStaying,
                PriceRoom  = item.PriceRoom,
                NumberElectric = item.NumberElectric,
                NumberCountries = item.NumberCountries,
                Note = item.Note,
                Status = item.Status,
            }).FirstOrDefaultAsync();
            if (roomdata == null) throw new NotFoundException(nameof(roomId));
            return roomdata;
        }
        public async Task CreateEditRoom(CreateEditRoomReqModel input, List<IFormFile> imgs)
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

                    SaveImgToDB(imgs, newRoom.Id, input.TowerId);
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
                    DeleteImgRoomByRoomId(input.Id, input.TowerId);
                    SaveImgToDB(imgs, input.Id, input.TowerId);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
        private async void SaveImgToDB(List<IFormFile> Imgs, long roomId, long towerId)
        {
            // Tạo thư mục lưu ảnh dựa trên TowerId và RoomId
            var folderPath = Path.Combine("D:/QuanLyNhaTro", towerId.ToString(), roomId.ToString());
            try
            {
                if (Imgs != null && Imgs.Count > 0)
                {
                    // Kiểm tra và tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    foreach (var img in Imgs)
                    {
                        if (img.Length > 0)
                        {
                            // Đặt tên file duy nhất
                            var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                            var filePath = Path.Combine(folderPath, fileName);

                            // Lưu ảnh vào thư mục
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await img.CopyToAsync(stream);
                            }

                            // Lưu đường dẫn ảnh vào database
                            var contractImage = new ImgRoom
                            {
                                RoomId = roomId,
                                Path = "images/" + fileName
                            };

                            //Lưu đường dẫn vào database
                            _Context.ImgRooms.Add(contractImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task DeleteRoom(long roomId,long towerId)
        {
            _Context.Rooms.Delete(roomId);
            DeleteImgRoomByRoomId(roomId, towerId);
            await _Context.SaveChangesAsync();
        }
        private void DeleteImgRoomByRoomId(long roomId,long towerId)
        {
            // Tạo thư mục lưu ảnh dựa trên TowerId và RoomId
            var folderPath = Path.Combine("D:/QuanLyNhaTro", towerId.ToString(), roomId.ToString());
            try
            {
                // Kiểm tra xem thư mục có tồn tại không
                if (Directory.Exists(folderPath))
                {
                    // Lấy tất cả các file trong thư mục
                    var files = Directory.GetFiles(folderPath);

                    Directory.Delete(folderPath, true);
                    Console.WriteLine("Đã xóa thư mục và tất cả nội dung trong: " + folderPath);
                }
                else
                {
                    throw new NotFoundException(nameof(roomId.ToString));
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
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
