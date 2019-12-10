using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BooksContext _context;

        public AuthController(BooksContext context)
        {
            _context = context;
        }

        [HttpPost("token")]
        public ActionResult GetToken(Users user)
        {
            //security key
            string securityKey = "this_is_our_supper_long_security_key_for_token_validation_project_2018_09_07$smesk.in";
            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new List<Claim>();
            if (user.role == "Administrator")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                //claims.Add(new Claim("role", "Administrator"));
            }
            else
            {
                //claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim("role", "User"));
            }
            claims.Add(new Claim("Id", user.Id.ToString()));

            //add claims
            //var claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            //claims.Add(new Claim(ClaimTypes.Role, "Reader"));
            //claims.Add(new Claim("Our_Custom_Claim", "Our custom value"));
            //claims.Add(new Claim("Id", "110"));

            //create token
            var token = new JwtSecurityToken(
                    issuer: "smesk.in",
                    audience: "readers",
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCredentials
                    , claims: claims
                );

            //return new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost("login")]
        public ActionResult<string> Login(Login login)
        {
            var users = _context.Users.Where(u => u.username == login.username && u.password == login.password).FirstOrDefault();
            if (users == null)
            {
                return NotFound();
            }

            return GetToken(users);
        }



    }
}