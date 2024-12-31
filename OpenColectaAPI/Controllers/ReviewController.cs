using OpenColectaAPI.Data;
using OpenColectaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OpenColectaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly OpenColectaContext _context;

        public ReviewsController(OpenColectaContext context)
        {
            _context = context;
        }

        // GET: api/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetAll()
        {
            return await _context.Reviews
                                 .Include(r => r.Materia)
                                 .ToListAsync();
        }

        // GET: api/reviews/{materiaId}/{respondente}
        [HttpGet("{materiaId}/{respondente}")]
        public async Task<ActionResult<Review>> GetByCompositeKey(int materiaId, string respondente)
        {
            var review = await _context.Reviews
                                       .Include(r => r.Materia)
                                       .FirstOrDefaultAsync(r =>
                                            r.Id == materiaId &&
                                            r.Respondente == respondente);

            if (review == null) return NotFound();
            return review;
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<ActionResult<Review>> Create(Review review)
        {
            // Validate the Materia referenced by materia_id
            var materia = await _context.Materias.FindAsync(review.Id);
            if (materia == null)
            {
                return BadRequest($"No Materia found with ID = {review.Id}");
            }

            // Ensure no conflict with existing (materiaId, respondente)
            var existingReview = await _context.Reviews.FindAsync(review.Id, review.Respondente);
            if (existingReview != null)
            {
                return Conflict("A Review for this (materiaId, respondente) already exists.");
            }

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Return the route to retrieve it by composite key
            return CreatedAtAction(nameof(GetByCompositeKey),
                new { materiaId = review.Id, respondente = review.Respondente },
                review
            );
        }

        // PUT: api/reviews/{materiaId}/{respondente}
        [HttpPut("{materiaId}/{respondente}")]
        public async Task<IActionResult> Update(int materiaId, string respondente, Review review)
        {
            // Check route vs body
            if (materiaId != review.Id || respondente != review.Respondente)
            {
                return BadRequest("URL composite key does not match the review data.");
            }

            // Ensure the Materia exists
            var materia = await _context.Materias.FindAsync(review.Id);
            if (materia == null)
            {
                return BadRequest($"No Materia found with ID = {review.Id}");
            }

            // Mark entity as modified
            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Double-check if it truly doesn't exist
                var exists = await _context.Reviews.FindAsync(materiaId, respondente);
                if (exists == null)
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/reviews/{materiaId}/{respondente}
        [HttpDelete("{materiaId}/{respondente}")]
        public async Task<IActionResult> Delete(int materiaId, string respondente)
        {
            var review = await _context.Reviews.FindAsync(materiaId, respondente);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
