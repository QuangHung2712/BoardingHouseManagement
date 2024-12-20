using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Vml.Office;
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
using Contract = QLNhaTro.Moddel.Entity.Contract;

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
            var roomData = await _Context.Rooms.Where(r => !r.IsDeleted && r.TowerId == toweId).Select(r => new GetAllRoomResModel
            {
                Id = r.Id,
                NumberOfRoom = r.Name,
                NoPStaying = r.NoPStaying,
                PriceRoom = r.PriceRoom,
                Status = r.StatusNewCustomer,
            }).ToListAsync();
            foreach (var room in roomData)
            {
                room.CustomerName = GetCustomerByRoom(room.Id); // Gọi phương thức GetCustomerByRoom bên ngoài LINQ
            }
            return roomData;
        }
        private string GetCustomerByRoom(long roomId)
        {
            var data = _Context.Contracts.Where(record=> record.RoomId == roomId && record.TerminationDate ==null && !record.IsDeleted)
                                .SelectMany(record=> record.Customers)
                                .Select(item=> item.FullName).ToList();
            if(data.Count > 0)
            {
                return string.Join(",", data);
            }
            return "";
        }
        public async Task<GetRoomDetailByIdResModel> GetDetailRoomById(long roomId)
        {
            var roomdata = await _Context.Rooms.Where(record => record.Id == roomId && !record.IsDeleted).Select(item => new GetRoomDetailByIdResModel 
            {
                Id = item.Id,
                NumberOfRoom = item.Name,
                Equipment = item.Equipment,
                CustomerName = _Context.Contracts.Where(c=> c.RoomId == roomId && c.TerminationDate == null && !c.IsDeleted)
                                                .SelectMany(c=> c.Customers)
                                                .Select(c=> CustomerResModel.Mapping(c))
                                                .ToList(),
                NoPStaying = item.NoPStaying,
                PriceRoom  = item.PriceRoom,
                NumberElectric = item.NumberElectric,
                NumberCountries = item.NumberCountries,
                Note = item.Note,
                Status = item.StatusNewCustomer,
            }).FirstOrDefaultAsync();
            if (roomdata == null) throw new NotFoundException(nameof(roomId));
            return roomdata;
        }
        public Task<List<GetDropDownRoom>> GetDropDownRooms(long towerId)
        {
            var roomdata = _Context.Rooms.Where(record=> record.TowerId == towerId && !record.IsDeleted).Select(item => new GetDropDownRoom
            {
                Id= item.Id,
                Name = item.Name,
            }).ToListAsync();
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
                        StatusNewCustomer = false,
                    };
                    _Context.Rooms.Add(newRoom);

                    //SaveImgToDB(imgs, newRoom.Id, input.TowerId);
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
                    //DeleteImgRoomByRoomId(input.Id, input.TowerId);
                    //SaveImgToDB(imgs, input.Id, input.TowerId);
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
        public void DeleteRoom(long roomId)
        {
            var roomData = _Context.Rooms.GetAvailableById(roomId);
            roomData.IsDeleted = true;
            _Context.Rooms.Update(roomData);
            _Context.SaveChanges();
            //DeleteImgRoomByRoomId(roomId, towerId);
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
            _Context.SaveChangesAsync();
        }
        public async Task FineNewCustomers(long roomId)
        {
            var room = _Context.Rooms.GetAvailableById(roomId);
            room.StatusNewCustomer = true;
            _Context.Rooms.Update(room);
            await _Context.SaveChangesAsync();
        }
        public async Task CheckOut(CheckOutRoomReqModel input)
        {
            var contract =  _Context.Contracts.Where(record=> record.RoomId == input.Id && record.TerminationDate == null && !record.IsDeleted).FirstOrDefault();
            var room = _Context.Rooms.GetAvailableById(input.Id);
            if(contract == null) throw new NotFoundException(nameof(contract));
            contract.TerminationDate = DateTime.Now;
            room.StatusNewCustomer = true;
            _Context.Contracts.Update(contract);
            _Context.Rooms.Update(room);
            await _Context.SaveChangesAsync();
        }
        public async Task ChangeRoom(ChangeRoomReqModel input)
        {
            //Lấy thông tin của phòng cũ và mới
            var roomNew = _Context.Rooms.Where(r => r.Id == input.RoomIdNew).FirstOrDefault();
            var roomOld = _Context.Rooms.Where(r => r.Id == input.RoomIdOld).FirstOrDefault();
            if(roomOld == null || roomNew == null)
            {
                throw new NotFoundException($"{input.RoomIdOld} và {input.RoomIdNew}");
            }

            var contractOld = _Context.Contracts
            .SingleOrDefault(record => record.RoomId == input.RoomIdOld && !record.IsDeleted && record.TerminationDate == null)
            ?? throw new NotFoundException(nameof(input.RoomIdOld));
            //Tạo hợp đồng mới và cập nhật lại hợp đồng cũ
            Contract contractNew = new Contract
            {
                RoomId = input.RoomIdNew,
                StartDate = input.TimesChange,
                EndDate = input.TimesChange.AddMonths(input.ContractPeriod),
                Deposit = contractOld.Deposit,
            };
            contractOld.TerminationDate = input.TimesChange;
            contractOld.Note = $"Khách đã chuyển sang phòng {roomNew.Name} ở địa chỉ {roomNew.Tower.Address}";
            _Context.Contracts.Update(contractOld);
            _Context.Contracts.Add(contractNew);
            await _Context.SaveChangesAsync();

            //Cập nhập lại hợp đồng ở bảng khách hàng
            var customer = _Context.Customers.Where(c => c.ContractId == contractOld.Id && c!.IsDeleted).ToList();
            foreach (var item in customer)
            {
                item.ContractId = contractNew.Id;
            }
            _Context.Customers.UpdateRange(customer);

            //Thêm lại các dịch vụ
            var contractService = input.Services.Select(s => new ServiceRoom
            {
                ContractId = contractNew.Id,
                ServiceId = s.ServiceId,
                Price = s.Price,
                Number = s.Number,
            }).ToList();
            await _Context.ServiceRooms.AddRangeAsync(contractService);

            //Cập nhật lại trạng thái phòng
            roomOld.StatusNewCustomer = true;
            roomNew.StatusNewCustomer = false;
            _Context.Rooms.UpdateRange(roomOld, roomNew);

            //Thêm phát sinh nếu 2 phòng có giá khác nhau
            if(roomOld.PriceRoom != roomNew.PriceRoom)
            {
                Incur incurNew = new Incur() 
                {
                    RoomId = roomNew.Id,
                    Amount = roomNew.PriceRoom - roomOld.PriceRoom,
                    CreationDate = input.TimesChange,
                    Reason = "Tiền cọc chênh của phòng mới và phòng cũ",
                    TowerId = roomNew.TowerId,
                };
                _Context.Incurs.Add(incurNew);
            }
            
            //Lưu tất cả các thông tin vừa chỉnh sửa
            await _Context.SaveChangesAsync();
        }
        public async Task<List<GetDropDownRoom>> GetRoomNoContract(long towerId)
        {
            DateTime currentDate = DateTime.Now;

            // Lấy danh sách ID của các phòng đã có hợp đồng hợp lệ
            var occupiedRoomIds = _Context.Contracts
                .Where(contract =>
                    (contract.TerminationDate == null || contract.TerminationDate >= currentDate) && !contract.IsDeleted) // Hợp đồng vẫn còn hiệu lực
                .Select(contract => contract.RoomId)
                .Distinct();
            var availableRooms = _Context.Rooms
            .Where(room => !occupiedRoomIds.Contains(room.Id) && room.TowerId == towerId) // Phòng không thuộc danh sách đã có hợp đồng hợp lệ
            .Select(room => new GetDropDownRoom
            {
                Id = room.Id,
                Name = room.Name,
            }).ToList();
            return availableRooms;
        }
    }
}
