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
    public class PostLikesController : ControllerBase
    {
        private readonly HellotorontoContext _context;

        public PostLikesController(HellotorontoContext context)
        {
            _context = context;
        }


        // GET: api/PostLikes/3
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<PostLike>>> GetPostLikeByUser(int userId)
        {
            var postLike = await _context.PostLike.FindAsync(userId);

            if (postLike == null)
            {
                return NotFound();
            }

            return await _context.PostLike.Where(o => o.UserId == userId).ToListAsync();
        }

        // GET: api/PostLikes/3
        [HttpGet("getPostLikes/{postId}")]
        public async Task<int> GetPostLikeCount(int postId)
        {
            var postLike = await _context.PostLike.FindAsync(postId);

            if (postLike == null)
            {
                return 0;
            }

            return await _context.PostLike.Where(o => o.PostId == postId).CountAsync();
        }



    
        // POST: api/PostLikes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostLike>> PostPostLike(PostLike postLike)
        {
            _context.PostLike.Add(postLike);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostLikeExists(postLike.PostLikeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPostLike", new { id = postLike.PostLikeId }, postLike);
        }

        // DELETE: api/PostLikes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostLike(int id)
        {
            var postLike = await _context.PostLike.FindAsync(id);
            if (postLike == null)
            {
                return NotFound();
            }

            _context.PostLike.Remove(postLike);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostLikeExists(int id)
        {
            return _context.PostLike.Any(e => e.PostLikeId == id);
        }
    }
}
