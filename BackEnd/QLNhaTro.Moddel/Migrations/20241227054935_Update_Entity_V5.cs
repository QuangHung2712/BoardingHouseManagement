using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ServiceRooms",
                newName: "IsOldNewNumber");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "ServiceRooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PathAvatar",
                table: "Landlords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PathAvatar",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserEnterInformation",
                table: "Contracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bills",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceRoom",
                table: "Bills",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathAvatar",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "PathAvatar",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UserEnterInformation",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PriceRoom",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "IsOldNewNumber",
                table: "ServiceRooms",
                newName: "IsDeleted");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "ServiceRooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Bills",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
