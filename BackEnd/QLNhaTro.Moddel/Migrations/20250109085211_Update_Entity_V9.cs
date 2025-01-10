using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_V9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRepresentative",
                table: "Customers");

            migrationBuilder.AddColumn<bool>(
                name: "IsRepresentative",
                table: "ContractCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRepresentative",
                table: "ContractCustomers");

            migrationBuilder.AddColumn<bool>(
                name: "IsRepresentative",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
