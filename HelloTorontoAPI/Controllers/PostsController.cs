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
using System.Data;

namespace HelloTorontoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly HellotorontoContext _context;

        public PostsController(HellotorontoContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()
        {
            return await _context.Post.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Post.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpGet("/api/Posts/GetPostsByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostByCategory(int categoryId)
        {
            var post = await _context.Post.FindAsync(categoryId);
            if(post == null)
            {
                return NotFound();
            }

            return await _context.Post.Where(o=>o.CategoryId==categoryId).ToListAsync();
        }

        [HttpGet("/api/Posts/GetPostsByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostByUser(int userId)
        {
            var post = await _context.Post.FindAsync(userId);
            if (post == null)
            {
                return NotFound();
            }

            return await _context.Post.Where(o => o.UserId == userId).ToListAsync();
        }


        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.PostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            _context.Post.Add(post);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostExists(post.PostId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("/api/Posts/Pagenation")]
        public async Task<ActionResult<IEnumerable<Post>>> GetTenPosts(int pageNum)
        {
            var post = await _context.Post.Take(10).ToListAsync();
            if (pageNum != 1)
            {
                post = await _context.Post.Skip(pageNum * 15).Take(10).ToListAsync();
            }
            
            return post;
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}
