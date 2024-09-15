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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<ImgRoom> ImgRoom { get; set; }
        public DbSet<Landlord> Landlord { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceRoom> ServiceRoom { get; set; }
        public DbSet<Tower> Tower { get; set; }
    }
}
