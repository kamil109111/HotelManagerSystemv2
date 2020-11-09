using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagerSystemv2.Migrations
{
    public partial class WypełnienieRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[RoomStatus] ON
           
            INSERT INTO[dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Address], [Photo], [FirstNameLastName]) VALUES(N'e39af82e-30d7-4a1b-affb-01ce12fbb651', N'admin@example.com', N'ADMIN@EXAMPLE.COM', N'admin@example.com', N'ADMIN@EXAMPLE.COM', 0, N'AQAAAAEAACcQAAAAEFSoZ211O4jgA5cXWGB7ZwzS4mvjW1aeeXMimiVflmYRIXZtCWHCNJUBOaR3PO6z+w==', N'G3BIINWVHTJB2XKGBOOGY2XEJFSBQVHC', N'3be68374-1999-49f0-b81a-464fe0e117ec', NULL, 0, 0, NULL, 1, 0, NULL, NULL, N'Default Admin')
            
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3b9d54fa-2938-4f2b-a563-86145fbea7bd', N'Guests', N'GUESTS', N'9c8257d0-f97c-4eff-8465-85726df1fd32')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3c74cf30-faf9-421c-98da-408d98ce93da', N'User', N'USER', N'9e503416-3855-4250-9b5e-f0c81f1450f5')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3fbad300-5d7c-454d-b569-8b7a5fc998a1', N'Administrator', N'ADMINISTRATOR', N'7c1af768-1585-4764-91fe-5bca37e320a9')

            INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e39af82e-30d7-4a1b-affb-01ce12fbb651', N'3fbad300-5d7c-454d-b569-8b7a5fc998a1')
            
            SET IDENTITY_INSERT [dbo].[RoomStatus] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
