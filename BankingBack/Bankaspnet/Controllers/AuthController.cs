using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BankingManagement.Models;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BankingManagementContext _context;

        public AuthController(BankingManagementContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin(loginDTo login)
        {
            var account = _context.CreditCards.FirstOrDefault(a => a.Rib == login.Rib);
            if (account == null)
            {
                return NotFound("Invalid Rib");
            }
            var userlog = await _context.Clients.FirstOrDefaultAsync(a => a.Id == account.Id && login.FullName == a.FullName);
            if (userlog == null)
            {
                return NotFound("Invalid FullName!");
            }
            return Ok("Login Successful!");
        }

        [HttpGet("ClientsAnalytics/{rib}")]
        public async Task<IActionResult> DisplayAnalytics(string rib)
        {
            if (string.IsNullOrWhiteSpace(rib))
                return BadRequest("RIB cannot be empty.");


            var creditCard = await _context.CreditCards
                                           .Include(cc => cc.BnkAccs)
                                           .FirstOrDefaultAsync(cc => cc.Rib == rib);

            if (creditCard == null)
                return NotFound($"No credit card found with RIB {rib}.");


            var client = await _context.Clients
                                       .FirstOrDefaultAsync(c => c.Id == creditCard.Id);

            if (client == null)
                return NotFound($"No client found for credit card with RIB {rib}.");


            var transactions = await _context.Transactions
                                             .Where(t => t.Rib == rib)
                                             .ToListAsync();

            var Loan = await _context.Loans.Include(t => t.LoanPayments).Where(t => t.ClientId == client.Id).ToListAsync();

            var result = new ClientAnalyticsDto
            {
                ClientId = client.Id,
                FullName = client.FullName,
                Country = client.Country,
                CreditCard = new CreditCardDto
                {
                    Rib = creditCard.Rib
                },
                Transactions = transactions.Select(t => new TransactionDto
                {
                    Amount = t.Amount,
                    TranType = t.TranType,
                    RibTo = t.RibTo,
                    TranDate = t.TranDate
                }
                ).ToList(),
                Loans = Loan.Select(l => new LoanDto
                {
                    LoanId = l.Loanid,
                    LoanType = l.LoanType,
                    LoanStatus = l.LoanStatus,
                    LoanPayment = l.LoanPayments.Select(lp => new LoanPaymentDTo
                    {
                        Paymentid = lp.Paymentid,
                        AmountPaid = lp.AmountPaid,
                        RmainingBalance = lp.RmainingBalance
                    }).ToList()
                }).ToList()
            };

            return Ok(result);

        }
        [HttpPost("MakeTransfer")]
        public async Task<IActionResult> TransferMoney(TransferDTo transfer)
        {

            if (transfer.Amount <= 0)
                return BadRequest("Transfer amount must be greater than zero.");
            var AccountVerif = await _context.CreditCards.Include(a=> a.BnkAccs).FirstOrDefaultAsync(a => a.Cvv == transfer.Cvv && a.Rib == transfer.Rib && a.ExpirationDate == transfer.ExpirationDate);
            if (AccountVerif == null)
            {
                return Unauthorized("All inputs must be filled!");
            }
            var fromAccount = await _context.BnkAccs.FirstOrDefaultAsync(a => a.Rib == transfer.Rib && transfer.Cvv == AccountVerif.Cvv && transfer.ExpirationDate == AccountVerif.ExpirationDate);
            var toAccount = await _context.BnkAccs.FirstOrDefaultAsync(a => a.Rib == transfer.RibTo);

            if (toAccount == null || fromAccount == null)
                return NotFound("One or both accounts not found.");

            if (fromAccount.Rib == toAccount.Rib)
            {
                return Unauthorized("You cannot transfer money to yourself!");
            }
            if (fromAccount.Balance < transfer.Amount)
                return BadRequest("Insufficient funds.");

            // Debit sender
            fromAccount.Balance -= transfer.Amount;

            // Credit receiver
            toAccount.Balance += transfer.Amount;

            // Log transaction
            var transaction = new Transaction
            {
                Amount = transfer.Amount,
                TranDate = DateTime.Now,
                Rib = transfer.Rib,
                RibTo = transfer.RibTo,
                TranType = "T"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Transfer successful",
                FromBalance = fromAccount.Balance,
                ToBalance = toAccount.Balance
            });
        }

    }
}


