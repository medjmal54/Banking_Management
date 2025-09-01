using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Table("LOAN_PAYMENT")]
public partial class LoanPayment
{
    [Key]
    [Column("PAYMENTID")]
    public int Paymentid { get; set; }

    public DateOnly? PaymentDte { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal RmainingBalance { get; set; }

    [Column("LoanID")]
    public int? LoanId { get; set; }

    [ForeignKey("LoanId")]
    [InverseProperty("LoanPayments")]
    public virtual Loan? Loan { get; set; }
}
