using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelManagerSystemv2.Models;
using HotelManagerSystemv2.Areas.Admin.Models;

namespace HotelManagerSystemv2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HotelManagerSystemv2.Areas.Admin.Models.Room> Room { get; set; }
        public DbSet<HotelManagerSystemv2.Areas.Admin.Models.RoomStatus> RoomStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
     
        }
    }
}
