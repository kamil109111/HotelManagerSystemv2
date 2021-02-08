using HotelManagerSystemv2.Areas.Employee.Models;
using HotelManagerSystemv2.Data;
using HotelManagerSystemv2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Id = id;
            HttpContext.Session.SetInt32("Id", id);
            var applicationDbContext = _context.Payment.Where(i => i.BookingId == id).Include(p => p.Booking).Include(p => p.Employee);
            return View(await applicationDbContext.ToListAsync());
        }


        public IActionResult Create()
        {

            ViewBag.Id = HttpContext.Session.GetInt32("Id");
            var payment = new Payment
            {
                BookingId = ViewBag.Id,

            };


            return View(payment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Info,Amount,Date,BookingId,EmployeeId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.EmployeeId = _userManager.GetUserId(HttpContext.User);
                _context.Add(payment);
                await _context.SaveChangesAsync();

                var paymentList = _context.Payment.Where(i => i.BookingId == payment.BookingId).ToList();
                var booking = _context.Booking.Single(i => i.Id == payment.BookingId);
                booking.PaidInAlready = paymentList.Sum(i => i.Amount);

                _context.Update(booking);
                await _context.SaveChangesAsync();

                return Redirect("Index/" + payment.BookingId);
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "Id", "Email", payment.BookingId);
            return View(payment);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Id = HttpContext.Session.GetInt32("Id");
            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }


            ViewData["BookingId"] = new SelectList(_context.Booking, "Id", "Email", payment.BookingId);
            return View(payment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Info,Amount,Date,BookingId,EmployeeId")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    payment.EmployeeId = _userManager.GetUserId(HttpContext.User);

                    _context.Update(payment);
                    await _context.SaveChangesAsync();

                    var paymentList = _context.Payment.Where(i => i.BookingId == payment.BookingId).ToList();
                    var booking = _context.Booking.Single(i => i.Id == payment.BookingId);
                    booking.PaidInAlready = paymentList.Sum(i => i.Amount);

                    _context.Update(booking);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Payments", new { id = payment.BookingId });
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "Id", "Email", payment.BookingId);
            return View(payment);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Id = HttpContext.Session.GetInt32("Id");

            var payment = await _context.Payment
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);

            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();

            var paymentList = _context.Payment.Where(i => i.BookingId == payment.BookingId).ToList();
            var booking = _context.Booking.Single(i => i.Id == payment.BookingId);
            booking.PaidInAlready = paymentList.Sum(i => i.Amount);

            _context.Update(booking);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Payments", new { id = payment.BookingId });
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.Id == id);
        }
    }
}
