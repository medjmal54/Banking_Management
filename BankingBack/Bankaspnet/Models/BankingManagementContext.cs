using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BankingManagement.Models;

public partial class BankingManagementContext : DbContext
{
    public BankingManagementContext(DbContextOptions<BankingManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BnkAcc> BnkAccs { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Exercice> Exercices { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<LoanPayment> LoanPayments { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BnkAcc>(entity =>
        {
            entity.HasKey(e => e.Idacc).HasName("PK__BNK_Acc__932128580D590BC4");

            entity.Property(e => e.Idacc).ValueGeneratedNever();
            entity.Property(e => e.AccStatus).IsFixedLength();

            entity.HasOne(d => d.RibNavigation).WithMany(p => p.BnkAccs).HasConstraintName("FK_RIMCRD_BL");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__BRANCH__A1682FA5D55C0AB5");

            entity.Property(e => e.BranchId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC27BA28B2AB");
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.Rib).HasName("PK__Credit_C__CAFF41347E4CBCA7");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.CreditCards).HasConstraintName("FK_IDClient");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpMatricule).HasName("PK__EMPLOYEE__98A48D0F0C1ECC5A");

            entity.Property(e => e.EmpMatricule).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees).HasConstraintName("FK_BREMP");
        });

        modelBuilder.Entity<Exercice>(entity =>
        {
            entity.HasKey(e => e.CodExer).HasName("PK__EXERCICE__5626EF28020F6351");

            entity.Property(e => e.CodExer).ValueGeneratedNever();
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Loanid).HasName("PK__LOAN__D98B7DC8893E1B77");

            entity.Property(e => e.Loanid).ValueGeneratedNever();
            entity.Property(e => e.LoanStatus).IsFixedLength();

            entity.HasOne(d => d.Client).WithMany(p => p.Loans).HasConstraintName("FK_CLLOAN");
        });

        modelBuilder.Entity<LoanPayment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("PK__LOAN_PAY__F9599AC87FAEAAEF");

            entity.Property(e => e.Paymentid).ValueGeneratedNever();

            entity.HasOne(d => d.Loan).WithMany(p => p.LoanPayments).HasConstraintName("FK_LOANPAY");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(k => k.TranId).HasName("PK__Transact__3214EC278555A7D1");
            entity.Property(e => e.TranDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TranType).IsFixedLength();

            entity.HasOne(d => d.RibNavigation).WithMany().HasConstraintName("FK_RIB_BA2TRAN");

            entity.HasOne(d => d.RibToNavigation).WithMany().HasConstraintName("FK_RIB_TO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
