using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using login_registration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace login_registration.Controllers
{
    public class HomeController : Controller
    {
        private YourContext dbContext;
    // here we can "inject" our context service into the constructor
        public HomeController(YourContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet("success")]
        
        public IActionResult successpage()
        {
            if (HttpContext.Session.GetInt32("logged_in_userID") == null)
            {
                return RedirectToAction("Index");
            }
            return View("Success");
        }

        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

        [HttpPost("login")]
        public IActionResult login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                var userindb = dbContext.users.FirstOrDefault(u => u.email == user.email);

                if(userindb == null)
                {
                    ModelState.AddModelError("email", "Invalid Email/Password");
                    return View("Login");
                }

                var hasher = new PasswordHasher<LoginUser>();

                var result = hasher.VerifyHashedPassword(user, userindb.password, user.password);

                if(result == 0)
                {
                    ModelState.AddModelError("password", "Invalid Email/Password");
                    return View("Login");
                }

                // int logged_in_user_id = (int)userindb.id;
                HttpContext.Session.SetInt32("logged_in_userID", userindb.id);
                int? logged_in_user = HttpContext.Session.GetInt32("logged_in_userID");
                System.Console.WriteLine(logged_in_user);

            return RedirectToAction("successpage");

            }
            else
            {
                return View("Login");
            }
        }

        [HttpPost("register")]
        public IActionResult register(User user)
        {
            System.Console.WriteLine("STEPONEEEEEEEE");
            if(ModelState.IsValid)
            {
                if(dbContext.users.Any(u => u.email == user.email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                System.Console.WriteLine("ISVAAALLLIIDDD");
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.password = Hasher.HashPassword(user, user.password);
                User Newuser = new User
                {
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    password = user.password,
                };
                System.Console.WriteLine("WORKINGGGGGGG");
                dbContext.Add(Newuser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("logged_in_userID", Newuser.id);
                int? logged_in_user = HttpContext.Session.GetInt32("logged_in_userID");
                return RedirectToAction("successpage");
            }
            else
            {
                System.Console.WriteLine("NOTWORKING");
                return View("Index");
            }
        }
    }
}
