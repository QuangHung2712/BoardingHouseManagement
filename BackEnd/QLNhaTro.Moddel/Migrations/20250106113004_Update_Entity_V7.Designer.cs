﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QLNhaTro.Moddel;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250106113004_Update_Entity_V7")]
    partial class Update_Entity_V7
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Bill", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("CreationDate")
                        .HasColumnType("date");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PriceRoom")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("RoomId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Contract", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TerminationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("UserEnterInformation")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ContractCustomer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ContractId")
                        .HasColumnType("bigint");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("CustomerId");

                    b.ToTable("ContractCustomers");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Customers", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DoB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRepresentative")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathAvatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SDTZalo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ImgRoom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("ImgRooms");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Incur", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<bool>("StatusPay")
                        .HasColumnType("bit");

                    b.Property<long>("TowerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("TowerId");

                    b.ToTable("Incurs");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Landlord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DoB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathAvatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentQRImageLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SDTZalo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("STK")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SampleContractLink")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Landlords");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.NewRoomPhoto", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("NewId")
                        .HasColumnType("bigint");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NewId");

                    b.ToTable("NewRoomPhotos");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("BillId")
                        .HasColumnType("bigint");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("LandlordId")
                        .HasColumnType("bigint");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<bool>("ReadStatus")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BillId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("LandlordId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Room", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Equipment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoPStaying")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("NumberCountries")
                        .HasColumnType("bigint");

                    b.Property<long?>("NumberElectric")
                        .HasColumnType("bigint");

                    b.Property<decimal>("PriceRoom")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("StatusNewCustomer")
                        .HasColumnType("bit");

                    b.Property<long>("TowerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TowerId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.SaveNews", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long>("NewId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("NewId");

                    b.ToTable("SaveNews");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.SaveRoom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("RoomId");

                    b.ToTable("SaveRooms");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ServiceInvoiceDetails", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("BillId")
                        .HasColumnType("bigint");

                    b.Property<long?>("NewNumber")
                        .HasColumnType("bigint");

                    b.Property<long?>("OldNumber")
                        .HasColumnType("bigint");

                    b.Property<long>("ServiceId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("UsageNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BillId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServiceInvoiceDetails");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ServiceRoom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ContractId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsOldNewNumber")
                        .HasColumnType("bit");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("ServiceId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServiceRooms");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Services", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOldNewNumber")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TowerId")
                        .HasColumnType("bigint");

                    b.Property<string>("UnitOfCalculation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("TowerId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.SharedResidents", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("Describe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFound")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PriceRoom")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("SharedResidents");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Tower", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LandlordId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LandlordId");

                    b.ToTable("Towers");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Bill", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Contract", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ContractCustomer", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Contract", "Contract")
                        .WithMany("ContractCustomers")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ImgRoom", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Incur", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.Tower", "Tower")
                        .WithMany()
                        .HasForeignKey("TowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("Tower");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.NewRoomPhoto", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.SharedResidents", "New")
                        .WithMany()
                        .HasForeignKey("NewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("New");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Notification", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Bill", "Bill")
                        .WithMany()
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("QLNhaTro.Moddel.Entity.Landlord", "Landlord")
                        .WithMany()
                        .HasForeignKey("LandlordId");

                    b.Navigation("Bill");

                    b.Navigation("Customer");

                    b.Navigation("Landlord");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Room", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Tower", "Tower")
                        .WithMany()
                        .HasForeignKey("TowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tower");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.SaveNews", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.SharedResidents", "New")
                        .WithMany()
                        .HasForeignKey("NewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("New");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.SaveRoom", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ServiceInvoiceDetails", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Bill", "Bills")
                        .WithMany()
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.Services", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bills");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.ServiceRoom", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Contract", "Contract")
                        .WithMany("ServiceMotels")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLNhaTro.Moddel.Entity.Services", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Services", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Tower", "Tower")
                        .WithMany()
                        .HasForeignKey("TowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tower");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.SharedResidents", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Tower", b =>
                {
                    b.HasOne("QLNhaTro.Moddel.Entity.Landlord", "Landlord")
                        .WithMany()
                        .HasForeignKey("LandlordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Landlord");
                });

            modelBuilder.Entity("QLNhaTro.Moddel.Entity.Contract", b =>
                {
                    b.Navigation("ContractCustomers");

                    b.Navigation("ServiceMotels");
                });
#pragma warning restore 612, 618
        }
    }
}
