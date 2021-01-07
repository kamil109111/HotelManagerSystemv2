using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagerSystemv2.Models;
using HotelManagerSystemv2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagerSystemv2.Controllers
{
    public class AccountController : Controller
    {        
        private readonly SignInManager<ApplicationUser> _signInManager;
        public  AccountController(SignInManager<ApplicationUser> signInManager)
        {            
            _signInManager = signInManager;
        }
         
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {                
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {                    
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                
            }

            return View(model);
        }
       
    }
}
