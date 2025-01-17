using Microsoft.EntityFrameworkCore;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract = QLNhaTro.Moddel.Entity.Contract;

namespace QLNhaTro.Moddel.SeedData
{
    public partial class SeedDataV1
    {
        public static void SeedData(AppDbContext context)
        {
            if (!context.Landlords.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newLandlord = new List<Landlord>
                        {
                            new Landlord
                            {
                                Id = 1,
                                FullName = "Phạm Quang Hưng",
                                CCCD = "022202001454",
                                Address = "Quảng Ninh",
                                DoB = DateTime.Now,
                                Email = "Quanghungksdtqn@gmail.com",
                                IsActive = true,
                                IsDeleted = false,
                                PhoneNumber = "0359988934",
                                Password= "hungksdtqn",
                                STK = "0080127122002",
                                SampleContractLink = "D:\\Du_An\\BoardingHouseManagement\\Tài liệu\\HopDongMau.docx",
                                PaymentQRImageLink = "D:\\Du_An\\BoardingHouseManagement\\FrontEnd\\public\\images\\UserInformation\\4\\Ảnh QR ngân hàng thanh toán.jpg",
                                SDTZalo = "0359988934",
                                Bank = "MB Bank",
                                PathAvatar = "D:\\Du_An\\BoardingHouseManagement\\FrontEnd\\public\\images\\UserInformation\\1\\Avatar.jpg",
                            }
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Landlords] ON");
                        context.Landlords.AddRange(newLandlord);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Landlords] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.Towers.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newTower = new List<Tower>
                        {
                            new Tower{Id = 1,LandlordId=1,Address="Số 11 Ngõ 91 Đường Cầu Diễn Phường Cầu Diễn Quận Nam Từ Liêm Thành phố Hà Nội", Name= "Số 11 ngõ 91 Cầu Diễm",IsDeleted = false,UserEnterInformation = true},
                            new Tower{Id = 2,LandlordId=1,Address="Số 11 ngõ 91 Cầu Diễn Phường Phú Diễn Quận Bắc Từ Liêm Thành phố Hà Nội", Name= "Số 12 ngõ 92 Cầu Diễm",IsDeleted = false,UserEnterInformation = true},
                            new Tower{Id = 3,LandlordId=1,Address="Số 43 ngõ 179 đường Trâu Quỳ Thị trấn Trâu Quỳ Huyện Gia Lâm Thành phố Hà Nội", Name= "Số 13 ngõ 93 Cầu Diễm",IsDeleted = false, UserEnterInformation = false},
                            new Tower{Id = 4,LandlordId=1,Address="Số 11 ngõ 99 Cầu Diễn Phường Phú Diễn Quận Bắc Từ Liêm Thành phố Hà Nội", Name= "Số 14 ngõ 94 Cầu Diễm",IsDeleted = false, UserEnterInformation = false},
                            new Tower{Id = 5,LandlordId=1,Address="Số 15 ngõ 95 - Cầu Diễm - Nam Từ Liêm - Hà Nội", Name= "Số 15 ngõ 95 Cầu Diễm",IsDeleted = false, UserEnterInformation = false},
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Towers] ON");
                        context.Towers.AddRange(newTower);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Towers] OFF");
                        transaction.Commit();
                    }
                    catch(Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.Rooms.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newRoom = new List<Room>
                        {
                            new Room{Id = 1,TowerId=1,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2200000,StatusNewCustomer = true,IsDeleted= false,},
                            new Room{Id = 2,TowerId=1,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4900000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 3,TowerId=1,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=3200000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 4,TowerId=1,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2700000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 5,TowerId=1,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3300000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 6,TowerId=2,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3100000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 7,TowerId=2,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 8,TowerId=2,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 9,TowerId=2,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3700000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 10,TowerId=2,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4200000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 11,TowerId=3,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3600000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 12,TowerId=3,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 13,TowerId=3,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=3100000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 14,TowerId=3,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=1300000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 15,TowerId=3,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2900000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 16,TowerId=4,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3400000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 17,TowerId=4,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3800000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 18,TowerId=4,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=2700000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 19,TowerId=4,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2300000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 20,TowerId=4,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3700000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 21,TowerId=5,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4000000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 22,TowerId=5,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 23,TowerId=5,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=4900000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 24,TowerId=5,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=1300000,StatusNewCustomer = true,IsDeleted= false},
                            new Room{Id = 25,TowerId=5,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=1700000,StatusNewCustomer = true,IsDeleted= false},

                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Rooms] ON");
                        context.Rooms.AddRange(newRoom);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Rooms] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.Services.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newService = new List<Services>
                        {
                            new Services { Id = 1, TowerId = 1, Name = "Điện", UnitPrice = 3000, IsActive = true, IsDeleted = false,UnitOfCalculation = "KWh" ,IsOldNewNumber = true},
                            new Services { Id = 2, TowerId = 1, Name = "Nước", UnitPrice = 30000, IsActive = true, IsDeleted = false,UnitOfCalculation = "M3",IsOldNewNumber = true },
                            new Services { Id = 3, TowerId = 1, Name = "Máy giặt", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người",IsOldNewNumber = false },
                            new Services { Id = 4, TowerId = 1, Name = "Mạng", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Phòng",IsOldNewNumber = false },
                            new Services { Id = 5, TowerId = 1, Name = "Thang máy", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người",IsOldNewNumber = false },
                            new Services { Id = 6, TowerId = 2, Name = "Điện", UnitPrice = 3500, IsActive = true, IsDeleted = false,UnitOfCalculation = "KWh",IsOldNewNumber = true },
                            new Services { Id = 7, TowerId = 2, Name = "Nước", UnitPrice = 35000, IsActive = true, IsDeleted = false,UnitOfCalculation = "M3",IsOldNewNumber = true },
                            new Services { Id = 8, TowerId = 2, Name = "Máy giặt", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người",IsOldNewNumber = false },
                            new Services { Id = 9, TowerId = 2, Name = "Mạng", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Phòng",IsOldNewNumber = false },
                            new Services { Id = 10, TowerId = 2, Name = "Thang máy", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người",IsOldNewNumber = false },

                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Services] ON");
                        context.Services.AddRange(newService);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Services] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.Contracts.Any())
            {
                DateTime currentDate = DateTime.Now;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var NewContract = new List<Contract>
                        {
                            new Contract {Id = 1, RoomId = 6, StartDate = currentDate.AddMonths(-6), EndDate = currentDate.AddMonths(1),Deposit = 3300000,TerminationDate = null,Note = null, IsDeleted = false,UserEnterInformation = true,},
                            new Contract {Id = 2, RoomId = 7, StartDate = currentDate, EndDate = currentDate.AddMonths(6),Deposit = 3100000,TerminationDate = null,Note = null, IsDeleted = false,UserEnterInformation = true,},
                            new Contract {Id = 3, RoomId = 8, StartDate = currentDate.AddMonths(-2), EndDate = currentDate.AddMonths(6),Deposit = 2000000,TerminationDate = null,Note = null, IsDeleted = false,UserEnterInformation = true},
                            new Contract {Id = 4, RoomId = 9, StartDate = currentDate.AddMonths(-3), EndDate = currentDate.AddMonths(6),Deposit = 2000000,TerminationDate = null,Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 5, RoomId = 10, StartDate = currentDate.AddMonths(-2), EndDate = currentDate.AddMonths(6),Deposit = 3700000,TerminationDate = null,Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 6, RoomId = 6, StartDate = currentDate.AddMonths(-6), EndDate = currentDate.AddMonths(0),Deposit = 3700000,TerminationDate = currentDate.AddMonths(-1),Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 7, RoomId = 7, StartDate = currentDate.AddMonths(-7), EndDate = currentDate.AddMonths(0),Deposit = 3700000,TerminationDate = currentDate.AddMonths(-2),Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 8, RoomId = 8, StartDate = currentDate.AddMonths(-8), EndDate = currentDate.AddMonths(0),Deposit = 3700000,TerminationDate = currentDate.AddMonths(-2),Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 9, RoomId = 9, StartDate = currentDate.AddMonths(-6), EndDate = currentDate.AddMonths(0),Deposit = 3700000,TerminationDate = currentDate.AddMonths(-1),Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 10, RoomId = 10, StartDate = currentDate.AddMonths(-8), EndDate = currentDate.AddMonths(0),Deposit = 3700000,TerminationDate = currentDate.AddMonths(-3),Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 11, RoomId = 1, StartDate = currentDate.AddMonths(-6), EndDate = currentDate.AddMonths(1),Deposit = 3700000,TerminationDate = null,Note = null, IsDeleted = false, UserEnterInformation = true},
                            new Contract {Id = 12, RoomId = 2, StartDate = currentDate.AddMonths(-6), EndDate = currentDate.AddMonths(0),Deposit = 3700000,TerminationDate = null,Note = null, IsDeleted = false, UserEnterInformation = true,},


                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Contracts] ON");
                        context.Contracts.AddRange(NewContract);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Contracts] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.Customers.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var NewCustomer = new List<Customers>
                        {
                            new Customers {Id = 1,FullName = "Phạm Quang Hưng",DoB = new DateTime(2002,12,27), PhoneNumber = "0359988934", CCCD = "022202001454",Address = "Kim Sơn Đông Triều Quảng Ninh",IsDeleted = false,Email = "quanghungksdtqn@gmail.com",SDTZalo = "0359988934",PathAvatar="D:\\Du_An\\BoardingHouseManagement\\FrontEnd\\public\\images\\AvatarDefault.jpg",Password="1"},
                            new Customers {Id = 2,FullName = "Phạm Văn A",DoB = new DateTime(2002,12,27), PhoneNumber = "0363317140", CCCD = "386751689273",Address = "Đường Phan Chu Trinh, Quận 7, Vũng Tàu",IsDeleted = false,Email = "Khachhangso1@gmail.com"},
                            new Customers {Id = 3,FullName = "Phạm Thị Minh Trang",DoB = new DateTime(2002,04,23), PhoneNumber = "0886682304", CCCD = "282721999505",Address = "Găng Thép Thái Nguyên",IsDeleted = false,Email = "phamquanghungksdtqn@gmail.com"},
                            new Customers {Id = 4,FullName = "Phạm Thị B",DoB = new DateTime(2002,12,27), PhoneNumber = "0882320612", CCCD = "445804496623",Address = "Đường Hoàng Diệu, Quận 9, Vũng Tàu",IsDeleted = false,Email = "Khachhangso2@gmail.com"},
                            new Customers {Id = 5,FullName = "Phạm Minh C",DoB = new DateTime(2002,12,27), PhoneNumber = "0372602489", CCCD = "112053382362",Address = "Đường Trần Phú, Quận Cầu Giấy, Vũng Tàu",IsDeleted = false,Email = "Khachhangso3@gmail.com"},
                            new Customers {Id = 6,FullName = "Phạm Văn Thu",DoB = new DateTime(1966,12,27), PhoneNumber = "0333378465", CCCD = "664972286351",Address = "Đường Nguyễn Văn Linh, Quận 1, Quảng Ninh",IsDeleted = false,Email = "vu24593@gmail.com"},
                            new Customers {Id = 7,FullName = "Phạm Thị Bích Hà",DoB = new DateTime(1967,02,14), PhoneNumber = "0342550359", CCCD = "610525008965",Address = "Đường Phan Chu Trinh, Quận 10, Hà Nội",IsDeleted = false,Email = "fbminhtrang@gmail.com"},
                            new Customers {Id = 8,FullName = "Nguyễn Văn D",DoB = new DateTime(2000,02,14), PhoneNumber = "0327786189", CCCD = "800095517479",Address = "Đường Nguyễn Văn Linh, Quận 9, Huế",IsDeleted = false,Email = "Khachhang4@gmail.com"},
                            new Customers {Id = 9,FullName = "Lê Thị E",DoB = new DateTime(2000,5,14), PhoneNumber = "0329391131", CCCD = "373072204337",Address = "Đường Nguyễn Văn Linh, Quận 7, Huế",IsDeleted = false,Email = "Khachhang5@gmail.com"},
                            new Customers {Id = 10,FullName = "Lê Thị G",DoB = new DateTime(2004,8,20), PhoneNumber = "0916124628", CCCD = "047784617208",Address = "Đường Võ Nguyên Giáp, Quận 9, Hà Nội",IsDeleted = false,Email = "Khachhang6@gmail.com"},
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Customers] ON");
                        context.Customers.AddRange(NewCustomer);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Customers] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.ServiceRooms.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var NewServiceRoom = new List<ServiceRoom>
                        {
                            new ServiceRoom {Id = 1,ContractId = 1,ServiceId = 6,Price = 3500,Number = 1,IsOldNewNumber = true,CurrentNumber = 10},
                            new ServiceRoom {Id = 2,ContractId = 1,ServiceId = 7,Price = 35000,Number = 3,IsOldNewNumber = true,CurrentNumber = 1},
                            new ServiceRoom {Id = 3,ContractId = 1,ServiceId = 8,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                            new ServiceRoom {Id = 4,ContractId = 2,ServiceId = 6,Price = 3500,Number = 1,IsOldNewNumber = true,CurrentNumber = 10},
                            new ServiceRoom {Id = 5,ContractId = 2,ServiceId = 7,Price = 35000,Number = 3,IsOldNewNumber = true,CurrentNumber = 10},
                            new ServiceRoom {Id = 6,ContractId = 2,ServiceId = 8,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                            new ServiceRoom {Id = 7,ContractId = 2,ServiceId = 9,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                            new ServiceRoom {Id = 8,ContractId = 3,ServiceId = 6,Price = 3500,Number = 1,IsOldNewNumber = true,CurrentNumber = 10},
                            new ServiceRoom {Id = 9,ContractId = 3,ServiceId = 10,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                            new ServiceRoom {Id = 10,ContractId = 3,ServiceId = 9,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                            new ServiceRoom {Id = 11,ContractId = 4,ServiceId = 6,Price = 3500,Number = 1,IsOldNewNumber = true,CurrentNumber = 5},
                            new ServiceRoom {Id = 12,ContractId = 4,ServiceId = 7,Price = 35000,Number = 1,IsOldNewNumber = true,CurrentNumber = 6},
                            new ServiceRoom {Id = 13,ContractId = 4,ServiceId = 9,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                            new ServiceRoom {Id = 14,ContractId = 5,ServiceId = 10,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                            new ServiceRoom {Id = 15,ContractId = 5,ServiceId = 8,Price = 100000,Number = 3,IsOldNewNumber = false,CurrentNumber = 0},
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ServiceRooms] ON");
                        context.ServiceRooms.AddRange(NewServiceRoom);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ServiceRooms] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.Incurs.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var NewIncur = new List<Incur>
                        {
                            new Incur {Id = 1,TowerId=2,RoomId=6,Amount=100000,CreationDate=new DateTime(2024,12,14),Reason="Tiền phạt",IsDeleted=false,StatusPay=false},
                            new Incur { Id = 2, TowerId = 2, RoomId = 7, Amount = 200000, CreationDate = new DateTime(2024, 11, 15), Reason = "Tiền phạt bẩn", IsDeleted = false, StatusPay = true },
                            new Incur { Id = 3, TowerId = 2, RoomId = 8, Amount = 150000, CreationDate = new DateTime(2024, 12, 16), Reason = "Tiền sửa chữa", IsDeleted = false, StatusPay = false },
                            new Incur { Id = 4, TowerId = 2, RoomId = 9, Amount = 250000, CreationDate = new DateTime(2024, 12, 17), Reason = "Tiền chênh chuyển trọ", IsDeleted = false, StatusPay = false },
                            new Incur { Id = 5, TowerId = 2, RoomId = 10, Amount = 300000, CreationDate = new DateTime(2024, 12, 18), Reason = "Tiền phạt nhà bẩn", IsDeleted = true, StatusPay = true }
                            
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Incurs] ON");
                        context.Incurs.AddRange(NewIncur);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Incurs] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.ContractCustomers.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var NewContractCustomers = new List<ContractCustomer>
                        {
                            new ContractCustomer {Id = 1,ContractId = 1,CustomerId = 1,IsRepresentative = true},
                            new ContractCustomer {Id = 2,ContractId = 1,CustomerId = 2,IsRepresentative = false},
                            new ContractCustomer {Id = 3,ContractId = 2,CustomerId = 3,IsRepresentative = true},
                            new ContractCustomer {Id = 4,ContractId = 2,CustomerId = 4,IsRepresentative = false},
                            new ContractCustomer {Id = 5,ContractId = 2,CustomerId = 5,IsRepresentative = false},
                            new ContractCustomer {Id = 6,ContractId = 3,CustomerId = 6,IsRepresentative = true},
                            new ContractCustomer {Id = 7,ContractId = 4,CustomerId = 7,IsRepresentative = true},
                            new ContractCustomer {Id = 8,ContractId = 4,CustomerId = 8,IsRepresentative = false},
                            new ContractCustomer {Id = 9,ContractId = 5,CustomerId = 9,IsRepresentative = true},
                            new ContractCustomer {Id = 10,ContractId = 5,CustomerId = 10, IsRepresentative = false},
                            new ContractCustomer {Id = 11,ContractId = 6,CustomerId = 1,IsRepresentative = true},
                            new ContractCustomer {Id = 12,ContractId = 6,CustomerId = 2,IsRepresentative = false},
                            new ContractCustomer {Id = 13,ContractId = 7,CustomerId = 3,IsRepresentative = true},
                            new ContractCustomer {Id = 14,ContractId = 7,CustomerId = 4,IsRepresentative = false},
                            new ContractCustomer {Id = 15,ContractId = 7,CustomerId = 5,IsRepresentative = false},
                            new ContractCustomer {Id = 16,ContractId = 8,CustomerId = 6,IsRepresentative = true},
                            new ContractCustomer {Id = 17,ContractId = 9,CustomerId = 7,IsRepresentative = true},
                            new ContractCustomer {Id = 18,ContractId = 9,CustomerId = 8,IsRepresentative = false},
                            new ContractCustomer {Id = 19,ContractId = 10,CustomerId = 9,IsRepresentative = true},
                            new ContractCustomer {Id = 20,ContractId = 10,CustomerId = 10, IsRepresentative = false},
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ContractCustomers] ON");
                        context.ContractCustomers.AddRange(NewContractCustomers);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ContractCustomers] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.Bills.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var NewBills = new List<Bill>
                        {
                           new Bill{Id = 1,CustomerId = 1,RoomId = 6,CreationDate = new DateOnly(2024,12,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,12,15),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 2,CustomerId = 3,RoomId = 7,CreationDate = new DateOnly(2024,12,02),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,12,14),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 3,CustomerId = 6,RoomId = 8,CreationDate = new DateOnly(2024,12,03),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,12,10),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 4,CustomerId = 7,RoomId = 9,CreationDate = new DateOnly(2024,12,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,12,11),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 5,CustomerId = 9,RoomId = 10,CreationDate = new DateOnly(2024,12,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,12,12),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 6,CustomerId = 1,RoomId = 6,CreationDate = new DateOnly(2024,11,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,11,15),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 7,CustomerId = 3,RoomId = 7,CreationDate = new DateOnly(2024,11,02),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,11,14),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 8,CustomerId = 6,RoomId = 8,CreationDate = new DateOnly(2024,11,03),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,11,10),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 9,CustomerId = 7,RoomId = 9,CreationDate = new DateOnly(2024,11,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,11,11),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 10,CustomerId = 9,RoomId = 10,CreationDate = new DateOnly(2024,11,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,11,12),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 11,CustomerId = 1,RoomId = 6,CreationDate = new DateOnly(2024,10,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,10,15),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 12,CustomerId = 3,RoomId = 7,CreationDate = new DateOnly(2024,10,02),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,10,14),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 13,CustomerId = 6,RoomId = 8,CreationDate = new DateOnly(2024,10,03),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,10,10),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 14,CustomerId = 7,RoomId = 9,CreationDate = new DateOnly(2024,10,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,10,11),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 15,CustomerId = 9,RoomId = 10,CreationDate = new DateOnly(2024,10,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,10,12),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 16,CustomerId = 1,RoomId = 6,CreationDate = new DateOnly(2024,9,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,9,15),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 17,CustomerId = 3,RoomId = 7,CreationDate = new DateOnly(2024,9,02),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,9,14),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 18,CustomerId = 6,RoomId = 8,CreationDate = new DateOnly(2024,9,03),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,9,10),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 19,CustomerId = 7,RoomId = 9,CreationDate = new DateOnly(2024,9,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,9,11),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 20,CustomerId = 9,RoomId = 10,CreationDate = new DateOnly(2024,9,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,9,12),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 21,CustomerId = 1,RoomId = 6,CreationDate = new DateOnly(2024,8,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,8,15),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 22,CustomerId = 3,RoomId = 7,CreationDate = new DateOnly(2024,8,02),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,8,14),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 23,CustomerId = 6,RoomId = 8,CreationDate = new DateOnly(2024,8,03),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,8,10),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 24,CustomerId = 7,RoomId = 9,CreationDate = new DateOnly(2024,8,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,8,11),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                           new Bill{Id = 25,CustomerId = 9,RoomId = 10,CreationDate = new DateOnly(2024,8,01),TotalAmount=4015000,PriceRoom=3100000,PaymentDate = new DateTime(2024,8,12),IsDeleted = false,Status = Commons.CommonEnums.StatusBill.DaXacNhanThanhToan},
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Bills] ON");
                        context.Bills.AddRange(NewBills);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Bills] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            if (!context.ServiceInvoiceDetails.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var NewServiceInvoiceDetails = new List<ServiceInvoiceDetails>
                        {
                            new ServiceInvoiceDetails {Id = 1,BillId = 1,ServiceId = 6,NewNumber = 100,OldNumber = 10,UnitPrice = 3500,UsageNumber = 90},
                            new ServiceInvoiceDetails {Id = 2,BillId = 1,ServiceId = 7,NewNumber = 4,OldNumber = 1,UnitPrice = 35000,UsageNumber = 3},
                            new ServiceInvoiceDetails {Id = 3,BillId = 1,ServiceId = 8,NewNumber = 0,OldNumber = null,UnitPrice = 1000000,UsageNumber = 3},
                        };
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ServiceInvoiceDetails] ON");
                        context.ServiceInvoiceDetails.AddRange(NewServiceInvoiceDetails);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ServiceInvoiceDetails] OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
