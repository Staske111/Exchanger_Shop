using Exchanger_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Exchanger_Shop.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class BalanceController  : Controller
    {
        private UserContext db;
        public BalanceController(UserContext _db)
        {
            db = _db;
        }
        [Authorize]
        [HttpGet]
        public IActionResult getBalance()
        {
            User CurrUser = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            return Ok(CurrUser);
        }
    }
}
