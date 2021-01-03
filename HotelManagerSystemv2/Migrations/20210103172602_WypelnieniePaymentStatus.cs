using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class WypelnieniePaymentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[PaymentStatus] ON
                                        INSERT INTO [dbo].[PaymentStatus] ([Id], [PaymentStatusName]) VALUES (1, N'Nieopłacono') 
                                        INSERT INTO [dbo].[PaymentStatus] ([Id], [PaymentStatusName]) VALUES (2, N'Opłacono zaliczkę') 
                                        INSERT INTO [dbo].[PaymentStatus] ([Id], [PaymentStatusName]) VALUES (3, N'Opłacono')                                        
                                   SET IDENTITY_INSERT [dbo].[PaymentStatus] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
