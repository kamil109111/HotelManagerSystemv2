using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class DodanieEmployeeDoPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Payment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_EmployeeId",
                table: "Payment",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_AspNetUsers_EmployeeId",
                table: "Payment",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_AspNetUsers_EmployeeId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_EmployeeId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Payment");
        }
    }
}
