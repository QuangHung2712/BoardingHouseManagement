using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_V8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserEnterInformation",
                table: "Towers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CurrentNumber",
                table: "ServiceRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PathImg",
                table: "ServiceInvoiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "Landlords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "BillId",
                table: "Incurs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incurs_BillId",
                table: "Incurs",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incurs_Bills_BillId",
                table: "Incurs",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incurs_Bills_BillId",
                table: "Incurs");

            migrationBuilder.DropIndex(
                name: "IX_Incurs_BillId",
                table: "Incurs");

            migrationBuilder.DropColumn(
                name: "UserEnterInformation",
                table: "Towers");

            migrationBuilder.DropColumn(
                name: "CurrentNumber",
                table: "ServiceRooms");

            migrationBuilder.DropColumn(
                name: "PathImg",
                table: "ServiceInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Incurs");
        }
    }
}
