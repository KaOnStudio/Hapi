using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hapi.Api.Models;
using Hapi.Data.Abstracts;
using Hapi.Data.Enumerations;
using Hapi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hapi.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            UserModel user;
            if (login.AuthTypes == TokenAuthTypes.AppLogin)
                user = Authenticate(new AppLoginModel
                {
                    Username = login.Username,
                    Password = login.Password
                });
            else if (login.AuthTypes == TokenAuthTypes.AdLogin)
                user = Authenticate(new AdLoginModel
                {
                    Username = login.Username,
                    Password = login.Password,
                    DomainName = login.DomainName
                });
            else
                return BadRequest(new { Message = "Token Türü bulunamadı!" });


            if (user != null)
            {
                var tokenString = BuildToken(user);
                
                response = Ok(new TokenResponse{ AccessToken = tokenString,TokenType = "Bearer"});
            }

            return response;
        }


        private string BuildToken(UserModel user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private UserModel Authenticate(IAppLogin login)
        {
            UserModel user = null;

            if (login.Username == "hasan" && login.Password == "123654")
            {
                user = new UserModel { Name = "Hasan URAL", Email = "metalsimyaci@gmail.com"};
            }
            return user;
        }
        private UserModel Authenticate(IAdLogin login)
        {
            UserModel user = null;

            if (login.Username == "hasan" && login.Password == "123654" && login.DomainName=="metalsimyaci")
            {
                user = new UserModel { Name = "Hasan URAL", Email = "metalsimyaci@gmail.com"};
            }
            return user;
        }
    }
}