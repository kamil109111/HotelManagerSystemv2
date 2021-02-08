using HotelManagerSystemv2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HotelManagerSystemv2.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(DateTime day)
        {

            var today = DateTime.Today.Date;

            if (day == default)
            {
                day = today;
            }


            var bookings = _context.Booking.Where(d => d.FirstDay <= day && d.LastDay >= day && (d.BookingStatusId == 3 || d.BookingStatusId == 4)).Include(r => r.Room).ToList();
            var breakfast = bookings.Where(b => b.FirstDay != day).Sum(g => g.NumberOfPeople);
            var dinner = bookings.Where(d => d.Dinner == true && d.LastDay != day).Sum(g => g.NumberOfPeople);




            ViewBag.breakfastList = bookings.Where(b => b.FirstDay != day);
            ViewBag.dinnerList = bookings.Where(d => d.Dinner == true && d.LastDay != day);
            ViewBag.breakfast = breakfast;
            ViewBag.dinner = dinner;
            ViewBag.day = day.ToShortDateString();

            return View(day);
        }
    }
}
