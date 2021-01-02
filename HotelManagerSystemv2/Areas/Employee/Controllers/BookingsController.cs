using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagerSystemv2.Areas.Employee.Models;
using HotelManagerSystemv2.Data;
using HotelManagerSystemv2.Areas.Employee.ViewModel;

namespace HotelManagerSystemv2.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

       

        [HttpGet]
        public IActionResult SearchResult(SearchRoomViewModel vm)
        {
            if (vm.DateFrom == null || vm.DateTo == null)
            {
                return View();
            }

            if (vm.DateFrom >= vm.DateTo )
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

        // GET: Admin/Bookings 
        public async Task<IActionResult> Index(string bookingStatus, string searchString, string arrivals, string departure)
        {
            ViewData["GetBookingDetails"] = searchString;
            ViewData["Status"] = bookingStatus;
            ViewData["Arrivals"] = arrivals;
            ViewData["Departure"] = departure;

            IQueryable<string> bookingQuery = from m in _context.BookingStatus
                                              select m.BookingStatusName;


            var booking = from b in _context.Booking.Include(b => b.BookingStatus).Include(b => b.Room.RoomType).Include(b => b.Employee)
                          select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                booking = booking.Where(s => s.Name.Contains(searchString) || s.Email.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(bookingStatus))
            {
                booking = booking.Where(x => x.BookingStatus.BookingStatusName == bookingStatus);
            }

            if (!string.IsNullOrEmpty(arrivals))
            {
                booking = booking.Where(x => x.FirstDay.Date.ToString() == arrivals);
            }

            if (!string.IsNullOrEmpty(departure))
            {
                booking = booking.Where(x => x.LastDay.Date.ToString() == departure);
            }

            return View(await booking.AsNoTracking().ToListAsync());
        }       


        // GET: Admin/Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.BookingStatus)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }             

        [HttpGet]
        public IActionResult Create(int id, DateTime DateFrom, DateTime DateTo, int NoOfPeople, bool Dinner)
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
                booking.TotalPrice = (numberOfDays * RoomPrice) + ((NoOfPeople*20)*numberOfDays); 
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
        public IActionResult Create(BookingViewModel bookingvm)
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
                BookingStatusId = bookingvm.Booking.BookingStatusId,                
                EmployeeId = bookingvm.Booking.EmployeeId,
                RoomId = bookingvm.Booking.RoomId
            };
            _context.Add(booking);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        /*/ GET: Admin/Bookings/Create
        public IActionResult Create()
        {
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatus, "Id", "Id");
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId");
            return View();
        }

        // POST: Admin/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstDay,LastDay,ReservationDate,Breakfast,Dinner,NumberOfPeople,Deposit,AllPaid,TotalPrice,BookingStatusId,GuestId,EmployeeId,RoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatus, "Id", "Id", booking.BookingStatus);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", booking.);
            return View(booking);
        }*/

        // GET: Admin/Bookings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                ViewBag.ErrorMessage = $"Room with Id = {id} cannot be found";
                return View("NotFound");
            }
           
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatus, "Id", "Id", booking.BookingStatusId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }

        // POST: Admin/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,FirstDay,LastDay,ReservationDate,Name,Phone,Email,Dinner,NumberOfPeople,Deposit,AllPaid,TotalPrice,BookingStatusId,EmployeeId,RoomId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingStatusId"] = new SelectList(_context.BookingStatus, "Id", "Id", booking.BookingStatusId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }

        // GET: Admin/Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.BookingStatus)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Admin/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }
    }
}
