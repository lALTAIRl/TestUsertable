using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Task4.Data;
using Task4.Helpers;
using Task4.Models;
using Task4.ViewModels;

namespace Task4.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult CreateUser(UserRegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Users.Any(x => x.Email == registerModel.Email))
                {
                    if (registerModel.Password == registerModel.PasswordConfirmation)
                    {
                        _context.Users.Add(new ApplicationUser
                        {
                            Id = new Guid().ToString(),
                            Email = registerModel.Email,
                            UserName = registerModel.UserName,
                            PasswordHash = PasswordHelper.HashPassword(registerModel.Password),
                            RegistrationDate = DateTime.Now,
                            LastLoginDate = DateTime.Now,
                            Status = "Active",
                        });
                        _context.SaveChanges();
                        return Redirect("/Users/UserList");
                    }
                    else
                    {
                        return View("Passwords don't match!");
                    }
                }
                else
                {
                    return View("User with provided Email already exists!");
                }
            }
            else
            {
                return View("All fields required!");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Login(UserLoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(loginModel.Email);
                if (user.PasswordHash == PasswordHelper.HashPassword(loginModel.Password))
                {
                    return View();
                }
                else
                {
                    return View("Wrong password!");
                }
            }
            else
            {
                return View("All fields required!");
            }
            
        }
    }
}
