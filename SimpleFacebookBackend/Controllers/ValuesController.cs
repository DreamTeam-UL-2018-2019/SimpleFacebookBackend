﻿using System;
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

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/values/5
        [HttpDelete]
        [Route("delete/{id}/{id1}")]
        public IActionResult DeleteGroup(int id, int id1)
        {
            var userGroup = _context.UserGroup.Single(z => z.IdGroup == id && z.IdUser == id1);
            _context.UserGroup.Remove(userGroup);
            _context.SaveChanges();
            return NoContent();
        }

        
        [HttpPost]
        [Route("saveChanges")]
        public IActionResult Post([FromBody] User user)
        {
            User changedUser = new User();
            changedUser = _context.User.FirstOrDefault(u => u.Id == user.Id);

            changedUser.Description = (user.Description == changedUser.Description) ? changedUser.Description : user.Description;
            changedUser.FirstName = (user.FirstName == changedUser.FirstName) ? changedUser.FirstName : user.FirstName;
            changedUser.LastName = (user.LastName == changedUser.LastName) ? changedUser.LastName : user.LastName;
            changedUser.Mail = (user.Mail == changedUser.Mail) ? changedUser.Mail : user.Mail;
            changedUser.Password = (user.Password == changedUser.Password) ? changedUser.Password : user.Password;
            changedUser.Phone = (user.Phone == changedUser.Phone) ? changedUser.Phone : user.Phone;
            _context.SaveChanges();

            JwtSecurityToken token = GetToken();

            token.Payload["user"] = changedUser;

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost]
        [Route("sendMessage")]
        public void SendMessage(Message message)
        {
            _context.Message.Add(message);
            _context.SaveChanges();
        }

        [HttpGet]
        [Route("getMessage")]
        public List<Message> GetMessage(int id, int id1)
        {
            using (var context = new FbDBContext())
            {
                //var myMessage = context.Message.Find(message.Id);
                // return myMessage.Message1;
                var myMessages = context.Message.ToList();
                return myMessages;
            }
        }


        [HttpPost]
        [Route("newGroup/{id}")]
        public IActionResult Post(Group obj, int id)
        {
            Group newGroup = new Group();
            newGroup.Name = obj.Name;
            newGroup.Date = DateTime.Now;
            _context.Group.Add(newGroup);
            _context.SaveChanges();

            var getIdNewGroup = _context.Group.FirstOrDefault(g => g.Name == obj.Name);
            int groupId = getIdNewGroup.Id;

            UserGroup ug = new UserGroup();
            ug.IdGroup = groupId;
            ug.IdUser = id;
            _context.UserGroup.Add(ug);
            _context.SaveChanges();

            return Ok();
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

        [HttpGet]
        [Route("group/{id}")]
        public async Task<IActionResult> Groups(int id)
        {   

            var userGroupName = _context.UserGroup.Where(u => u.IdUser == id).Join(_context.Group, a => a.IdGroup, b => b.Id, (a,b) => new { Id = b.Id, Name = b.Name });


            return Ok(userGroupName);
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
        [Route("ChatApp")]
        public async Task<IActionResult> ChatApp([FromBody] User user)
        {
            JwtSecurityToken token = GetToken();
            token.Payload["user"] = user;
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
            if (String.IsNullOrEmpty(user.Mail)) return false;
            if (String.IsNullOrEmpty(user.Password)) return false;
            return true;
        }

        
    }
}
