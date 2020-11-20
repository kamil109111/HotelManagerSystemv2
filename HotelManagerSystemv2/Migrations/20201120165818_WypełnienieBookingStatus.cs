using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class WypełnienieBookingStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[BookingStatus] ON
                                        INSERT INTO [dbo].[BookingStatus] ([Id], [BookingStatusName]) VALUES (1, N'Nowa') 
                                        INSERT INTO [dbo].[BookingStatus] ([Id], [BookingStatusName]) VALUES (2, N'Potwierdzona') 
                                        INSERT INTO [dbo].[BookingStatus] ([Id], [BookingStatusName]) VALUES (3, N'Zameldowany')
                                        INSERT INTO [dbo].[BookingStatus] ([Id], [BookingStatusName]) VALUES (4, N'Wymeldowany') 
                                   SET IDENTITY_INSERT [dbo].[BookingStatus] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
