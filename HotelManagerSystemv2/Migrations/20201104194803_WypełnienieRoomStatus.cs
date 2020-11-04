using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class WypełnienieRoomStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[RoomStatus] ON
                                        INSERT INTO [dbo].[RoomStatus] ([Id], [RoomStatusName]) VALUES (1, N'Dostępny') 
                                        INSERT INTO [dbo].[RoomStatus] ([Id], [RoomStatusName]) VALUES (2, N'Zajęty') 
                                        INSERT INTO [dbo].[RoomStatus] ([Id], [RoomStatusName]) VALUES (3, N'Do posprzątania') 
                                   SET IDENTITY_INSERT [dbo].[RoomStatus] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
