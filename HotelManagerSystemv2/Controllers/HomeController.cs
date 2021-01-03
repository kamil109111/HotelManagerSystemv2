using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HotelManagerSystemv2.Models;
using HotelManagerSystemv2.Data;
using Microsoft.EntityFrameworkCore;
using HotelManagerSystemv2.Areas.Employee.ViewModel;
using HotelManagerSystemv2.Areas.Employee.Models;

namespace HotelManagerSystemv2.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {            
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> Offer()
        {
            var applicationDbContext = _context.Room.Include(r => r.RoomStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public IActionResult SearchOffer(SearchRoomViewModel vm)
        {
            if (vm.DateFrom == null || vm.DateTo == null)
            {
                return View();
            }

            if (vm.DateFrom >= vm.DateTo)
            {
                return View();
            }

            var roomsBooked = from b in _context.Booking
                              where
                              ((vm.DateFrom >= b.FirstDay) && (vm.DateFrom <= b.LastDay)) ||
                              ((vm.DateTo >= b.FirstDay) && (vm.DateTo <= b.LastDay)) ||
                              ((vm.DateFrom <= b.LastDay) && (vm.DateTo >= b.LastDay) && (vm.DateTo <= b.LastDay)) ||
                              ((vm.DateFrom >= b.LastDay) && (vm.DateFrom <= b.LastDay) && (vm.DateTo >= b.LastDay)) ||
                              ((vm.DateFrom <= b.LastDay) && (vm.DateTo >= b.LastDay))
                              select b;

            var availableRooms = _context.Room.Where(r => !roomsBooked.Any(b => b.RoomId == r.RoomId))
                .Include(x => x.RoomType).ToList();

            foreach (var item in availableRooms)
            {
                if (item.RoomCapacity >= vm.NoOfPeople)
                {
                    vm.Room.Add(item);
                }
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult CreateBooking(int id, DateTime DateFrom, DateTime DateTo, int NoOfPeople, bool Dinner)
        {

            var Room = _context.Room.Single(model => model.RoomId == id);
            var RoomPrice = Room.RoomPrice;
            var numberOfDays = DateTo.Subtract(DateFrom).TotalDays;


            Booking booking = new Booking
            {
                FirstDay = DateFrom,
                LastDay = DateTo,
                NumberOfPeople = NoOfPeople,
                RoomId = id,
                Dinner = Dinner
            };




            if (Dinner == true)
            {
                booking.TotalPrice = (numberOfDays * RoomPrice) + ((NoOfPeople * 20) * numberOfDays);
            }
            else
            {
                booking.TotalPrice = numberOfDays * RoomPrice;
            }



            var bookingStatuses = _context.BookingStatus.ToList();
            var employee = _context.Users.Where(i => i.IsGuest == false).ToList();
            var guest = _context.Users.Where(i => i.IsGuest == true).ToList();
            var viewModel = new BookingViewModel
            {
                BookingStatuses = bookingStatuses,
                Employees = employee,
                Booking = booking
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBooking(BookingViewModel bookingvm)
        {
            var booking = new Booking
            {
                //Id = 1 + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year ,
                FirstDay = bookingvm.Booking.FirstDay,
                LastDay = bookingvm.Booking.LastDay,
                ReservationDate = bookingvm.Booking.ReservationDate,
                Name = bookingvm.Booking.Name,
                Phone = bookingvm.Booking.Phone,
                Email = bookingvm.Booking.Email,
                Dinner = bookingvm.Booking.Dinner,
                NumberOfPeople = bookingvm.Booking.NumberOfPeople,
                Deposit = bookingvm.Booking.Deposit,
                AllPaid = bookingvm.Booking.AllPaid,
                TotalPrice = bookingvm.Booking.TotalPrice,
                BookingStatusId = 1,
                PaymentStatusId = 1,
                EmployeeId = bookingvm.Booking.EmployeeId,
                RoomId = bookingvm.Booking.RoomId
            };
            _context.Add(booking);
            _context.SaveChanges();

            return RedirectToAction(nameof(Success));
        }

        /*
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */

        // GET: Admin/Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.RoomStatus)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
    }
}
