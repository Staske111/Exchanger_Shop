using Exchanger_Shop.Methods;
using Exchanger_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> getWallet()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("currency", "BTC");
            nvc.Add("uid", "3120983");//user_id
            nvc.Add("ts", DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            ZixiPayApi zixiPayApi = new ZixiPayApi();
            PayClass pc = await zixiPayApi.CallMethod("getwallet", nvc, "sdklsdhfiebehnf");//api key
            return Ok(new { adress = pc.payload.First().address });
        }
    }
}
