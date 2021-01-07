using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagerSystemv2.Areas.Employee.Models;
using HotelManagerSystemv2.Data;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace HotelManagerSystemv2.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee/Payments
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Id = id;
            HttpContext.Session.SetInt32("Id", id);
            var applicationDbContext = _context.Payment.Where(i => i.BookingId == id).Include(p => p.Booking);
            return View(await applicationDbContext.ToListAsync());
        }
                        

        // GET: Employee/Payments/Create
        public IActionResult Create()
        {

            ViewBag.Id = HttpContext.Session.GetInt32("Id");
            var payment = new Payment
            {
                BookingId = ViewBag.Id
            };


            return View(payment);
        }

        // POST: Employee/Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Info,Amount,BookingId")] Payment payment)
        {
            if (ModelState.IsValid)
            {              

                _context.Add(payment);
                await _context.SaveChangesAsync();

                var paymentList = _context.Payment.Where(i => i.BookingId == payment.BookingId).ToList();
                var booking = _context.Booking.Single(i => i.Id == payment.BookingId);
                booking.PaidInAlready = paymentList.Sum(i => i.Amount);

                _context.Update(booking);
                await _context.SaveChangesAsync();

                return Redirect("Index/"+payment.BookingId);
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "Id", "Email", payment.BookingId);
            return View(payment);
        }

        // GET: Employee/Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "Id", "Email", payment.BookingId);
            return View(payment);
        }

        // POST: Employee/Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Info,Amount,BookingId")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
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
                return Redirect("Index/" + payment.BookingId);
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "Id", "Email", payment.BookingId);
            return View(payment);
        }

        // GET: Employee/Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Employee/Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return Redirect("Index/" + payment.BookingId);
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.Id == id);
        }
    }
}
