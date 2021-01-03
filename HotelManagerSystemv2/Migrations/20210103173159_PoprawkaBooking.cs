using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class PoprawkaBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusId",
                table: "Booking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PaymentStatusId",
                table: "Booking",
                column: "PaymentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_PaymentStatus_PaymentStatusId",
                table: "Booking",
                column: "PaymentStatusId",
                principalTable: "PaymentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_PaymentStatus_PaymentStatusId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_PaymentStatusId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "PaymentStatusId",
                table: "Booking");
        }
    }
}
