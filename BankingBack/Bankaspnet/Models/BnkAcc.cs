using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Table("BNK_Acc")]
public partial class BnkAcc
{
    [Key]
    [Column("IDACC")]
    public int Idacc { get; set; }

    [Column("RIB")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Rib { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal Balance { get; set; }

    [Column("Max_Withdraw", TypeName = "decimal(15, 2)")]
    public decimal? MaxWithdraw { get; set; }

    [Column("Acc_Status")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AccStatus { get; set; }

    [ForeignKey("Rib")]
    [InverseProperty("BnkAccs")]
    public virtual CreditCard? RibNavigation { get; set; }
}
