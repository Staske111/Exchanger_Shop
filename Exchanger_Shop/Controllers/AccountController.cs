using Exchanger_Shop.Models;
using Exchanger_Shop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Exchanger_Shop.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext _db)
        {
            db = _db;
        }
        [HttpPost("/register")]
        public async Task<IActionResult> Registration(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
                if (user == null)
                {
                    db.Users.Add(new User { Email = model.Email, Password = model.Password, Balance = 0 });
                    await db.SaveChangesAsync();
                    Resp response = await Authenticate(model.Email, model.Password);
                    return Json(response);

                    //получить токен и вернуть Json
                }
                //добавить вывод ошибок
                return BadRequest(new { errorText = "Пользователь уже зарегестрирован" });
            }
            //брать ошибки из моделстейт
            //добавить проверку почты и пароля
            return BadRequest(new { errorText = "Некоректные логин или пароль" });
        }
        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Resp response = await Authenticate(model.Email, model.Password);
                if(response != null) return Json(response);
                return BadRequest(new { errorText = "Неправильный1 логин или пароль." });
            }
            return BadRequest(new { errorText = "Неправильный логин или пароль." });
        }
        private async Task<Resp> Authenticate(string email, string password)//применить авторизацию
        {
            ClaimsIdentity claimsIdentity = await GetIdentity(email, password);
            if (claimsIdentity != null)
            {
                DateTime now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claimsIdentity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return await Task.Run(() => new Resp
                {
                    access_token = encodedJwt,
                    username = claimsIdentity.Name
                });
            }
            return null;
        }
        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Email == username && x.Password == password);
            if(user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
                };
                return await Task.Run(() => new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType));
            }
            return null;
        }
    }
        public class Resp
        {
            public string access_token { get; set; }
            public string username { get; set; }
        }
}
