using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class DodanieRoomTypeDoRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomType",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "RoomType");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoomType",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "AirConditioning",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Balcony",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bath",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bathrobes",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bathroom",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bidet",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Desk",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Fridge",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hairdryer",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Internet",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Iron",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Jacuzzi",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Kitchen",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Kitchenette",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Radio",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Shower",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TV",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Terrace",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Toilet",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Towels",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WashingMachine",
                table: "RoomType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId",
                table: "Room",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomType",
                table: "RoomType",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoomTypeId",
                table: "Room",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room",
                column: "RoomTypeId",
                principalTable: "RoomType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomType",
                table: "RoomType");

            migrationBuilder.DropIndex(
                name: "IX_Room_RoomTypeId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "AirConditioning",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Balcony",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Bath",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Bathrobes",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Bathroom",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Bidet",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Desk",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Fridge",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Hairdryer",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Internet",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Iron",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Jacuzzi",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Kitchen",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Kitchenette",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Radio",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Shower",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "TV",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Terrace",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Toilet",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "Towels",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "WashingMachine",
                table: "RoomType");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "Room");

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId",
                table: "RoomType",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomType",
                table: "RoomType",
                column: "RoomTypeId");
        }
    }
}
