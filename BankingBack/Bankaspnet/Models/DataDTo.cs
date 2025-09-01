using Azure.Identity;
using BankingManagement.Models;
using System.Transactions;

public class ClientAnalyticsDto
{
    public int ClientId { get; set; }
    public required string FullName { get; set; }
    public required CreditCardDto CreditCard { get; set; }
    public string? Email { get; set; }
    public string Country { get; set; } = null!;

    public required List<TransactionDto> Transactions { get; set; }
    public required List<LoanDto> Loans { get; set; }
}

public class CreditCardDto
{
    public required string Rib { get; set; }
}

public class TransactionDto
{
    public decimal Amount { get; set; }
    public required string TranType { get; set; }
    public string? RibTo { get; set; }
    public DateTime? TranDate { get; set; }
}

public class LoanDto
{
    public int LoanId { get; set; }
    public required string LoanType { get; set; }
    public required string LoanStatus { get; set; }
    public required List<LoanPaymentDTo> LoanPayment { get; set; } = new();
}

public class LoanPaymentDTo
{
    public int Paymentid { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RmainingBalance { get; set; }
}

public class TransferDTo
{
    public required String Rib { get; set; } = default!;
    public required int Cvv { get; set; }
    public required string ExpirationDate { get; set; } = default!;
    public required decimal Amount { get; set; }
    public required string RibTo { get; set; }
    public string TranType { get; set; } = "T";
    public DateTime TranDate { get; set; } = DateTime.Now;
}

