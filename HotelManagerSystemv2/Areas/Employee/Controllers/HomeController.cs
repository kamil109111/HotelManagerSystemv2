using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystemManagerv2.Areas.Employee.Controllers
{ 

    [Area("Employee")]
    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
