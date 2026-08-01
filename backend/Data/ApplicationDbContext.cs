using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<QuotationItem> QuotationItems => Set<QuotationItem>();
    public DbSet<Discount> Discounts => Set<Discount>();
    public DbSet<Approval> Approvals => Set<Approval>();
    public DbSet<StatusHistory> StatusHistories => Set<StatusHistory>();
    public DbSet<DiscountEvaluation> DiscountEvaluations => Set<DiscountEvaluation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Users ────────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.HasIndex(u => u.Role);
        });

        // ── Customers ────────────────────────────────────────────────────────
        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(c => c.CustomerId);
            e.HasIndex(c => c.Email).IsUnique();
            e.HasIndex(c => c.Name);
            e.HasIndex(c => c.Company);
        });

        // ── Products ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.ProductId);
            e.HasIndex(p => p.Name).IsUnique();
        });

        // ── Quotations ───────────────────────────────────────────────────────
        modelBuilder.Entity<Quotation>(e =>
        {
            e.HasKey(q => q.QuotationId);
            e.HasIndex(q => q.QuotationNumber).IsUnique();
            e.HasIndex(q => q.CustomerId);
            e.HasIndex(q => q.Status);
            e.HasIndex(q => q.CreatedAt);

            e.HasOne(q => q.Customer)
             .WithMany(c => c.Quotations)
             .HasForeignKey(q => q.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── QuotationItems ───────────────────────────────────────────────────
        modelBuilder.Entity<QuotationItem>(e =>
        {
            e.HasKey(qi => qi.QuotationItemId);
            e.HasIndex(qi => qi.QuotationId);
            e.HasIndex(qi => qi.ProductId);

            e.HasOne(qi => qi.Quotation)
             .WithMany(q => q.QuotationItems)
             .HasForeignKey(qi => qi.QuotationId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(qi => qi.Product)
             .WithMany(p => p.QuotationItems)
             .HasForeignKey(qi => qi.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Discounts ────────────────────────────────────────────────────────
        modelBuilder.Entity<Discount>(e =>
        {
            e.HasKey(d => d.DiscountId);
            e.HasIndex(d => d.Code).IsUnique();
            e.HasIndex(d => d.DiscountType);
        });

        // ── Approvals ────────────────────────────────────────────────────────
        modelBuilder.Entity<Approval>(e =>
        {
            e.HasKey(a => a.ApprovalId);
            e.HasIndex(a => a.QuotationId).IsUnique(); // one approval per quotation
            e.HasIndex(a => a.Status);

            e.HasOne(a => a.Quotation)
             .WithOne(q => q.Approval)
             .HasForeignKey<Approval>(a => a.QuotationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── StatusHistory ────────────────────────────────────────────────────
        modelBuilder.Entity<StatusHistory>(e =>
        {
            e.HasKey(sh => sh.StatusHistoryId);
            e.HasIndex(sh => sh.QuotationId);

            e.HasOne(sh => sh.Quotation)
             .WithMany(q => q.StatusHistories)
             .HasForeignKey(sh => sh.QuotationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── DiscountEvaluations ──────────────────────────────────────────────
        modelBuilder.Entity<DiscountEvaluation>(e =>
        {
            e.HasKey(de => de.EvaluationId);

            e.HasOne(de => de.QuotationItem)
             .WithMany(qi => qi.DiscountEvaluations)
             .HasForeignKey(de => de.QuotationItemId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(de => de.Product)
             .WithMany(p => p.DiscountEvaluations)
             .HasForeignKey(de => de.ProductId)
             .OnDelete(DeleteBehavior.NoAction); // avoid multiple cascade paths
        });
    }
}
