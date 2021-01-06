﻿using System;
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
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Routing;

namespace HotelManagerSystemv2.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {            
            _context = context;
        }



        public async Task<IActionResult> IndexAsync()
        {
            var applicationDbContext = _context.Room.Include(r => r.RoomStatus).Include(r => r.RoomType);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail(int id)
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
                Deposit = false,
                AllPaid = false,
                TotalPrice = bookingvm.Booking.TotalPrice,
                BookingStatusId = 1,
                PaymentStatusId = 1,
                EmployeeId = bookingvm.Booking.EmployeeId,
                RoomId = bookingvm.Booking.RoomId
            };
            _context.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("SendEmail", new RouteValueDictionary(
            new { action = "SendEmail", booking.Id }));

           // return RedirectToAction(actionName: nameof(SendEmail(booking.Id) ));
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
