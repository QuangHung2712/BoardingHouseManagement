using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInvoiceDetails_Services_ServicesId",
                table: "ServiceInvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedResidents_Customers_CustomersId",
                table: "SharedResidents");

            migrationBuilder.DropIndex(
                name: "IX_SharedResidents_CustomersId",
                table: "SharedResidents");

            migrationBuilder.DropIndex(
                name: "IX_ServiceInvoiceDetails_ServicesId",
                table: "ServiceInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "CustomersId",
                table: "SharedResidents");

            migrationBuilder.DropColumn(
                name: "ServicesId",
                table: "ServiceInvoiceDetails");

            migrationBuilder.CreateIndex(
                name: "IX_SharedResidents_CustomerId",
                table: "SharedResidents",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInvoiceDetails_ServiceId",
                table: "ServiceInvoiceDetails",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInvoiceDetails_Services_ServiceId",
                table: "ServiceInvoiceDetails",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedResidents_Customers_CustomerId",
                table: "SharedResidents",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInvoiceDetails_Services_ServiceId",
                table: "ServiceInvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedResidents_Customers_CustomerId",
                table: "SharedResidents");

            migrationBuilder.DropIndex(
                name: "IX_SharedResidents_CustomerId",
                table: "SharedResidents");

            migrationBuilder.DropIndex(
                name: "IX_ServiceInvoiceDetails_ServiceId",
                table: "ServiceInvoiceDetails");

            migrationBuilder.AddColumn<long>(
                name: "CustomersId",
                table: "SharedResidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ServicesId",
                table: "ServiceInvoiceDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SharedResidents_CustomersId",
                table: "SharedResidents",
                column: "CustomersId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInvoiceDetails_ServicesId",
                table: "ServiceInvoiceDetails",
                column: "ServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInvoiceDetails_Services_ServicesId",
                table: "ServiceInvoiceDetails",
                column: "ServicesId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedResidents_Customers_CustomersId",
                table: "SharedResidents",
                column: "CustomersId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
