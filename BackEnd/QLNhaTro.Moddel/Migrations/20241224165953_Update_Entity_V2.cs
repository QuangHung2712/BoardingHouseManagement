using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Moddel.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaveRooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveRooms_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaveRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedResidents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PriceRoom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Describe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedResidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewRoomPhotos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewRoomPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewRoomPhotos_SharedResidents_NewId",
                        column: x => x.NewId,
                        principalTable: "SharedResidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaveNews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveNews_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaveNews_SharedResidents_NewId",
                        column: x => x.NewId,
                        principalTable: "SharedResidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewRoomPhotos_NewId",
                table: "NewRoomPhotos",
                column: "NewId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveNews_CustomerId",
                table: "SaveNews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveNews_NewId",
                table: "SaveNews",
                column: "NewId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveRooms_CustomerId",
                table: "SaveRooms",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveRooms_RoomId",
                table: "SaveRooms",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewRoomPhotos");

            migrationBuilder.DropTable(
                name: "SaveNews");

            migrationBuilder.DropTable(
                name: "SaveRooms");

            migrationBuilder.DropTable(
                name: "SharedResidents");
        }
    }
}
