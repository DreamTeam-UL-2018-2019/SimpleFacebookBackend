using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleFacebookBackend.Models;

namespace SimpleFacebookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("login")]
        [DisableCors]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            //var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

            User findUser = new User {
                Id = 1,
                Email = "adammalej1@gmail.com",
                FirstName = "Adam",
                LastName = "Malej",
                Password = "adammalej"
            };

            if (findUser == null)
            {
                //TODO: json understand 'null' and get error, need to parse or send exception from backend to frontend
                // that is not user about this email and password in database
                return Unauthorized();
            }

            JwtSecurityToken token = GetToken();

            token.Payload["user"] = findUser;

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        private JwtSecurityToken GetToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySupersecurityKey"));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:50882",
                audience: "http://localhost:50882",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return token;
        }

        private bool UserRequiredFields(User user)
        {
            if (String.IsNullOrEmpty(user.FirstName)) return false;
            if (String.IsNullOrEmpty(user.LastName)) return false;
            if (String.IsNullOrEmpty(user.Email)) return false;
            if (String.IsNullOrEmpty(user.Password)) return false;
            return true;
        }
    }
}
