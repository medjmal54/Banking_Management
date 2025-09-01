using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Keyless]
public partial class Transaction
{
    [Key]
    [Column("ID")]
    public int TranId { get; set; }


    [Column("RIB")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Rib { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal Amount { get; set; }

    [Column("Tran_Type")]
    [StringLength(1)]
    [Unicode(false)]
    public string TranType { get; set; } = null!;

    [Column("RIB_TO")]
    [StringLength(20)]
    [Unicode(false)]
    public string? RibTo { get; set; }

    [Column("Tran_Date", TypeName = "datetime")]
    public DateTime? TranDate { get; set; }

    [ForeignKey("Rib")]
    public virtual CreditCard? RibNavigation { get; set; }

    [ForeignKey("RibTo")]
    public virtual CreditCard? RibToNavigation { get; set; }
}
