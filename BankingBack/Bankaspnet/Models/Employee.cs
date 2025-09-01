using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Table("EMPLOYEE")]
public partial class Employee
{
    [Key]
    [Column("Emp_Matricule")]
    public int EmpMatricule { get; set; }

    [StringLength(45)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string Position { get; set; } = null!;

    public DateOnly? HireDate { get; set; }

    public float Salary { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Employees")]
    public virtual Branch? Branch { get; set; }
}
