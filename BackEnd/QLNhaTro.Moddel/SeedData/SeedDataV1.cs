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
                                Password= "hungksdtqn"
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
                            new Room{Id = 1,TowerId=1,Name="102",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2200000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 2,TowerId=1,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4900000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 3,TowerId=1,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=3200000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 4,TowerId=1,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 5,TowerId=1,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3300000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 6,TowerId=2,Name="102",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3100000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 7,TowerId=2,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 8,TowerId=2,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 9,TowerId=2,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 10,TowerId=2,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4200000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 11,TowerId=3,Name="102",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3600000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 12,TowerId=3,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2000000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 13,TowerId=3,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=3100000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 14,TowerId=3,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=1300000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 15,TowerId=3,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2900000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 16,TowerId=4,Name="102",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3400000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 17,TowerId=4,Name="102",Equipment= "Nóng lạnh,Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3800000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 18,TowerId=4,Name="103",Equipment= "Điều hoà",NoPStaying=2,NumberCountries=0,NumberElectric=0,PriceRoom=2700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 19,TowerId=4,Name="201",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=2300000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 20,TowerId=4,Name="202",Equipment= "Điều hoà",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=3700000,StatusNewCustomer = false,IsDeleted= false},
                            new Room{Id = 21,TowerId=5,Name="102",Equipment= "Nóng lạnh",NoPStaying=3,NumberCountries=0,NumberElectric=0,PriceRoom=4000000,StatusNewCustomer = false,IsDeleted= false},
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
        }
    }
}
