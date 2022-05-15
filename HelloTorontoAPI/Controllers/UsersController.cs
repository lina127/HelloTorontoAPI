#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelloTorontoAPI.Data;
using HelloTorontoAPI.Models;

namespace HelloTorontoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HellotorontoContext _context;

        public UsersController(HellotorontoContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // [True] if user exit
        [HttpPost]
        [Route("/api/Users/isUserExist")]
        public bool GetUser(string email, string password)
        {
            email = email.Trim().ToLower();
            var user = _context.User.Where(o => o.Email == email && o.Password == password).ToList();
            if (user.Count == 1)
            {
                return true;
            }
            return false;
        }

        // REGISTER
        [HttpPost]
        [Route("/api/Users/Register")]
        public async Task<string> Register(string email, string password, string gender, DateTime birthdate, string nickname)
        {
            email = email.Trim().ToLower();
            var user = _context.User.Where(o => o.Email == email).ToList();

            if (user.Count > 0)
            {
                return "Email Already Exists";
            }
            User newUser = new User()
            {
                Email = email,
                Password = password,
                Gender = gender,
                Birthdate = birthdate,
                Nickname = nickname
            };
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();
            return "success";
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
