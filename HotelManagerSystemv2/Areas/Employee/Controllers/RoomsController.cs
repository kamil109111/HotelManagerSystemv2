using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagerSystemv2.Areas.Admin.ViewModel;
using HotelManagerSystemv2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagerSystemv2.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public RoomsController(ApplicationDbContext context)
        {
            _context = context;

        }
        

        public async Task<IActionResult> Index(string roomStatus)
        {            

            IQueryable<string> roomQuery = from m in _context.RoomStatus
                                              select m.RoomStatusName;


            var room = from b in _context.Room.Include(b => b.RoomStatus)
                          select b;
                      

            if (!string.IsNullOrEmpty(roomStatus))
            {
                room = room.Where(x => x.RoomStatus.RoomStatusName == roomStatus);
            }            

            return View(await room.AsNoTracking().ToListAsync());
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Room.FindAsync(id);

            if (room == null)
            {
                ViewBag.ErrorMessage = $"Room with Id = {id} cannot be found";
                return View("NotFound");
            }

            var viewModel = new RoomViewModel
            {
                Room = room,
                RoomStatuses = _context.RoomStatus.ToList()
            };   

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RoomViewModel roomvm)
        {
           
            var room = _context.Room.SingleOrDefault(r => r.RoomId == id);

            if (room == null)
            {
                ViewBag.ErrorMessage = $"Room with Id = {room.RoomId} cannot be found";
                return View("NotFound");
            }

            {
                               
                room.RoomStatusId = roomvm.Room.RoomStatusId;
                              
            };
                        
            _context.Update(room);
            await _context.SaveChangesAsync();
                         
            return RedirectToAction(nameof(Index));
                                    
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.RoomStatus)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
    }
}
