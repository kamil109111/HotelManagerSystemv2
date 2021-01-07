using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class DodanieBookingPaidInAlready : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PaidInAlready",
                table: "Booking",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidInAlready",
                table: "Booking");
        }
    }
}
