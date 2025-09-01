using BankingManagement.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly BankingManagementContext _context;

    public TransactionController(BankingManagementContext context)
    {
        _context = context;
    }
    [HttpGet("{rib}")]
    public IActionResult GetTransactions(string rib)
    {
        var Tran = _context.Transactions.Where(c => c.Rib == rib).ToList();
        if (Tran == null)
        {
            return NotFound();
        }
        return Ok(Tran);
    }
}