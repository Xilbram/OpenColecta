using OpenColectaAPI.Data;
using OpenColectaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OpenColectaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriasController : ControllerBase
    {
        private readonly OpenColectaContext _context;

        public MateriasController(OpenColectaContext context)
        {
            _context = context;
        }

        // GET: api/materias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materia>>> GetAll()
        {
            return await _context.Materias
                                 .ToListAsync();
        }

        // GET: api/materias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materia>> GetById(int id)
        {
            var materia = await _context.Materias
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (materia == null) return NotFound();

            return materia;
        }

        // POST: api/materias
        [HttpPost]
        public async Task<ActionResult<Materia>> Create(Materia materia)
        {
            _context.Materias.Add(materia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = materia.Id }, materia);
        }

        // PUT: api/materias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Materia materia)
        {
            if (id != materia.Id)
                return BadRequest("ID mismatch.");

            _context.Entry(materia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/materias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return NotFound();

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriaExists(int id)
        {
            return _context.Materias.Any(e => e.Id == id);
        }
    }
}
