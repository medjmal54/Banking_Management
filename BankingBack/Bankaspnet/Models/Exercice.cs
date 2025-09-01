using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

[Table("EXERCICE")]
public partial class Exercice
{
    [Key]
    [Column("CodEXER")]
    public int CodExer { get; set; }

    [Column("DatBED")]
    public DateOnly? DatDeb { get; set; }

    [Column("DATFIN")]
    public DateOnly? Datfin { get; set; }
}
