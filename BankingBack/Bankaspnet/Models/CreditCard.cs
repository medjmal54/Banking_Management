using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Table("Credit_Card")]
public partial class CreditCard
{
    [Key]
    [Column("RIB")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rib { get; set; } = null!;

    [Column("Expiration_date")]
    [StringLength(5)]
    [Unicode(false)]
    public string? ExpirationDate { get; set; }

    [Column("cvv")]
    public int? Cvv { get; set; }

    [Column("ID")]
    public int? Id { get; set; }

    [InverseProperty("RibNavigation")]
    public virtual ICollection<BnkAcc> BnkAccs { get; set; } = new List<BnkAcc>();

    [ForeignKey("Id")]
    [InverseProperty("CreditCards")]
    public virtual Client? IdNavigation { get; set; }
}
