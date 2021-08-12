using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoginAndReg.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginAndReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ForumContext _context;
        public HomeController(ForumContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index(User user)
        {
            if(ModelState.IsValid)
            {
                // Initializing a PasswordHasher object, providing our User class as its type
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                //Save your user object to the database
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult OtherIndex()
        {
            return View();
        }

        // [HttpGet("Login")]
    //     public IActionResult Login(LoginUser userSubmission)
    // {
    //     if(ModelState.IsValid)
    //     {
    //         // If inital ModelState is valid, query for a user with provided email
    //         var userInDb = _context.User.FirstOrDefault(u => u.Email == userSubmission.Email);
    //         // If no user exists with provided email
    //         if(userInDb == null)
    //         {
    //             // Add an error to ModelState and return to View!
    //             ModelState.AddModelError("Email", "Invalid Email/Password");
    //             return View("SomeView");
    //         }
            
    //         // Initialize hasher object
    //         var hasher = new PasswordHasher<LoginUser>();
            
    //         // verify provided password against hash stored in db
    //         var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
            
    //         // result can be compared to 0 for failure
    //         if(result == 0)
    //         {
    //             // handle failure (this should be similar to how "existing email" is handled)
    //         }
    //     }
    //     return View("Complete");
    // }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
