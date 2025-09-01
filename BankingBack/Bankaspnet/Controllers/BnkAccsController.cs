using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankingManagement.Models;
using AutoMapper;

namespace BankingManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BnkAccsController : ControllerBase
    {
        private readonly BankingManagementContext _context;
        public BnkAccsController(BankingManagementContext context)
        {
            _context = context;
        }

        // GET: api/BnkAccs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BnkAcc>>> GetBnkAccs()
        {
            return await _context.BnkAccs.ToListAsync();
        }

        // GET: api/BnkAccs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BnkAcc>> GetBnkAcc(int id)
        {
            var bnkAcc = await _context.BnkAccs.FindAsync(id);

            if (bnkAcc == null)
            {
                return NotFound();
            }

            return bnkAcc;
        }

        // PUT: api/BnkAccs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBnkAcc(int id, BnkAcc bnkAcc)
        {
            if (id != bnkAcc.Idacc)
            {
                return BadRequest();
            }

            _context.Entry(bnkAcc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BnkAccExists(id))
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

        // POST: api/BnkAccs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BnkAcc>> PostBnkAcc(BnkAcc bnkAcc)
        {

            _context.BnkAccs.Add(bnkAcc);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (BnkAccExists(bnkAcc.Idacc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBnkAcc", new { id = bnkAcc.Idacc }, bnkAcc);
        }

        // DELETE: api/BnkAccs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBnkAcc(int id)
        {
            var bnkAcc = await _context.BnkAccs.FindAsync(id);
            if (bnkAcc == null)
            {
                return NotFound();
            }

            _context.BnkAccs.Remove(bnkAcc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // New endpoint to get balance by Rib
        [HttpGet("Balance/{rib}")]
        public async Task<ActionResult<decimal>> GetBalanceByRib(string rib)
        {
            if (string.IsNullOrWhiteSpace(rib))
            {
                return BadRequest("RIB cannot be empty.");
            }

            var bnkAcc = await _context.BnkAccs
                                       .FirstOrDefaultAsync(acc => acc.Rib == rib);

            if (bnkAcc == null)
            {
                return NotFound($"Account with RIB {rib} not found.");
            }

            return Ok(bnkAcc.Balance);
        }


        private bool BnkAccExists(int id)
        {
            return _context.BnkAccs.Any(e => e.Idacc == id);
        }
    }
}
