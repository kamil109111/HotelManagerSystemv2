using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class DodanieBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FirstDay = table.Column<DateTime>(nullable: false),
                    LastDay = table.Column<DateTime>(nullable: false),
                    ReservationDate = table.Column<DateTime>(nullable: false),
                    Breakfast = table.Column<bool>(nullable: false),
                    Dinner = table.Column<bool>(nullable: false),
                    NumberOfPeople = table.Column<int>(nullable: false),
                    Deposit = table.Column<bool>(nullable: false),
                    AllPaid = table.Column<bool>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false),
                    BookingStatusId = table.Column<int>(nullable: false),
                    GuestId1 = table.Column<string>(nullable: true),
                    GuestId = table.Column<int>(nullable: false),
                    EmployeeId1 = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_BookingStatus_BookingStatusId",
                        column: x => x.BookingStatusId,
                        principalTable: "BookingStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Booking_AspNetUsers_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Booking_AspNetUsers_GuestId1",
                        column: x => x.GuestId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Booking_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BookingStatusId",
                table: "Booking",
                column: "BookingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_EmployeeId1",
                table: "Booking",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_GuestId1",
                table: "Booking",
                column: "GuestId1");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RoomId",
                table: "Booking",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");
        }
    }
}
