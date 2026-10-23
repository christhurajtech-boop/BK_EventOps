using System;
using System.Collections.Generic;
using BK.EventOps.Domain.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BK.EventOps.Domain.Persistence;

public partial class BkEventOpsDbContext : DbContext
{
    public BkEventOpsDbContext(DbContextOptions<BkEventOpsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Test> Tests { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EventItem> Events { get; set; }
    public virtual DbSet<Attendance> Attendances { get; set; }
    public virtual DbSet<EventTransaction> Transactions { get; set; }
    public virtual DbSet<SalaryPayment> SalaryPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
