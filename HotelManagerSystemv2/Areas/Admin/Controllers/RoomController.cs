using HotelManagerSystemv2.Areas.Admin.Models;
using HotelManagerSystemv2.Areas.Admin.ViewModel;
using HotelManagerSystemv2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RoomsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Rooms
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Room.Include(r => r.RoomStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Rooms/Details/5
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

        // GET: Admin/Rooms/Create
        public IActionResult Create()
        {


            var roomStatuses = _context.RoomStatus.ToList();
            var roomTypes = _context.RoomType.ToList();
            var viewModel = new RoomViewModel
            {
                RoomStatuses = roomStatuses,
                RoomTypes = roomTypes
            };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomViewModel roomvm)
        {
            string stringFilename = UploadFile(roomvm);
            if (UploadFile(roomvm) == null)
            {
                stringFilename = "No-image.png";
            }
            var room = new Room
            {

                RoomNumber = roomvm.Room.RoomNumber,
                RoomCapacity = roomvm.Room.RoomCapacity,
                RoomDescription = roomvm.Room.RoomDescription,
                RoomPrice = roomvm.Room.RoomPrice,
                RoomStatusId = roomvm.Room.RoomStatusId,
                RoomTypeId = roomvm.Room.RoomTypeId,
                RoomImage = stringFilename
            };
            _context.Room.Add(room);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private string UploadFile(RoomViewModel roomvm)
        {
            string fileName = null;
            if (roomvm.RoomImage != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + roomvm.RoomImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                roomvm.RoomImage.CopyTo(fileStream);
            }
            return fileName;
        }

        // GET: Admin/Rooms/Edit/5
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
                RoomStatuses = _context.RoomStatus.ToList(),
                RoomTypes = _context.RoomType.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RoomViewModel roomvm)
        {
            string stringFilename = UploadFile(roomvm);
            var room = _context.Room.SingleOrDefault(r => r.RoomId == id);

            if (room == null)
            {
                ViewBag.ErrorMessage = $"Room with Id = {room.RoomId} cannot be found";
                return View("NotFound");
            }

            {
                room.RoomNumber = roomvm.Room.RoomNumber;
                room.RoomCapacity = roomvm.Room.RoomCapacity;
                room.RoomDescription = roomvm.Room.RoomDescription;
                room.RoomPrice = roomvm.Room.RoomPrice;
                room.RoomStatusId = roomvm.Room.RoomStatusId;
                room.RoomTypeId = roomvm.Room.RoomTypeId;
                if (stringFilename != null)
                {
                    room.RoomImage = stringFilename;
                }
            };


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(roomvm.Room.RoomId))
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

            return View(roomvm);
        }



        // GET: Admin/Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }
    }
}
