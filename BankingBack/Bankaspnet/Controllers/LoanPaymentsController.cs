using AutoMapper;
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
    public class LoanPaymentsController : ControllerBase
    {
        private readonly BankingManagementContext _context;
        

        public LoanPaymentsController(BankingManagementContext context)
        {
            _context = context;
        }

        // GET: api/LoanPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanPayment>>> GetLoanPayments()
        {
            return await _context.LoanPayments.ToListAsync();
        }

        // GET: api/LoanPayments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanPayment>> GetLoanPayment(int id)
        {
            var loanPayment = await _context.LoanPayments.FindAsync(id);

            if (loanPayment == null)
            {
                return NotFound();
            }

            return loanPayment;
        }

        // PUT: api/LoanPayments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanPayment(int id, LoanPayment loanPayment)
        {
            if (id != loanPayment.Paymentid)
            {
                return BadRequest();
            }

            _context.Entry(loanPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanPaymentExists(id))
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

        // POST: api/LoanPayments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoanPayment>> PostLoanPayment(LoanPayment loanPayment)
        {
            _context.LoanPayments.Add(loanPayment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LoanPaymentExists(loanPayment.Paymentid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLoanPayment", new { id = loanPayment.Paymentid }, loanPayment);
        }

        // DELETE: api/LoanPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanPayment(int id)
        {
            var loanPayment = await _context.LoanPayments.FindAsync(id);
            if (loanPayment == null)
            {
                return NotFound();
            }

            _context.LoanPayments.Remove(loanPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanPaymentExists(int id)
        {
            return _context.LoanPayments.Any(e => e.Paymentid == id);
        }
    }
}
