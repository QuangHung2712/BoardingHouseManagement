using Microsoft.EntityFrameworkCore;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                SampleContractLink = "D:\\Code\\BoardingHouseManagement\\BoardingHouseManagement\\Tài liệu\\HopDongMau.docx",
                                PaymentQRImageLink = "Chưa có"
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
                            new Tower{Id = 1,LandlordId=1,Address="Số 11 ngõ 91 - Cầu Diễm - Nam Từ Liêm - Hà Nội", Name= "Số 11 ngõ 91 Cầu Diễm",IsDeleted = false},
                            new Tower{Id = 2,LandlordId=1,Address="Số 12 ngõ 92 - Cầu Diễm - Nam Từ Liêm - Hà Nội", Name= "Số 12 ngõ 92 Cầu Diễm",IsDeleted = false},
                            new Tower{Id = 3,LandlordId=1,Address="Số 13 ngõ 93 - Cầu Diễm - Nam Từ Liêm - Hà Nội", Name= "Số 13 ngõ 93 Cầu Diễm",IsDeleted = false},
                            new Tower{Id = 4,LandlordId=1,Address="Số 14 ngõ 94 - Cầu Diễm - Nam Từ Liêm - Hà Nội", Name= "Số 14 ngõ 94 Cầu Diễm",IsDeleted = false},
                            new Tower{Id = 5,LandlordId=1,Address="Số 15 ngõ 95 - Cầu Diễm - Nam Từ Liêm - Hà Nội", Name= "Số 15 ngõ 95 Cầu Diễm",IsDeleted = false},
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
                            new Room{Id = 1,TowerId=1,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2200000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 2,TowerId=1,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4900000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 3,TowerId=1,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=3200000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 4,TowerId=1,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 5,TowerId=1,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3300000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 6,TowerId=2,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3100000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 7,TowerId=2,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 8,TowerId=2,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 9,TowerId=2,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 10,TowerId=2,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4200000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 11,TowerId=3,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3600000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 12,TowerId=3,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 13,TowerId=3,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=3100000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 14,TowerId=3,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=1300000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 15,TowerId=3,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2900000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 16,TowerId=4,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3400000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 17,TowerId=4,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3800000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 18,TowerId=4,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=2700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 19,TowerId=4,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2300000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 20,TowerId=4,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 21,TowerId=5,Name="101",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 22,TowerId=5,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 23,TowerId=5,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=4900000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 24,TowerId=5,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=1300000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 25,TowerId=5,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=1700000,StatusNewCustomer = false,IsDeleted= false},

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
                            new Services { Id = 1, TowerId = 1, Name = "Điện", UnitPrice = 3000, IsActive = true, IsDeleted = false,UnitOfCalculation = "KWh" },
                            new Services { Id = 2, TowerId = 1, Name = "Nước", UnitPrice = 30000, IsActive = true, IsDeleted = false,UnitOfCalculation = "M3" },
                            new Services { Id = 3, TowerId = 1, Name = "Máy giặt", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người" },
                            new Services { Id = 4, TowerId = 1, Name = "Mạng", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Phòng" },
                            new Services { Id = 5, TowerId = 1, Name = "Thang máy", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người" },
                            new Services { Id = 6, TowerId = 2, Name = "Điện", UnitPrice = 3500, IsActive = true, IsDeleted = false,UnitOfCalculation = "KWh" },
                            new Services { Id = 7, TowerId = 2, Name = "Nước", UnitPrice = 35000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người" },
                            new Services { Id = 8, TowerId = 2, Name = "Máy giặt", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người" },
                            new Services { Id = 9, TowerId = 2, Name = "Mạng", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Phòng" },
                            new Services { Id = 10, TowerId = 2, Name = "Thang máy", UnitPrice = 100000, IsActive = true, IsDeleted = false,UnitOfCalculation = "Người" },

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
                            new Contract {Id = 1, RoomId = 6, StartDate = currentDate.AddMonths(-1), EndDate = currentDate.AddMonths(6),Deposit = 3300000,TerminationDate = null,Note = null, IsDeleted = false},
                            new Contract {Id = 2, RoomId = 7, StartDate = currentDate, EndDate = currentDate.AddMonths(6),Deposit = 3100000,TerminationDate = null,Note = null, IsDeleted = false},
                            new Contract {Id = 3, RoomId = 8, StartDate = currentDate.AddMonths(-2), EndDate = currentDate.AddMonths(6),Deposit = 2000000,TerminationDate = null,Note = null, IsDeleted = false},
                            new Contract {Id = 4, RoomId = 9, StartDate = currentDate.AddMonths(-3), EndDate = currentDate.AddMonths(6),Deposit = 2000000,TerminationDate = null,Note = null, IsDeleted = false},
                            new Contract {Id = 5, RoomId = 10, StartDate = currentDate.AddMonths(-2), EndDate = currentDate.AddMonths(6),Deposit = 3700000,TerminationDate = null,Note = null, IsDeleted = false},

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
                            new Customers {Id = 1,FullName = "Phạm Quang Hưng",DoB = new DateTime(2002,12,27), PhoneNumber = "0359988934", CCCD = "022202001454",Address = "Kim Sơn Đông Triều Quảng Ninh",ContractId = 1,IsDeleted = false,Email = "quanghungksdtqn@gmail.com",IsRepresentative = true},
                            new Customers {Id = 2,FullName = "Phạm Văn A",DoB = new DateTime(2002,12,27), PhoneNumber = "0363317140", CCCD = "386751689273",Address = "Đường Phan Chu Trinh, Quận 7, Vũng Tàu",ContractId = 1,IsDeleted = false,Email = "Khachhangso1@gmail.com",IsRepresentative = false},
                            new Customers {Id = 3,FullName = "Phạm Thị Minh Trang",DoB = new DateTime(2002,04,23), PhoneNumber = "0886682304", CCCD = "282721999505",Address = "Găng Thép Thái Nguyên",ContractId = 2,IsDeleted = false,Email = "phamquanghungksdtqn@gmail.com",IsRepresentative = true},
                            new Customers {Id = 4,FullName = "Phạm Thị B",DoB = new DateTime(2002,12,27), PhoneNumber = "0882320612", CCCD = "445804496623",Address = "Đường Hoàng Diệu, Quận 9, Vũng Tàu",ContractId = 2,IsDeleted = false,Email = "Khachhangso2@gmail.com",IsRepresentative = false},
                            new Customers {Id = 5,FullName = "Phạm Minh C",DoB = new DateTime(2002,12,27), PhoneNumber = "0372602489", CCCD = "112053382362",Address = "Đường Trần Phú, Quận Cầu Giấy, Vũng Tàu",ContractId = 2,IsDeleted = false,Email = "Khachhangso3@gmail.com",IsRepresentative = false},
                            new Customers {Id = 6,FullName = "Phạm Văn Thu",DoB = new DateTime(1966,12,27), PhoneNumber = "0333378465", CCCD = "664972286351",Address = "Đường Nguyễn Văn Linh, Quận 1, Quảng Ninh",ContractId = 3,IsDeleted = false,Email = "vu24593@gmail.com",IsRepresentative = true},
                            new Customers {Id = 7,FullName = "Phạm Thị Bích Hà",DoB = new DateTime(1967,02,14), PhoneNumber = "0342550359", CCCD = "610525008965",Address = "Đường Phan Chu Trinh, Quận 10, Hà Nội",ContractId = 4,IsDeleted = false,Email = "fbminhtrang@gmail.com",IsRepresentative = true},
                            new Customers {Id = 8,FullName = "Nguyễn Văn D",DoB = new DateTime(2000,02,14), PhoneNumber = "0327786189", CCCD = "800095517479",Address = "Đường Nguyễn Văn Linh, Quận 9, Huế",ContractId = 4,IsDeleted = false,Email = "Khachhang4@gmail.com",IsRepresentative = false},
                            new Customers {Id = 9,FullName = "Lê Thị E",DoB = new DateTime(2000,5,14), PhoneNumber = "0329391131", CCCD = "373072204337",Address = "Đường Nguyễn Văn Linh, Quận 7, Huế",ContractId = 5,IsDeleted = false,Email = "Khachhang5@gmail.com",IsRepresentative = true},
                            new Customers {Id = 10,FullName = "Lê Thị G",DoB = new DateTime(2004,8,20), PhoneNumber = "0916124628", CCCD = "047784617208",Address = "Đường Võ Nguyên Giáp, Quận 9, Hà Nội",ContractId = 5,IsDeleted = false,Email = "Khachhang6@gmail.com",IsRepresentative = false},
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
                            new ServiceRoom {Id = 1,ContractId = 1,ServiceId = 6,Price = 3500,IsDeleted = false,Number = null},
                            new ServiceRoom {Id = 2,ContractId = 1,ServiceId = 7,Price = 35000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 3,ContractId = 1,ServiceId = 8,Price = 100000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 4,ContractId = 2,ServiceId = 6,Price = 3500,IsDeleted = false,Number = null},
                            new ServiceRoom {Id = 5,ContractId = 2,ServiceId = 7,Price = 35000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 6,ContractId = 2,ServiceId = 8,Price = 100000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 7,ContractId = 2,ServiceId = 9,Price = 100000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 8,ContractId = 3,ServiceId = 6,Price = 3500,IsDeleted = false,Number = null},
                            new ServiceRoom {Id = 9,ContractId = 3,ServiceId = 10,Price = 100000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 10,ContractId = 3,ServiceId = 9,Price = 100000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 11,ContractId = 4,ServiceId = 6,Price = 3500,IsDeleted = false,Number = null},
                            new ServiceRoom {Id = 12,ContractId = 4,ServiceId = 7,Price = 35000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 13,ContractId = 4,ServiceId = 9,Price = 100000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 14,ContractId = 5,ServiceId = 10,Price = 100000,IsDeleted = false,Number = 3},
                            new ServiceRoom {Id = 15,ContractId = 5,ServiceId = 8,Price = 100000,IsDeleted = false,Number = 3},
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
        }
    }
}
