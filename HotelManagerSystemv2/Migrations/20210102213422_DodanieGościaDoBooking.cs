using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class DodanieGościaDoBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_GuestId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_GuestId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Breakfast",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Booking");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Booking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Booking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Booking",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Booking");

            migrationBuilder.AddColumn<bool>(
                name: "Breakfast",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GuestId",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_GuestId",
                table: "Booking",
                column: "GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_GuestId",
                table: "Booking",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
