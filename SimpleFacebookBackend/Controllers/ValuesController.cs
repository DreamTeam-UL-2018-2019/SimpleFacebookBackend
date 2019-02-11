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
        FbDBContext _context = new FbDBContext();
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
            var findUser = await _context.User.FirstOrDefaultAsync(u => u.Mail == user.Mail && u.Password == user.Password);

            if (findUser == null || !findUser.Mail.Equals(user.Mail) || !findUser.Password.Equals(user.Password))
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

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!UserRequiredFields(user))
            {
                return BadRequest();
            }

           // _context.Add(user);
            //await _context.SaveChangesAsync();
            JwtSecurityToken token = GetToken();
            token.Payload["user"] = user;
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost]
        [Route("sendMessage")]
        public async void SendMessage(Message message)
        {
           
            using (var context = new FbDBContext())
            {
                context.Add(message);
                await context.SaveChangesAsync();
            }
        }

        [HttpGet]
        [Route("getMessage")]
        public List<Message> GetMessage()
        {
            using (var context = new FbDBContext())
            {
                //var myMessage = context.Message.Find(message.Id);
                // return myMessage.Message1;
                var myMessages = context.Message.ToList();
                return myMessages;
            }
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
            if (String.IsNullOrEmpty(user.Mail)) return false;
            if (String.IsNullOrEmpty(user.Password)) return false;
            return true;
        }
    }
}
