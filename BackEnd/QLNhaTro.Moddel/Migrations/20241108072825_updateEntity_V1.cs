using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    /// <inheritdoc />
    public partial class updateEntity_V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Customer_CustomerId",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Room_RoomId",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Customer_CustomerId",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Room_RoomId",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_ImgRoom_Room_RoomId",
                table: "ImgRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Customer_CustomerId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Tower_TowerId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRoom_Contract_ContractId",
                table: "ServiceRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRoom_Service_ServiceId",
                table: "ServiceRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_Tower_Landlord_LandlordId",
                table: "Tower");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tower",
                table: "Tower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceRoom",
                table: "ServiceRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_CustomerId",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Landlord",
                table: "Landlord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImgRoom",
                table: "ImgRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contract",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_CustomerId",
                table: "Contract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bill",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ImgRoom");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "PriceElectricity",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "PriceWater",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "NewNumber",
                table: "Bill");

            migrationBuilder.RenameTable(
                name: "Tower",
                newName: "Towers");

            migrationBuilder.RenameTable(
                name: "ServiceRoom",
                newName: "ServiceRooms");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "Landlord",
                newName: "Landlords");

            migrationBuilder.RenameTable(
                name: "ImgRoom",
                newName: "ImgRooms");

            migrationBuilder.RenameTable(
                name: "Contract",
                newName: "Contracts");

            migrationBuilder.RenameTable(
                name: "Bill",
                newName: "Bills");

            migrationBuilder.RenameIndex(
                name: "IX_Tower_LandlordId",
                table: "Towers",
                newName: "IX_Towers_LandlordId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRoom_ServiceId",
                table: "ServiceRooms",
                newName: "IX_ServiceRooms_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRoom_ContractId",
                table: "ServiceRooms",
                newName: "IX_ServiceRooms_ContractId");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Rooms",
                newName: "StatusNewCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_Room_TowerId",
                table: "Rooms",
                newName: "IX_Rooms_TowerId");

            migrationBuilder.RenameIndex(
                name: "IX_ImgRoom_RoomId",
                table: "ImgRooms",
                newName: "IX_ImgRooms_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_RoomId",
                table: "Contracts",
                newName: "IX_Contracts_RoomId");

            migrationBuilder.RenameColumn(
                name: "NewCountryNumber",
                table: "Bills",
                newName: "NewWater");

            migrationBuilder.RenameIndex(
                name: "IX_Bill_RoomId",
                table: "Bills",
                newName: "IX_Bills_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Bill_CustomerId",
                table: "Bills",
                newName: "IX_Bills_CustomerId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceRooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<long>(
                name: "NumberElectric",
                table: "Rooms",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Landlords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Landlords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NewElectricity",
                table: "Bills",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Towers",
                table: "Towers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceRooms",
                table: "ServiceRooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Landlords",
                table: "Landlords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImgRooms",
                table: "ImgRooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bills",
                table: "Bills",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incurs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TowerId = table.Column<long>(type: "bigint", nullable: false),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusPay = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incurs_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incurs_Towers_TowerId",
                        column: x => x.TowerId,
                        principalTable: "Towers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TowerId = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Towers_TowerId",
                        column: x => x.TowerId,
                        principalTable: "Towers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ContractId",
                table: "Customers",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Incurs_RoomId",
                table: "Incurs",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Incurs_TowerId",
                table: "Incurs",
                column: "TowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TowerId",
                table: "Services",
                column: "TowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Customers_CustomerId",
                table: "Bills",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Rooms_RoomId",
                table: "Bills",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Rooms_RoomId",
                table: "Contracts",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImgRooms_Rooms_RoomId",
                table: "ImgRooms",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Towers_TowerId",
                table: "Rooms",
                column: "TowerId",
                principalTable: "Towers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRooms_Contracts_ContractId",
                table: "ServiceRooms",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRooms_Services_ServiceId",
                table: "ServiceRooms",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Towers_Landlords_LandlordId",
                table: "Towers",
                column: "LandlordId",
                principalTable: "Landlords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Customers_CustomerId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Rooms_RoomId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Rooms_RoomId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ImgRooms_Rooms_RoomId",
                table: "ImgRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Towers_TowerId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRooms_Contracts_ContractId",
                table: "ServiceRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRooms_Services_ServiceId",
                table: "ServiceRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Towers_Landlords_LandlordId",
                table: "Towers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Incurs");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Towers",
                table: "Towers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceRooms",
                table: "ServiceRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Landlords",
                table: "Landlords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImgRooms",
                table: "ImgRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bills",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceRooms");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "NewElectricity",
                table: "Bills");

            migrationBuilder.RenameTable(
                name: "Towers",
                newName: "Tower");

            migrationBuilder.RenameTable(
                name: "ServiceRooms",
                newName: "ServiceRoom");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "Landlords",
                newName: "Landlord");

            migrationBuilder.RenameTable(
                name: "ImgRooms",
                newName: "ImgRoom");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "Contract");

            migrationBuilder.RenameTable(
                name: "Bills",
                newName: "Bill");

            migrationBuilder.RenameIndex(
                name: "IX_Towers_LandlordId",
                table: "Tower",
                newName: "IX_Tower_LandlordId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRooms_ServiceId",
                table: "ServiceRoom",
                newName: "IX_ServiceRoom_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRooms_ContractId",
                table: "ServiceRoom",
                newName: "IX_ServiceRoom_ContractId");

            migrationBuilder.RenameColumn(
                name: "StatusNewCustomer",
                table: "Room",
                newName: "IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_TowerId",
                table: "Room",
                newName: "IX_Room_TowerId");

            migrationBuilder.RenameIndex(
                name: "IX_ImgRooms_RoomId",
                table: "ImgRoom",
                newName: "IX_ImgRoom_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_RoomId",
                table: "Contract",
                newName: "IX_Contract_RoomId");

            migrationBuilder.RenameColumn(
                name: "NewWater",
                table: "Bill",
                newName: "NewCountryNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_RoomId",
                table: "Bill",
                newName: "IX_Bill_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_CustomerId",
                table: "Bill",
                newName: "IX_Bill_CustomerId");

            migrationBuilder.AlterColumn<long>(
                name: "NumberElectric",
                table: "Room",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Room",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ImgRoom",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Contract",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceElectricity",
                table: "Contract",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceWater",
                table: "Contract",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NewNumber",
                table: "Bill",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tower",
                table: "Tower",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceRoom",
                table: "ServiceRoom",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Landlord",
                table: "Landlord",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImgRoom",
                table: "ImgRoom",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contract",
                table: "Contract",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bill",
                table: "Bill",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Room_CustomerId",
                table: "Room",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CustomerId",
                table: "Contract",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Customer_CustomerId",
                table: "Bill",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Room_RoomId",
                table: "Bill",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Customer_CustomerId",
                table: "Contract",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Room_RoomId",
                table: "Contract",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImgRoom_Room_RoomId",
                table: "ImgRoom",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Customer_CustomerId",
                table: "Room",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Tower_TowerId",
                table: "Room",
                column: "TowerId",
                principalTable: "Tower",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRoom_Contract_ContractId",
                table: "ServiceRoom",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRoom_Service_ServiceId",
                table: "ServiceRoom",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tower_Landlord_LandlordId",
                table: "Tower",
                column: "LandlordId",
                principalTable: "Landlord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
