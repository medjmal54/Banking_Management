using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Table("LOAN")]
public partial class Loan
{
    [Key]
    [Column("LOANID")]
    public int Loanid { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string LoanType { get; set; } = null!;

    public float LoanAmount { get; set; }

    [Column("Loan_Status")]
    [StringLength(1)]
    [Unicode(false)]
    public string LoanStatus { get; set; } = null!;

    [Column("Client_Id")]
    public int? ClientId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("Loans")]
    public virtual Client? Client { get; set; }

    [InverseProperty("Loan")]
    public virtual ICollection<LoanPayment> LoanPayments { get; set; } = new List<LoanPayment>();
}
