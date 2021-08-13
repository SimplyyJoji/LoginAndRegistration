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
using Microsoft.AspNetCore.Http;

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
            private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            Console.WriteLine(newUser.Email);
            Console.WriteLine(newUser.Password);
            Console.WriteLine(newUser.FirstName);
            Console.WriteLine(ModelState);
                if (ModelState.IsValid)
            {
                // If email already exists.
                if (_context.Users.Any(u => u.Email == newUser.Email))
                {
                    
                    ModelState.AddModelError("Email", "is taken.");
                }
            }

            // If any above custom errors were added, ModelState would now be invalid.

            if (ModelState.IsValid == false)
            {
                Console.WriteLine("Model is false");
                // So error messages are displayed.
                return View("Index");
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("FirstName", newUser.FirstName);
            return View("Index");
        }

        [HttpPost("/Login")]
        public IActionResult Login(LoginUser loginUser)
        {
            if (ModelState.IsValid == false)
            {
                Console.WriteLine(loginUser);
                return View("Index");
            }
        
        User dbUser = _context.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

        if (dbUser == null)
        {
            Console.WriteLine(dbUser);

            ModelState.AddModelError("LoginError", "Email not found");
            return View("Index");
        }

        PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(loginUser,dbUser.Password, loginUser.LoginPassword);

        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LoginPassword", "Invalid Password");
            return View("Index");
        }
            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            HttpContext.Session.SetString("FirstName", dbUser.FirstName);
            return RedirectToAction("Complete");

    }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


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


