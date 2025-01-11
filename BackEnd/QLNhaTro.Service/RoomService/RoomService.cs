using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.EmailService;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonConstants;
using static System.Net.Mime.MediaTypeNames;
using Contract = QLNhaTro.Moddel.Entity.Contract;

namespace QLNhaTro.Service.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _Context;
        private readonly IEmailService _emailService;

        public RoomService(AppDbContext context, IEmailService emailService)
        {
            _Context = context;
            _emailService = emailService;
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
            var data = _Context.ContractCustomers.Where(record => record.Contract.RoomId == roomId && record.Contract.TerminationDate == null && !record.Contract.IsDeleted)
                                  .Select(item => item.Customer.FullName).ToList();
            if (data.Count > 0)
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
                IsCustomer = _Context.Contracts.Any(item=> item.RoomId == roomId && !item.IsDeleted && item.TerminationDate == null),
                NoPStaying = item.NoPStaying,
                PriceRoom  = item.PriceRoom,
                NumberElectric = item.NumberElectric,
                NumberCountries = item.NumberCountries,
                Note = item.Note,
                Status = item.StatusNewCustomer,
            }).FirstOrDefaultAsync();
            roomdata.PathImgRoom= ConverPathListIMG(_Context.ImgRooms.Where(img => img.RoomId == roomdata.Id).Select(img => img.Path).ToList());
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
        public async Task CreateEditRoom(CreateEditRoomReqModel input, List<IFormFile> ImgRoom)
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
                        StatusNewCustomer = true,
                    };
                    _Context.Rooms.Add(newRoom);
                    await _Context.SaveChangesAsync();
                    SaveImgToDB(ImgRoom, newRoom.Id, input.TowerId);
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
                    string path = @$"{DefaultValue.DEFAULT_BASE_Directory_IMG}\images\Room\{room.TowerId}\{room.Id}";
                    DeleteImgRoomByRoomId(path, room.Id);
                    SaveImgToDB(ImgRoom, input.Id, input.TowerId);
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
        private void SaveImgToDB(List<IFormFile> Imgs, long roomId, long towerId)
        {
            try
            {
                // Đường dẫn thư mục lưu ảnh
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $@"{DefaultValue.DEFAULT_BASE_Directory_IMG}\images\Room\{towerId}\{roomId}");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                foreach (var img in Imgs)
                {
                    if (img.Length > 0)
                    {
                        // Đặt tên file duy nhất
                        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);

                        // Đường dẫn đầy đủ của ảnh
                        var filePath = Path.Combine(directoryPath, fileName);

                        // Lưu file vào đường dẫn đã chỉ định
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            img.CopyTo(stream);
                        }

                        var imgRoom = new ImgRoom { RoomId = roomId,Path = filePath };
                        _Context.ImgRooms.Add(imgRoom);

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
        private void DeleteImgRoomByRoomId(string path,long roomId)
        {
            try
            {
                // Kiểm tra xem thư mục có tồn tại không
                if (Directory.Exists(path))
                {
                    // Lấy tất cả các file trong thư mục
                    var files = Directory.GetFiles(path);

                    Directory.Delete(path, true);
                    Console.WriteLine("Đã xóa thư mục và tất cả nội dung trong: " + path);
                }
                else
                {
                    return;
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
            if (room.StatusNewCustomer == true) throw new Exception("Phòng đang được tìm khách mới rồi");
            room.StatusNewCustomer = true;
            _Context.Rooms.Update(room);
            await _Context.SaveChangesAsync();
        }
        public async Task CancelFineNewCustomers(long roomId)
        {
            var room = _Context.Rooms.GetAvailableById(roomId);
            if (room.StatusNewCustomer == false) throw new Exception("Phòng đang không tìm khách mới");
            room.StatusNewCustomer = false;
            _Context.Rooms.Update(room);
            await _Context.SaveChangesAsync();
        }
        public List<GetInfoCheckOutRoomResModel> GetInfoCheckout(long roomId)
        {
            return _Context.ServiceRooms.Include(r=> r.Contract)
                .Where(item=> item.Contract.RoomId == roomId && !item.Contract.IsDeleted)
                .Select(record => new GetInfoCheckOutRoomResModel
                {
                    ServiceId = record.ServiceId,
                    OldNumber = record.CurrentNumber,
                    ServiceName = record.Service.Name,
                    UnitPrice  =record.Price,
                    IsOldNewNumber = record.IsOldNewNumber,
                    UsageNumber = record.Number 
                }).ToList();
        }
        public async Task<string> CheckOut(CheckOutRoomReqModel input)
        {
            var contract =  _Context.Contracts.Where(record=> record.RoomId == input.Id && record.TerminationDate == null && !record.IsDeleted).FirstOrDefault();
            var room = _Context.Rooms.GetAvailableById(input.Id);
            if(contract == null) throw new NotFoundException(nameof(contract));
            var customer = _Context.ContractCustomers.Include(r => r.Customer)
                .Where(item => item.ContractId == contract.Id && item.IsRepresentative)
                .Select(c => new
                {
                    CustomerId = c.CustomerId,
                    Fullname = c.Customer.FullName,
                    Email = c.Customer.Email,
                }).FirstOrDefault();
            contract.TerminationDate = DateTime.Now;
            room.StatusNewCustomer = true;

            //Tạo hoá đơn
            Bill newBill = new Bill
            {
                RoomId = room.Id,
                CustomerId = customer.CustomerId,
                CreationDate = DateOnly.FromDateTime(DateTime.Now),
                PriceRoom = room.PriceRoom,
                PaymentDate = DateTime.Now,
                Status = CommonEnums.StatusBill.DaXacNhanThanhToan,
                TotalAmount = 0,
            };
            _Context.Bills.Add(newBill);
            await _Context.SaveChangesAsync();

            if(input.MoneyPunish != null)
            {
                Incur newArise = new Incur
                {
                    BillId = newBill.Id,
                    Amount = input.MoneyPunish.Value,
                    RoomId = room.Id,
                    Reason = input.Note,
                    StatusPay = true,
                    CreationDate = DateTime.Now,
                    TowerId = room.TowerId,
                };
                newBill.TotalAmount = input.MoneyPunish.Value;
                _Context.Incurs.Add(newArise);
            }
            var arise = _Context.Incurs.Where(item => (item.RoomId == room.Id && item.StatusPay == false) && !item.IsDeleted).ToList();
            foreach (var item in arise)
            {
                item.BillId = newBill.Id;
            }
            _Context.Incurs.UpdateRange(arise);
            var service = input.Service.Select(item => new ServiceInvoiceDetails
            {
                BillId = newBill.Id,
                ServiceId = item.ServiceId,
                NewNumber = item.NewNumber,
                OldNumber = item.OldNumber,
                UnitPrice = item.UnitPrice,
                UsageNumber = item.IsOldNewNumber ? item.NewNumber.Value - item.OldNumber : item.UsageNumber,

            });
            _Context.ServiceInvoiceDetails.AddRange(service);
            newBill.TotalAmount += newBill.PriceRoom + arise.Sum(item => item.Amount) + service.Sum(item=> item.UnitPrice * item.UsageNumber);
            _Context.Bills.Update(newBill);

            _Context.Contracts.Update(contract);
            _Context.Rooms.Update(room);

            await _Context.SaveChangesAsync();

            #region Gửi hóa đơn đến khách thuê
            string link = $"http://localhost:8080/vue/enterbill/{CommonFunctions.Encryption(newBill.Id.ToString())}";
            string EmailContent = $"Xin chào {customer.Fullname}.Sau khi quyết toàn tiền phòng bạn còn lại  {contract.Deposit - newBill.TotalAmount} VND. Bạn vui lòng truy cập vào link để xem hóa đơn \nĐây là link: {link}";
            await _emailService.SendEmailAsync(customer.Email, "Nhập hóa đơn tiền phòng", EmailContent);
            #endregion

            //Tính tiền thừa
            return $"Bạn phải thanh toán lại cho khách thuê {contract.Deposit} - {newBill.TotalAmount} = {contract.Deposit - newBill.TotalAmount} VND";
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


            //Thêm lại các dịch vụ
            var contractService = input.Services.Select(s => new ServiceRoom
            {
                ContractId = contractNew.Id,
                ServiceId = s.ServiceId,
                Price = s.Price,
                Number = s.Number.Value,
                CurrentNumber = s.CurrentNumber.Value
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
        public GetInfomationHomeResModel GetInfoHome(long towerId)
        {
            var occupiedRoomIds = _Context.Contracts
               .Where(contract =>
                   contract.TerminationDate == null && !contract.IsDeleted) // Hợp đồng vẫn còn hiệu lực
               .Select(contract =>new 
               { 
                   RoomId = contract.RoomId ,
                   EndDate = contract.EndDate
               })
               .ToList();
            var availableRooms = _Context.Rooms
            .Where(room => !occupiedRoomIds.Select(item=> item.RoomId).Contains(room.Id) && room.TowerId == towerId).Select(item=> new
            {
                name = item.Name,
            }).ToList();
            int RoomAvailable = availableRooms.Count();
            string InfoRoomAvailable = string.Join(", ", availableRooms.Select(item=> item.name));

            var bills = _Context.Bills.Include(item => item.Room ).Where(item => item.CreationDate.Month == DateTime.Now.Month && item.Room.TowerId == towerId).ToList();
            var RoomUnpaid = bills.Where(item => item.Status == CommonEnums.StatusBill.DaXacNhanThanhToan).Select(item =>new { item.Room.Name }).ToList();
            var RoomPaid = bills.Where(item => item.Status == CommonEnums.StatusBill.ChuaThanhToan || item.Status == CommonEnums.StatusBill.ChuaXacNhanThanhToan).Select(item => new { item.Room.Name }).ToList();
            string InfoRoomNoInvoice = string.Join(", ", (bills.Where(item => item.Status == CommonEnums.StatusBill.ChuaDienThongTin).Select(item => new { item.Room.Name }).ToList()).Select(item => item.Name));

            //hợp đồng sắp hết hạn
            var contractExpire = occupiedRoomIds.Where(item => (item.EndDate.Month - DateTime.Now.Month) <= 1).Select(item => item.RoomId).ToList();
            var roomExpire = _Context.Rooms
                .Where(room => contractExpire.Contains(room.Id) && room.TowerId == towerId).Select(item => new
                {
                    name = item.Name,
                }).ToList();
            return new GetInfomationHomeResModel
            {
                RoomUnpaid = RoomUnpaid.Count,
                InfoRoomUnpaid = string.Join(", ", RoomUnpaid.Select(item => item.Name)),
                RoomPaid = RoomPaid.Count,
                InfoRoomPaid = string.Join(", ", RoomPaid.Select(item => item.Name)),
                RoomNoInvoice = bills.Count - RoomUnpaid.Count - RoomPaid.Count,
                InfoRoomNoInvoice = InfoRoomNoInvoice,
                RoomAvailable = RoomAvailable,
                InfoRoomAvailable = InfoRoomAvailable,
                RoomExpireContract = roomExpire.Count(),
                InfoRoomExpireContract = string.Join(", ", roomExpire.Select(item => item.name)),
                TotalAmount = bills.Where(item => item.Status == CommonEnums.StatusBill.DaXacNhanThanhToan).Sum(item => item.TotalAmount),

            };
        }
        private static List<string> ConverPathListIMG(List<string> input)
        {
            List<string> result = new List<string>();
            foreach (var item in input) 
            {
                result.Add(CommonFunctions.ConverPathIMG(item));
            }
            return result;
        }
        public List<SearchRoomResModel> SearchRoom(string address,decimal priceForm, decimal priceArrive)
        {
            var room = _Context.Rooms.Include(r => r.Tower)
                .Where(item => item.Tower.Address.Contains(address) && item.PriceRoom >= priceForm && item.PriceRoom <= priceArrive && !item.IsDeleted && item.StatusNewCustomer)
                .Select(r => new SearchRoomResModel
                {
                    TowerName = r.Tower.Name,
                    TowerAddress = r.Tower.Address,
                    Id = r.Id,
                    Device = r.Equipment,
                    Price = r.PriceRoom,
                }).ToList();
            if(room.Count == 0)
            {
                throw new Exception("Không có phòng nào phù hợp với điều kiện của bạn");
            }
            foreach(var item in room)
            {
                item.IMG = ConverPathListIMG(_Context.ImgRooms.Where(r => r.RoomId == item.Id).Select(record => record.Path).ToList());
            }
            return room;
        }
        public GetRoomDetailFindRoomResModel GetRoomDetailFindRoom(long Id)
        {
            var roomData = _Context.Rooms.Include(r => r.Tower)
                .ThenInclude(t => t.Landlord)
                .Where(r => r.Id == Id && !r.IsDeleted && r.StatusNewCustomer)
                .Select(record => new GetRoomDetailFindRoomResModel
                {
                    TowerName = record.Tower.Name,
                    TowerAddress = record.Tower.Address,
                    PriceRoom = record.PriceRoom,
                    Device = record.Equipment,
                    MoTa = record.Note,
                    SDTLandlord = record.Tower.Landlord.PhoneNumber,
                    SDTZalo = record.Tower.Landlord.SDTZalo,
                    LandlordName = record.Tower.Landlord.FullName,
                    LandlordAvata = CommonFunctions.ConverPathIMG(record.Tower.Landlord.PathAvatar),
                    PathImgRoom = ConverPathListIMG(_Context.ImgRooms.Where(item => item.RoomId == record.Id).Select(i => i.Path).ToList()),
                    Service = _Context.Services.Where(item=> item.TowerId == record.TowerId).Select(s=> new GetAllServiceResModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Price = s.UnitPrice,
                        UnitOfCalculation = s.UnitOfCalculation
                    }).ToList()
                }).FirstOrDefault();
            if(roomData == null)
            {
                throw new Exception("Phòng không tồn tại");
            }
            return roomData;
        }
        public async Task SaveRoom(long customerId, long roomId)
        {
            if (!_Context.Customers.Any(item => item.Id == customerId && !item.IsDeleted))
            {
                throw new Exception("Người dùng không tồn tại");
            };
            if(!_Context.Rooms.Any(item=> item.Id == roomId && !item.IsDeleted))
            {
                throw new Exception("Phòng không tồn tại");
            }
            var newSaveRoom = new SaveRoom
            {
                CustomerId = customerId,
                RoomId = roomId
            };
            _Context.SaveRooms.Add(newSaveRoom);
            await _Context.SaveChangesAsync();

        }
        public async Task RegisterCustomer()
        {

        }
    }
}
