using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Task4.Data;

namespace Task4.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult UserList()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Block(string id)
        {
            var user = _context.Users.Find(id);

            user.Status = "Blocked";

            _context.SaveChanges();

            return RedirectPermanent("UserList");
        }

        public IActionResult Unblock(string id)
        {
            var user = _context.Users.Find(id);

            user.Status = "Active";

            _context.SaveChanges();

            return RedirectPermanent("UserList");
        }

        public IActionResult Delete(string id)
        {
            var user = _context.Users.Find(id);

            _context.Users.Remove(user);

            _context.SaveChanges();

            return RedirectPermanent("UserList");
        }
    }
}
