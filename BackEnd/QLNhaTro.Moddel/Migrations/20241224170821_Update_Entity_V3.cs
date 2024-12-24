using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "SharedResidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CustomersId",
                table: "SharedResidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SharedResidents_CustomersId",
                table: "SharedResidents",
                column: "CustomersId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedResidents_Customers_CustomersId",
                table: "SharedResidents",
                column: "CustomersId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedResidents_Customers_CustomersId",
                table: "SharedResidents");

            migrationBuilder.DropIndex(
                name: "IX_SharedResidents_CustomersId",
                table: "SharedResidents");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SharedResidents");

            migrationBuilder.DropColumn(
                name: "CustomersId",
                table: "SharedResidents");
        }
    }
}
