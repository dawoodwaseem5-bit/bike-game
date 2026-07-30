using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace backend.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        await SeedUsersAsync(context);
        await SeedCustomersAsync(context);
        await SeedProductsAsync(context);
        await SeedDiscountsAsync(context);
        await SeedQuotationsAsync(context);
    }

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private static async Task SeedUsersAsync(ApplicationDbContext ctx)
    {
        if (await ctx.Users.AnyAsync()) return;

        var now = DateTime.UtcNow;
        var hash = HashPassword("Password@123");

        var users = new List<User>
        {
            new() { Username = "ali.raza",     Email = "ali.raza@company.com",     PasswordHash = hash, FirstName = "Ali",    LastName = "Raza",        Role = "SalesRep", CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "sara.khan",    Email = "sara.khan@company.com",    PasswordHash = hash, FirstName = "Sara",   LastName = "Khan",        Role = "SalesRep", CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "umar.farooq",  Email = "umar.farooq@company.com",  PasswordHash = hash, FirstName = "Umar",   LastName = "Farooq",      Role = "SalesRep", CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "hamid.sheikh", Email = "hamid.sheikh@company.com", PasswordHash = hash, FirstName = "Hamid",  LastName = "Sheikh",      Role = "Manager",  CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "nadia.malik",  Email = "nadia.malik@company.com",  PasswordHash = hash, FirstName = "Nadia",  LastName = "Malik",       Role = "Manager",  CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "zain.ahmed",   Email = "zain.ahmed@company.com",   PasswordHash = hash, FirstName = "Zain",   LastName = "Ahmed",       Role = "Manager",  CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "apex.tech",    Email = "orders@apextech.com",      PasswordHash = hash, FirstName = "Apex",   LastName = "Tech",        Role = "Customer", CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "global.sol",   Email = "purchase@globalsol.com",   PasswordHash = hash, FirstName = "Global", LastName = "Solutions",   Role = "Customer", CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "falcon.ent",   Email = "buy@falconent.com",        PasswordHash = hash, FirstName = "Falcon", LastName = "Enterprises", Role = "Customer", CreatedBy = "seeder", CreatedAt = now },
            new() { Username = "nova.sys",     Email = "accounts@novasys.com",     PasswordHash = hash, FirstName = "Nova",   LastName = "Systems",     Role = "Customer", CreatedBy = "seeder", CreatedAt = now },
        };

        ctx.Users.AddRange(users);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedCustomersAsync(ApplicationDbContext ctx)
    {
        if (await ctx.Customers.AnyAsync()) return;

        var now = DateTime.UtcNow;
        var customers = new List<Customer>
        {
            new() { Name = "Apex Technologies Ltd",  Email = "orders@apextech.com",    Phone = "021-34567890", Company = "Apex Technologies",  Address = "Plot 12, SITE, Karachi",          CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Global Solutions Inc",   Email = "purchase@globalsol.com", Phone = "042-35612890", Company = "Global Solutions",   Address = "Main Boulevard, Gulberg, Lahore", CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "TechCorp Pakistan",      Email = "info@techcorp.pk",       Phone = "051-28900123", Company = "TechCorp",           Address = "G-10 Markaz, Islamabad",          CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Falcon Enterprises",     Email = "buy@falconent.com",      Phone = "021-99012345", Company = "Falcon Enterprises", Address = "Korangi Industrial, Karachi",     CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Nova Systems",           Email = "accounts@novasys.com",   Phone = "042-11223344", Company = "Nova Systems",       Address = "DHA Phase 6, Lahore",             CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Blue Ridge Partners",    Email = "contact@blueridge.com",  Phone = "051-32109876", Company = "Blue Ridge Partners",Address = "F-8 Sector, Islamabad",           CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Delta Commerce",         Email = "delta@commerce.com",     Phone = "021-44556677", Company = "Delta Commerce",     Address = "Clifton Block 5, Karachi",        CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Summit Industries",      Email = "summit@industries.com",  Phone = "042-66778899", Company = "Summit Industries",  Address = "Johar Town, Lahore",              CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Horizon Group",          Email = "info@horizongroup.com",  Phone = "051-55667788", Company = "Horizon Group",      Address = "Blue Area, Islamabad",            CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Prime Logistics",        Email = "prime@logistics.pk",     Phone = "021-33445566", Company = "Prime Logistics",    Address = "Port Qasim, Karachi",             CreatedBy = "seeder", CreatedAt = now },
        };

        ctx.Customers.AddRange(customers);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext ctx)
    {
        if (await ctx.Products.AnyAsync()) return;

        var now = DateTime.UtcNow;
        var products = new List<Product>
        {
            new() { Name = "Laptop Pro 15",          Category = "Computers",   Unit = "pcs", UnitPrice = 120000, CostPrice = 90000,  StockQuantity = 50,  Description = "15-inch business laptop, i7, 16GB RAM, 512GB SSD",  CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Desktop Workstation",    Category = "Computers",   Unit = "pcs", UnitPrice = 85000,  CostPrice = 62000,  StockQuantity = 30,  Description = "High-performance desktop, i9, 32GB RAM, 1TB SSD",   CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "27-inch Monitor",        Category = "Displays",    Unit = "pcs", UnitPrice = 38000,  CostPrice = 27000,  StockQuantity = 80,  Description = "4K IPS display, 27-inch",                            CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Mechanical Keyboard",    Category = "Peripherals", Unit = "pcs", UnitPrice = 8500,   CostPrice = 5800,   StockQuantity = 120, Description = "Mechanical keyboard with RGB backlight",             CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Wireless Mouse",         Category = "Peripherals", Unit = "pcs", UnitPrice = 4200,   CostPrice = 2900,   StockQuantity = 200, Description = "Ergonomic wireless mouse, 1600 DPI",                CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "USB-C Hub 7-in-1",       Category = "Accessories", Unit = "pcs", UnitPrice = 3500,   CostPrice = 2200,   StockQuantity = 150, Description = "USB-C hub with HDMI, USB 3.0, SD card reader",      CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "HD Webcam 1080p",        Category = "Peripherals", Unit = "pcs", UnitPrice = 12000,  CostPrice = 8500,   StockQuantity = 60,  Description = "Full HD webcam with built-in microphone",           CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Noise-Cancel Headset",   Category = "Audio",       Unit = "pcs", UnitPrice = 9500,   CostPrice = 6500,   StockQuantity = 75,  Description = "Over-ear headset with active noise cancellation",   CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "24-Port Network Switch", Category = "Networking",  Unit = "pcs", UnitPrice = 28000,  CostPrice = 20000,  StockQuantity = 25,  Description = "Managed 24-port Gigabit Ethernet switch",           CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "External HDD 2TB",       Category = "Storage",     Unit = "pcs", UnitPrice = 15000,  CostPrice = 10500,  StockQuantity = 90,  Description = "2TB portable external hard drive, USB 3.0",         CreatedBy = "seeder", CreatedAt = now },
        };

        ctx.Products.AddRange(products);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedDiscountsAsync(ApplicationDbContext ctx)
    {
        if (await ctx.Discounts.AnyAsync()) return;

        var now = DateTime.UtcNow;
        var discounts = new List<Discount>
        {
            new() { Name = "New Client Welcome", Code = "WELCOME10", DiscountType = "Percentage", Value = 10,    Description = "10% off for new clients",                     MinOrderAmount = 0,      CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Bulk Order 15%",     Code = "BULK15",    DiscountType = "Percentage", Value = 15,    Description = "15% for orders above 200,000",                MinOrderAmount = 200000, CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Loyalty Discount",   Code = "LOYAL20",   DiscountType = "Percentage", Value = 20,    Description = "20% for repeat customers",                    MinOrderAmount = 50000,  CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Seasonal Sale",      Code = "SUMMER25",  DiscountType = "Percentage", Value = 25,    Description = "Summer clearance discount",                   StartDate = new DateTime(2026, 6, 1), EndDate = new DateTime(2026, 8, 31), CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Government Tender",  Code = "GOVT5",     DiscountType = "Percentage", Value = 5,     Description = "5% for government/public sector orders",      MinOrderAmount = 100000, CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Flat 5000 Off",      Code = "FLAT5K",    DiscountType = "Fixed",      Value = 5000,  Description = "Flat PKR 5,000 off on any order",             MinOrderAmount = 30000,  CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Flat 10000 Off",     Code = "FLAT10K",   DiscountType = "Fixed",      Value = 10000, Description = "Flat PKR 10,000 off on large orders",         MinOrderAmount = 80000,  CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Peripherals Bundle", Code = "PERIPH12",  DiscountType = "Percentage", Value = 12,    Description = "12% on peripheral accessories bundle",        MinOrderAmount = 20000,  CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Year-End Clearance", Code = "YEAREND30", DiscountType = "Percentage", Value = 30,    Description = "Year-end stock clearance",                    StartDate = new DateTime(2026, 12, 1), EndDate = new DateTime(2026, 12, 31), CreatedBy = "seeder", CreatedAt = now },
            new() { Name = "Corporate Account",  Code = "CORP8",     DiscountType = "Percentage", Value = 8,     Description = "8% standing discount for corporate accounts", MinOrderAmount = 0,      CreatedBy = "seeder", CreatedAt = now },
        };

        ctx.Discounts.AddRange(discounts);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedQuotationsAsync(ApplicationDbContext ctx)
    {
        if (await ctx.Quotations.AnyAsync()) return;

        var customers = await ctx.Customers.ToListAsync();
        var products  = await ctx.Products.ToListAsync();
        var now       = DateTime.UtcNow;
        const decimal ApprovalThreshold = 500_000m;

        Customer C(int i) => customers[i % customers.Count];
        Product  P(int i) => products[i % products.Count];

        var quotationDefs = new[]
        {
            (0, "Draft",    30,  1,  new[]{(0,2,0m,17m),(3,4,5m,17m)}),
            (1, "Draft",    30,  2,  new[]{(1,1,0m,17m),(4,3,0m,17m),(5,5,0m,17m)}),
            (2, "Pending",  30,  5,  new[]{(0,5,10m,17m),(1,2,10m,17m),(2,3,5m,17m)}),
            (3, "Pending",  15,  3,  new[]{(8,2,5m,17m),(9,4,0m,17m)}),
            (4, "Approved", 20,  10, new[]{(0,3,15m,17m),(2,2,10m,17m),(6,1,0m,17m)}),
            (5, "Approved", 25,  12, new[]{(1,4,10m,17m),(3,6,5m,17m)}),
            (6, "Approved", 30,  14, new[]{(0,8,20m,17m),(1,3,15m,17m),(2,5,10m,17m)}),
            (7, "Rejected", -1,  20, new[]{(0,10,30m,17m),(4,8,25m,17m)}),
            (8, "Rejected", -1,  22, new[]{(8,3,20m,17m),(9,5,15m,17m)}),
            (9, "Expired",  -5,  25, new[]{(2,4,5m,17m),(5,6,0m,17m),(6,2,0m,17m)}),
            (0, "Approved", 30,  8,  new[]{(3,10,8m,17m),(4,10,8m,17m),(5,10,8m,17m)}),
            (1, "Draft",    30,  1,  new[]{(7,2,0m,17m),(8,1,0m,17m)}),
            (2, "Pending",  30,  4,  new[]{(0,4,12m,17m),(1,2,12m,17m),(9,3,0m,17m)}),
            (3, "Approved", 20,  15, new[]{(6,3,10m,17m),(7,4,10m,17m)}),
            (4, "Approved", 30,  18, new[]{(8,2,0m,17m),(2,3,5m,17m),(3,5,5m,17m)}),
            (5, "Rejected", -1,  30, new[]{(0,6,25m,17m),(1,4,20m,17m)}),
            (6, "Draft",    30,  0,  new[]{(4,3,0m,17m),(5,4,0m,17m),(6,1,0m,17m)}),
            (7, "Approved", 25,  11, new[]{(0,2,10m,17m),(3,3,10m,17m),(9,2,5m,17m)}),
            (8, "Pending",  30,  2,  new[]{(1,5,15m,17m),(8,2,10m,17m)}),
            (9, "Expired",  -3,  35, new[]{(2,6,0m,17m),(7,3,0m,17m)}),
            (0, "Approved", 30,  20, new[]{(0,10,18m,17m),(1,5,15m,17m),(8,3,10m,17m),(9,6,5m,17m)}),
            (1, "Draft",    30,  0,  new[]{(3,2,0m,17m),(4,2,0m,17m)}),
            (2, "Approved", 20,  16, new[]{(5,8,10m,17m),(6,4,8m,17m)}),
            (3, "Pending",  30,  3,  new[]{(7,5,12m,17m),(0,2,12m,17m),(2,3,8m,17m)}),
            (4, "Rejected", -1,  28, new[]{(0,7,28m,17m),(1,3,22m,17m)}),
        };

        int qNum = 1;
        foreach (var (custIdx, status, validDays, createdDaysAgo, lines) in quotationDefs)
        {
            var createdAt  = now.AddDays(-createdDaysAgo);
            var validUntil = now.AddDays(validDays);
            var salesRep   = "ali.raza";
            var items      = new List<QuotationItem>();
            decimal subTotal = 0;

            foreach (var (pIdx, qty, discPct, taxRate) in lines)
            {
                var product   = P(pIdx);
                var unitPrice = product.UnitPrice;
                var grossLine = unitPrice * qty;
                var discAmt   = Math.Round(grossLine * discPct / 100, 2);
                var netLine   = grossLine - discAmt;
                var taxAmt    = Math.Round(netLine * taxRate / 100, 2);
                var lineTotal = netLine + taxAmt;

                items.Add(new QuotationItem
                {
                    ProductId       = product.ProductId,
                    Quantity        = qty,
                    UnitPrice       = unitPrice,
                    DiscountPercent = discPct,
                    DiscountAmount  = discAmt,
                    TaxRate         = taxRate,
                    LineTotal       = lineTotal,
                    CreatedBy       = salesRep,
                    CreatedAt       = createdAt
                });

                subTotal += netLine;
            }

            var taxRate17 = 17m;
            var taxTotal  = Math.Round(subTotal * taxRate17 / 100, 2);
            var discTotal = items.Sum(i => i.DiscountAmount);
            var total     = subTotal + taxTotal;

            var quotation = new Quotation
            {
                QuotationNumber = $"QT-{createdAt.Year}-{qNum:D4}",
                CustomerId      = C(custIdx).CustomerId,
                Status          = status,
                SubTotal        = subTotal,
                TaxRate         = taxRate17,
                TaxAmount       = taxTotal,
                DiscountAmount  = discTotal,
                TotalAmount     = total,
                ValidUntil      = validUntil,
                Notes           = $"Quotation {qNum} — {status}",
                CreatedBy       = salesRep,
                CreatedAt       = createdAt,
                QuotationItems  = items
            };

            ctx.Quotations.Add(quotation);
            await ctx.SaveChangesAsync();

            var historyEntries = new List<StatusHistory>
            {
                new()
                {
                    QuotationId = quotation.QuotationId,
                    OldStatus   = "",
                    NewStatus   = "Draft",
                    ChangedBy   = salesRep,
                    ChangedAt   = createdAt,
                    CreatedBy   = salesRep,
                    CreatedAt   = createdAt
                }
            };

            if (status != "Draft")
            {
                historyEntries.Add(new StatusHistory
                {
                    QuotationId = quotation.QuotationId,
                    OldStatus   = "Draft",
                    NewStatus   = status == "Approved" || status == "Rejected" ? "Pending" : status,
                    ChangedBy   = salesRep,
                    ChangedAt   = createdAt.AddDays(1),
                    CreatedBy   = salesRep,
                    CreatedAt   = createdAt.AddDays(1)
                });
            }

            if (status == "Approved" || status == "Rejected")
            {
                historyEntries.Add(new StatusHistory
                {
                    QuotationId = quotation.QuotationId,
                    OldStatus   = "Pending",
                    NewStatus   = status,
                    ChangedBy   = "hamid.sheikh",
                    ChangedAt   = createdAt.AddDays(2),
                    Remarks     = status == "Approved" ? "Approved by manager." : "Rejected — pricing too aggressive.",
                    CreatedBy   = "hamid.sheikh",
                    CreatedAt   = createdAt.AddDays(2)
                });
            }

            ctx.StatusHistories.AddRange(historyEntries);

            if ((status == "Pending" || status == "Approved" || status == "Rejected")
                && total >= ApprovalThreshold)
            {
                var approval = new Approval
                {
                    QuotationId = quotation.QuotationId,
                    RequestedBy = salesRep,
                    RequestedAt = createdAt.AddDays(1),
                    Status      = status == "Pending" ? "Pending" : status,
                    Threshold   = ApprovalThreshold,
                    CreatedBy   = salesRep,
                    CreatedAt   = createdAt.AddDays(1)
                };

                if (status == "Approved")
                {
                    approval.ApprovedBy = "hamid.sheikh";
                    approval.ApprovedAt = createdAt.AddDays(2);
                    approval.Remarks    = "Pricing justified. Approved.";
                }
                else if (status == "Rejected")
                {
                    approval.ApprovedBy = "hamid.sheikh";
                    approval.ApprovedAt = createdAt.AddDays(2);
                    approval.Remarks    = "Discount too high, rejected.";
                }

                ctx.Approvals.Add(approval);
            }

            await ctx.SaveChangesAsync();

            if (qNum <= 20)
            {
                foreach (var item in quotation.QuotationItems)
                {
                    var product   = products.First(p => p.ProductId == item.ProductId);
                    var margin    = product.UnitPrice - product.CostPrice;
                    var marginPct = Math.Round(margin / product.UnitPrice * 100, 2);
                    var normalMin = Math.Round(marginPct * 0.1m, 2);
                    var normalMax = Math.Round(marginPct * 0.6m, 2);
                    var isAnomaly = item.DiscountPercent > normalMax;
                    var score     = isAnomaly
                        ? Math.Min(Math.Round((item.DiscountPercent - normalMax) / normalMax, 4), 1.0m)
                        : 0m;

                    ctx.DiscountEvaluations.Add(new DiscountEvaluation
                    {
                        QuotationItemId  = item.QuotationItemId,
                        ProductId        = item.ProductId,
                        ProposedDiscount = item.DiscountPercent,
                        NormalRangeMin   = normalMin,
                        NormalRangeMax   = normalMax,
                        AnomalyScore     = score,
                        IsAnomaly        = isAnomaly,
                        Explanation      = isAnomaly
                            ? $"Proposed discount {item.DiscountPercent}% exceeds normal max {normalMax}% based on product margin {marginPct}%."
                            : $"Discount {item.DiscountPercent}% is within normal range ({normalMin}%–{normalMax}%).",
                        UserOverride     = isAnomaly && (status == "Approved" || status == "Rejected"),
                        UserConfirmation = isAnomaly ? (status == "Approved") : null,
                        EvaluatedBy      = salesRep,
                        EvaluatedAt      = createdAt,
                        CreatedBy        = salesRep,
                        CreatedAt        = createdAt
                    });
                }

                await ctx.SaveChangesAsync();
            }

            qNum++;
        }
    }
}
