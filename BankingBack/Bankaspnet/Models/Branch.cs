using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Table("BRANCH")]
public partial class Branch
{
    [Key]
    [Column("BranchID")]
    public int BranchId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string BranchName { get; set; } = null!;

    [Column("city")]
    [StringLength(18)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [Column("country")]
    [StringLength(18)]
    [Unicode(false)]
    public string Country { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [InverseProperty("Branch")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
