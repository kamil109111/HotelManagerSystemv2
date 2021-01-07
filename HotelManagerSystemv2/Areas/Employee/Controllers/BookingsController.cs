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
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagerSystemv2.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            return View();
        }
       

        [HttpPost]
        public IActionResult SearchResult(SearchRoomViewModel vm)
        {
            if (vm.DateFrom == null || vm.DateTo == null || vm.NoOfPeople <= 0 || vm.NoOfPeople == null)
            {
                return View();
            }

            if (vm.DateFrom >= vm.DateTo )
            {
                return View();
            }

            if (vm.NoOfPeople <= 0)
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
        public async Task<IActionResult> Index(string bookingStatus, string paymentStatus, string searchString, string arrivals, string departure)
        {
            ViewData["GetBookingDetails"] = searchString;
            ViewData["Status"] = bookingStatus;
            ViewData["Arrivals"] = arrivals;
            ViewData["Departure"] = departure;

            IQueryable<string> bookingQuery = from m in _context.BookingStatus
                                              select m.BookingStatusName;


            var booking = from b in _context.Booking.Include(b => b.BookingStatus).Include(b => b.Room.RoomType).Include(b => b.Employee).Include(b => b.PaymentStatus)
                          select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                booking = booking.Where(s => s.Name.Contains(searchString) || s.Email.Equals(searchString) || s.Id.ToString().Equals(searchString));
            }

            if (!string.IsNullOrEmpty(bookingStatus))
            {
                booking = booking.Where(x => x.BookingStatus.BookingStatusName == bookingStatus);
            }
            if (!string.IsNullOrEmpty(paymentStatus))
            {
                booking = booking.Where(x => x.PaymentStatus.PaymentStatusName == paymentStatus);
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
                .Include(b => b.PaymentStatus)
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
            var employee = _context.Users.ToList();
            
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
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {

                    FirstDay = bookingvm.Booking.FirstDay,
                    LastDay = bookingvm.Booking.LastDay,
                    ReservationDate = bookingvm.Booking.ReservationDate,
                    Name = bookingvm.Booking.Name,
                    Phone = bookingvm.Booking.Phone,
                    Email = bookingvm.Booking.Email,
                    Dinner = bookingvm.Booking.Dinner,
                    NumberOfPeople = bookingvm.Booking.NumberOfPeople,
                    Deposit = false,
                    AllPaid = false,
                    PaidInAlready = bookingvm.Booking.PaidInAlready,
                    TotalPrice = bookingvm.Booking.TotalPrice,
                    BookingStatusId = 1,
                    PaymentStatusId = 1,
                    EmployeeId = bookingvm.Booking.EmployeeId,
                    RoomId = bookingvm.Booking.RoomId,
                    Note = bookingvm.Booking.Note
                };
                _context.Add(booking);
                _context.SaveChanges();

                return RedirectToAction("SendWelcomeEmail", new RouteValueDictionary(
                new { action = "SendWelcomeEmail", booking.Id }));
            }
            else
            {
                return View(bookingvm);
            }
        }      

        // GET: Admin/Bookings/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                ViewBag.ErrorMessage = $"Rezerwacja o  Id = {id} nie została znaleziona";
                return View("NotFound");
            }

            var bookingStatuses = _context.BookingStatus.ToList();
            var paymentStatuses = _context.PaymentStatus.ToList();
            var employees = _context.Users.ToList();
            var rooms = _context.Room.ToList();

            var viewModel = new BookingViewModel
            {                
                Booking = booking,
                BookingStatuses = bookingStatuses,
                PaymentStatuses = paymentStatuses,
                Rooms = rooms,
                Employees = employees
            };
            

            return View(viewModel);
        }

        // POST: Admin/Bookings/Edit/5        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookingViewModel vm)
        {

            if (ModelState.IsValid)
            {
                if (id != vm.Booking.Id)
                {
                    return NotFound();
                }

                if (vm.Booking.BookingStatusId == 4 && vm.Booking.PaymentStatusId == 2)
                {
                    vm.Booking.BookingStatusId = 4;
                }

                else if (vm.Booking.BookingStatusId == 3 && vm.Booking.PaymentStatusId == 2)
                {
                    vm.Booking.BookingStatusId = 3;
                }

                else if (vm.Booking.PaymentStatusId == 2)
                {
                    vm.Booking.BookingStatusId = 2;
                }

                var numberOfDays = vm.Booking.LastDay.Subtract(vm.Booking.FirstDay).TotalDays;
                var room = await _context.Room.FindAsync(vm.Booking.RoomId);

                if (vm.Booking.Dinner == true)
                {
                    vm.Booking.TotalPrice = (numberOfDays * room.RoomPrice) + ((vm.Booking.NumberOfPeople * 20) * numberOfDays);
                }
                else
                {
                    vm.Booking.TotalPrice = numberOfDays * room.RoomPrice;
                }

                try
                {
                    _context.Update(vm.Booking);
                    await _context.SaveChangesAsync();
                    
                    if (((vm.Booking.PaymentStatusId == 2) && (vm.Booking.Deposit == false)) == true)
                    {

                        vm.Booking.Deposit = true;
                        _context.Update(vm.Booking);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("SendBookingConfirmedEmail", new RouteValueDictionary(
                       new { action = "SendBookingConfirmedEmail", vm.Booking.Id }));
                    }

                    if (((vm.Booking.PaymentStatusId == 3) && (vm.Booking.AllPaid == false)) == true )
                    {
                        vm.Booking.AllPaid = true;
                        _context.Update(vm.Booking);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("SendPaymentConfirmedEmail", new RouteValueDictionary(
                       new { action = "SendBookingPaymentEmail", vm.Booking.Id }));

                    }
                    if (vm.Booking.BookingStatusId == 4)
                    {
                        return RedirectToAction("SendFarewellEmail", new RouteValueDictionary(
                       new { action = "SendFarewellEmail", vm.Booking.Id }));                                            
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(vm.Booking.Id))
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

            var bookingStatuses = _context.BookingStatus.ToList();
            var paymentStatuses = _context.PaymentStatus.ToList();
            var employees = _context.Users.ToList();
            var rooms = _context.Room.ToList();

            vm.BookingStatuses = bookingStatuses;
            vm.PaymentStatuses = paymentStatuses;
            vm.Rooms = rooms;
            vm.Employees = employees;            

            return View(vm);
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

        [HttpGet]
        public async Task<IActionResult> SendWelcomeEmail(int id)
        {

            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                ViewBag.ErrorMessage = $"Booking with Id = {id} cannot be found";
                return View("NotFound");
            }

            var numberOfDays = booking.LastDay.Subtract(booking.FirstDay).TotalDays;
            var room = _context.Room.Single(model => model.RoomId == booking.RoomId);



            string dinner;

            if (booking.Dinner == true)
            {
                dinner = "śniadanie + obiadokolacja";
            }
            else
            {
                dinner = "śniadanie";
            }

            string text = new String("Witaj " + booking.Name + ",\n\n" +
                "Serdecznie dziękujemy za wybór naszego hotelu." +
                " Mamy przyjemność potwierdzić następującą rezerwację: \n\n" +
                "Szczegóły rezerwacji: \n" +
                "\nNumer rezerwacji: " + booking.Id +
                "\nImię i nazwisko: " + booking.Name +
                "\nTwoja rezerwacja: noclegów: " + numberOfDays + ", osób: " + booking.NumberOfPeople +
                "\nPrzyjazd: " + booking.FirstDay.ToShortDateString() + " (od 15:00)" +
                "\nWyjazd: " + booking.LastDay.ToShortDateString() + " (do 11:00)" +
                "\nWyżywienie: " + dinner +
                "\nPokój: " + "Nr." + room.RoomNumber +
                "\nCena za pobyt: " + booking.TotalPrice.ToString("C") +
                "\n\nW celu potwierdzenia rezerwacji proszę o wpłatę bezzwrotnego zadatku w kwocie " + (booking.TotalPrice * 0.4).ToString("C") +
                " w ciągu 6 dni (do " + booking.ReservationDate.AddDays(6).ToShortDateString() + ") na nasze konto." +
                "\n\nDane do wpłaty na konto: 11 1111 1111 1111 1111 1111 1111" +
                "\nTytuł wpłaty: " + booking.Name + " rez. nr: " + booking.Id);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HotelSystemManager", "ttest69777@gmail.com"));
            message.To.Add(new MailboxAddress(booking.Name, booking.Email));
            message.Subject = "HotelSystemManager - Rezerwacja pobytu";

            var builder = new BodyBuilder
            {
                TextBody = text
            };

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("ttest69777@gmail.com", "Qwerty12345!");
                client.Send(message);

                client.Disconnect(true);
            }

            return RedirectToAction(nameof(Success));
        }

        public async Task<IActionResult> SendBookingConfirmedEmail(int id)
        {

            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                ViewBag.ErrorMessage = $"Booking with Id = {id} cannot be found";
                return View("NotFound");
            }

            var numberOfDays = booking.LastDay.Subtract(booking.FirstDay).TotalDays;
            var room = _context.Room.Single(model => model.RoomId == booking.RoomId);



            string dinner;

            if (booking.Dinner == true)
            {
                dinner = "śniadanie + obiadokolacja";
            }
            else
            {
                dinner = "śniadanie";
            }

            string text = new String("Witaj " + booking.Name + ",\n\n" +
                "Otrzymaliśmy wpłatę na poczet Państwa rezerwacji.\n\n" +

                "Wartość rezerwacji: " + booking.TotalPrice.ToString("C") +
                "\nSuma wpłat aktualnie wynosi: " + (booking.TotalPrice * 0.4).ToString("C") +
                "\nDo zapłaty na miejscu: " + (booking.TotalPrice - (booking.TotalPrice * 0.4)).ToString("C") +
                "\nPozostała kwota za pobyt rozliczana jest w dniu przyjazdu." +

                "\n\nSzczegóły rezerwacji: " +
                "\nImię i nazwisko: " + booking.Name +
                "\nTwoja rezerwacja: noclegów: " + numberOfDays + ", osób: " + booking.NumberOfPeople +
                "\nPrzyjazd: " + booking.FirstDay.ToShortDateString() + " (od 15:00)" +
                "\nWyjazd: " + booking.LastDay.ToShortDateString() + " (do 11:00)" +
                "\nWyżywienie: " + dinner +
                "\nPokój: " + "Nr." + room.RoomNumber);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HotelSystemManager", "ttest69777@gmail.com"));
            message.To.Add(new MailboxAddress(booking.Name, booking.Email));
            message.Subject = "HotelSystemManager - Otrzymaliśmy wpłatę";

            var builder = new BodyBuilder
            {
                TextBody = text
            };

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("ttest69777@gmail.com", "Qwerty12345!");
                client.Send(message);

                client.Disconnect(true);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendPaymentConfirmedEmail(int id)
        {

            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                ViewBag.ErrorMessage = $"Booking with Id = {id} cannot be found";
                return View("NotFound");
            }

            var numberOfDays = booking.LastDay.Subtract(booking.FirstDay).TotalDays;
            var room = _context.Room.Single(model => model.RoomId == booking.RoomId);



            string dinner;

            if (booking.Dinner == true)
            {
                dinner = "śniadanie + obiadokolacja";
            }
            else
            {
                dinner = "śniadanie";
            }

            string text = new String("Witaj " + booking.Name + ",\n\n" +
                "Otrzymaliśmy wpłatę na poczet Państwa rezerwacji.\n\n" +

                "Wartość rezerwacji: " + booking.TotalPrice.ToString("C") +
                "\nSuma wpłat aktualnie wynosi: " + (booking.TotalPrice).ToString("C") +
                "\nDo zapłaty na miejscu: " + (booking.TotalPrice - (booking.TotalPrice)).ToString("C") +
                "\nPozostała kwota za pobyt rozliczana jest w dniu przyjazdu." +

                "\n\nSzczegóły rezerwacji: " +
                "\nImię i nazwisko: " + booking.Name +
                "\nTwoja rezerwacja: noclegów: " + numberOfDays + ", osób: " + booking.NumberOfPeople +
                "\nPrzyjazd: " + booking.FirstDay.ToShortDateString() + " (od 15:00)" +
                "\nWyjazd: " + booking.LastDay.ToShortDateString() + " (do 11:00)" +
                "\nWyżywienie: " + dinner +
                "\nPokój: " + "Nr." + room.RoomNumber);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HotelSystemManager", "ttest69777@gmail.com"));
            message.To.Add(new MailboxAddress(booking.Name, booking.Email));
            message.Subject = "HotelSystemManager - Otrzymaliśmy wpłatę";

            var builder = new BodyBuilder
            {
                TextBody = text
            };

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("ttest69777@gmail.com", "Qwerty12345!");
                client.Send(message);

                client.Disconnect(true);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendFarewellEmail(int id)
        {

            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                ViewBag.ErrorMessage = $"Booking with Id = {id} cannot be found";
                return View("NotFound");
            }

            string text = new String("Witaj " + booking.Name + ",\n\n" +
                "Dziękujemy, że wybrałeś nasz hotel. Mamy nadzieję że spełniliśmy twoje oczekiwania. Zapraszamy ponownie w przyszłości \n\n" +
                "Z poważaniem\n" +
                "HotelSystemManager");
                                

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HotelSystemManager", "ttest69777@gmail.com"));
            message.To.Add(new MailboxAddress(booking.Name, booking.Email));
            message.Subject = "HotelSystemManager - Dziękujmy za pobyt";

            var builder = new BodyBuilder
            {
                TextBody = text
            };

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("ttest69777@gmail.com", "Qwerty12345!");
                client.Send(message);

                client.Disconnect(true);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
