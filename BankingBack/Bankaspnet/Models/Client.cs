using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

public partial class Client
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("email")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("Address_Name")]
    [StringLength(70)]
    [Unicode(false)]
    public string? AddressName { get; set; }

    [Column("city")]
    [StringLength(20)]
    [Unicode(false)]
    public string? City { get; set; }

    [Column("country")]
    [StringLength(20)]
    [Unicode(false)]
    public string Country { get; set; } = null!;

    [InverseProperty("IdNavigation")]
    public virtual ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();

    [InverseProperty("Client")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
