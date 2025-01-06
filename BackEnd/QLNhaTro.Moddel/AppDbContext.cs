using Microsoft.EntityFrameworkCore;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<ImgRoom> ImgRooms { get; set; }
        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<ServiceRoom> ServiceRooms { get; set; }
        public DbSet<Tower> Towers { get; set; }
        public DbSet<Incur> Incurs { get; set; }
        public DbSet<ServiceInvoiceDetails> ServiceInvoiceDetails { get; set; }
        public DbSet<NewRoomPhoto> NewRoomPhotos { get; set; }
        public DbSet<SaveNews> SaveNews { get; set; }
        public DbSet<SaveRoom> SaveRooms { get; set; }
        public DbSet<SharedResidents> SharedResidents { get; set; }
        public DbSet<ContractCustomer> ContractCustomers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
