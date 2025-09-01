using BankingManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercicesController : ControllerBase
    {
        private readonly BankingManagementContext _context;
        public ExercicesController(BankingManagementContext context)
        {
            _context = context;
        }

        // GET: api/Exercices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercice>>> GetExercices()
        {
            var Exercices = await _context.Exercices.ToListAsync();
            return Ok(Exercices);
        }

        // GET: api/Exercices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercice>> GetExercice(int id)
        {
            var exercice = await _context.Exercices.FindAsync(id);

            if (exercice == null)
            {
                return NotFound();
            } 
            return Ok(exercice);
        }

        // PUT: api/Exercices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercice(int id, Exercice exercice)
        {
            if (id != exercice.CodExer)
            {
                return BadRequest();
            }

            _context.Entry(exercice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciceExists(id))
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

        // POST: api/Exercices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Exercice>> PostExercice(Exercice exercice)
        {
            _context.Exercices.Add(exercice);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ExerciceExists(exercice.CodExer))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetExercice", new { id = exercice.CodExer }, exercice);
        }

        // DELETE: api/Exercices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercice(int id)
        {
            var exercice = await _context.Exercices.FindAsync(id);
            if (exercice == null)
            {
                return NotFound();
            }

            _context.Exercices.Remove(exercice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciceExists(int id)
        {
            return _context.Exercices.Any(e => e.CodExer == id);
        }
    }
}
