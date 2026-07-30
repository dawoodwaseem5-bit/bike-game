-- ============================================================================
-- SalesQuotationDB — Manual Seed Script
-- Database  : SalesQuotationDB (SQL Server 2022)
-- Use this  : Run manually in SSMS if DbSeeder (dotnet run) cannot be used
-- Password  : All users → Password@123
-- NOTE      : PasswordHash below is a STATIC PLACEHOLDER.
--             For real login to work, run the app once (dotnet run) so
--             DbSeeder inserts proper PBKDF2/SHA256 verifiable hashes.
--             Then comment out the Users block here before running this file.
-- ============================================================================

USE SalesQuotationDB;
GO

-- ============================================================================
-- 1. USERS  (10 records)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM Users)
BEGIN
    SET IDENTITY_INSERT Users ON;
    INSERT INTO Users
        (UserId, Username, Email, PasswordHash, FirstName, LastName, Role,
         IsActive, IsDeleted, CreatedAt, CreatedBy)
    VALUES
    (1,  'ali.raza',     'ali.raza@company.com',     'PLACEHOLDER_Hash_Password@123', 'Ali',    'Raza',        'SalesRep', 1, 0, '2026-07-31', 'seeder'),
    (2,  'sara.khan',    'sara.khan@company.com',    'PLACEHOLDER_Hash_Password@123', 'Sara',   'Khan',        'SalesRep', 1, 0, '2026-07-31', 'seeder'),
    (3,  'umar.farooq',  'umar.farooq@company.com',  'PLACEHOLDER_Hash_Password@123', 'Umar',   'Farooq',      'SalesRep', 1, 0, '2026-07-31', 'seeder'),
    (4,  'hamid.sheikh', 'hamid.sheikh@company.com', 'PLACEHOLDER_Hash_Password@123', 'Hamid',  'Sheikh',      'Manager',  1, 0, '2026-07-31', 'seeder'),
    (5,  'nadia.malik',  'nadia.malik@company.com',  'PLACEHOLDER_Hash_Password@123', 'Nadia',  'Malik',       'Manager',  1, 0, '2026-07-31', 'seeder'),
    (6,  'zain.ahmed',   'zain.ahmed@company.com',   'PLACEHOLDER_Hash_Password@123', 'Zain',   'Ahmed',       'Manager',  1, 0, '2026-07-31', 'seeder'),
    (7,  'apex.tech',    'orders@apextech.com',      'PLACEHOLDER_Hash_Password@123', 'Apex',   'Tech',        'Customer', 1, 0, '2026-07-31', 'seeder'),
    (8,  'global.sol',   'purchase@globalsol.com',   'PLACEHOLDER_Hash_Password@123', 'Global', 'Solutions',   'Customer', 1, 0, '2026-07-31', 'seeder'),
    (9,  'falcon.ent',   'buy@falconent.com',        'PLACEHOLDER_Hash_Password@123', 'Falcon', 'Enterprises', 'Customer', 1, 0, '2026-07-31', 'seeder'),
    (10, 'nova.sys',     'accounts@novasys.com',     'PLACEHOLDER_Hash_Password@123', 'Nova',   'Systems',     'Customer', 1, 0, '2026-07-31', 'seeder');
    SET IDENTITY_INSERT Users OFF;
END
GO

-- ============================================================================
-- 2. CUSTOMERS  (10 records)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM Customers)
BEGIN
    SET IDENTITY_INSERT Customers ON;
    INSERT INTO Customers
        (CustomerId, Name, Email, Phone, Company, Address,
         IsActive, IsDeleted, CreatedAt, CreatedBy)
    VALUES
    (1,  'Apex Technologies Ltd',  'orders@apextech.com',    '021-34567890', 'Apex Technologies',  'Plot 12, SITE, Karachi',          1, 0, '2026-07-31', 'seeder'),
    (2,  'Global Solutions Inc',   'purchase@globalsol.com', '042-35612890', 'Global Solutions',   'Main Boulevard, Gulberg, Lahore', 1, 0, '2026-07-31', 'seeder'),
    (3,  'TechCorp Pakistan',      'info@techcorp.pk',       '051-28900123', 'TechCorp',           'G-10 Markaz, Islamabad',          1, 0, '2026-07-31', 'seeder'),
    (4,  'Falcon Enterprises',     'buy@falconent.com',      '021-99012345', 'Falcon Enterprises', 'Korangi Industrial, Karachi',     1, 0, '2026-07-31', 'seeder'),
    (5,  'Nova Systems',           'accounts@novasys.com',   '042-11223344', 'Nova Systems',       'DHA Phase 6, Lahore',             1, 0, '2026-07-31', 'seeder'),
    (6,  'Blue Ridge Partners',    'contact@blueridge.com',  '051-32109876', 'Blue Ridge Partners','F-8 Sector, Islamabad',           1, 0, '2026-07-31', 'seeder'),
    (7,  'Delta Commerce',         'delta@commerce.com',     '021-44556677', 'Delta Commerce',     'Clifton Block 5, Karachi',        1, 0, '2026-07-31', 'seeder'),
    (8,  'Summit Industries',      'summit@industries.com',  '042-66778899', 'Summit Industries',  'Johar Town, Lahore',              1, 0, '2026-07-31', 'seeder'),
    (9,  'Horizon Group',          'info@horizongroup.com',  '051-55667788', 'Horizon Group',      'Blue Area, Islamabad',            1, 0, '2026-07-31', 'seeder'),
    (10, 'Prime Logistics',        'prime@logistics.pk',     '021-33445566', 'Prime Logistics',    'Port Qasim, Karachi',             1, 0, '2026-07-31', 'seeder');
    SET IDENTITY_INSERT Customers OFF;
END
GO

-- ============================================================================
-- 3. PRODUCTS  (10 records)
-- UnitPrice / CostPrice in PKR
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    SET IDENTITY_INSERT Products ON;
    INSERT INTO Products
        (ProductId, Name, Category, Unit, UnitPrice, CostPrice, StockQuantity, Description,
         IsActive, IsDeleted, CreatedAt, CreatedBy)
    VALUES
    (1,  'Laptop Pro 15',           'Computers',   'pcs', 120000, 90000,  50,  '15-inch business laptop, i7, 16GB RAM, 512GB SSD',  1, 0, '2026-07-31', 'seeder'),
    (2,  'Desktop Workstation',     'Computers',   'pcs', 85000,  62000,  30,  'High-performance desktop, i9, 32GB RAM, 1TB SSD',   1, 0, '2026-07-31', 'seeder'),
    (3,  '27-inch Monitor',         'Displays',    'pcs', 38000,  27000,  80,  '4K IPS display, 27-inch',                           1, 0, '2026-07-31', 'seeder'),
    (4,  'Mechanical Keyboard',     'Peripherals', 'pcs', 8500,   5800,   120, 'Mechanical keyboard with RGB backlight',             1, 0, '2026-07-31', 'seeder'),
    (5,  'Wireless Mouse',          'Peripherals', 'pcs', 4200,   2900,   200, 'Ergonomic wireless mouse, 1600 DPI',                1, 0, '2026-07-31', 'seeder'),
    (6,  'USB-C Hub 7-in-1',        'Accessories', 'pcs', 3500,   2200,   150, 'USB-C hub with HDMI, USB 3.0, SD card reader',     1, 0, '2026-07-31', 'seeder'),
    (7,  'HD Webcam 1080p',         'Peripherals', 'pcs', 12000,  8500,   60,  'Full HD webcam with built-in microphone',           1, 0, '2026-07-31', 'seeder'),
    (8,  'Noise-Cancel Headset',    'Audio',       'pcs', 9500,   6500,   75,  'Over-ear headset with active noise cancellation',  1, 0, '2026-07-31', 'seeder'),
    (9,  '24-Port Network Switch',  'Networking',  'pcs', 28000,  20000,  25,  'Managed 24-port Gigabit Ethernet switch',           1, 0, '2026-07-31', 'seeder'),
    (10, 'External HDD 2TB',        'Storage',     'pcs', 15000,  10500,  90,  '2TB portable external hard drive, USB 3.0',        1, 0, '2026-07-31', 'seeder');
    SET IDENTITY_INSERT Products OFF;
END
GO

-- ============================================================================
-- 4. DISCOUNTS  (10 records)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM Discounts)
BEGIN
    SET IDENTITY_INSERT Discounts ON;
    INSERT INTO Discounts
        (DiscountId, Name, Code, DiscountType, Value, Description,
         MinOrderAmount, MaxOrderAmount, StartDate, EndDate,
         IsActive, IsDeleted, CreatedAt, CreatedBy)
    VALUES
    (1,  'New Client Welcome', 'WELCOME10', 'Percentage', 10,    '10% off for new clients',                     0,       NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder'),
    (2,  'Bulk Order 15%',     'BULK15',    'Percentage', 15,    '15% for orders above 200,000',                200000,  NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder'),
    (3,  'Loyalty Discount',   'LOYAL20',   'Percentage', 20,    '20% for repeat customers',                    50000,   NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder'),
    (4,  'Seasonal Sale',      'SUMMER25',  'Percentage', 25,    'Summer clearance discount',                   NULL,    NULL, '2026-06-01', '2026-08-31', 1, 0, '2026-07-31', 'seeder'),
    (5,  'Government Tender',  'GOVT5',     'Percentage', 5,     '5% for government/public sector orders',      100000,  NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder'),
    (6,  'Flat 5000 Off',      'FLAT5K',    'Fixed',      5000,  'Flat PKR 5,000 off on any order',             30000,   NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder'),
    (7,  'Flat 10000 Off',     'FLAT10K',   'Fixed',      10000, 'Flat PKR 10,000 off on large orders',         80000,   NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder'),
    (8,  'Peripherals Bundle', 'PERIPH12',  'Percentage', 12,    '12% on peripheral accessories bundle',        20000,   NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder'),
    (9,  'Year-End Clearance', 'YEAREND30', 'Percentage', 30,    'Year-end stock clearance',                    NULL,    NULL, '2026-12-01', '2026-12-31', 1, 0, '2026-07-31', 'seeder'),
    (10, 'Corporate Account',  'CORP8',     'Percentage', 8,     '8% standing discount for corporate accounts', 0,       NULL, NULL,         NULL,         1, 0, '2026-07-31', 'seeder');
    SET IDENTITY_INSERT Discounts OFF;
END
GO

-- ============================================================================
-- 5. QUOTATIONS  (25 records)
-- Approval threshold: 500,000 PKR
-- Totals computed from seeder logic (TaxRate = 17%)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM Quotations)
BEGIN
    SET IDENTITY_INSERT Quotations ON;
    INSERT INTO Quotations
        (QuotationId, QuotationNumber, CustomerId, Status,
         SubTotal, TaxRate, TaxAmount, DiscountAmount, TotalAmount,
         ValidUntil, Notes,
         IsActive, IsDeleted, CreatedAt, CreatedBy)
    VALUES
    --  ID  Number           CustId  Status     SubTotal    TxRt  TaxAmt      DiscAmt   Total       ValidUntil    Notes
    (1,  'QT-2026-0001', 1,  'Draft',    272300.00,  17, 46291.00,   1700.00,  318591.00,  '2026-08-30', 'Quotation 1 - Draft',    1, 0, '2026-07-30', 'ali.raza'),
    (2,  'QT-2026-0002', 2,  'Draft',    115100.00,  17, 19567.00,   0.00,     134667.00,  '2026-08-30', 'Quotation 2 - Draft',    1, 0, '2026-07-29', 'ali.raza'),
    (3,  'QT-2026-0003', 3,  'Pending',  801300.00,  17, 136221.00,  82700.00, 937521.00,  '2026-08-30', 'Quotation 3 - Pending',  1, 0, '2026-07-26', 'ali.raza'),
    (4,  'QT-2026-0004', 4,  'Pending',  113200.00,  17, 19244.00,   2800.00,  132444.00,  '2026-08-15', 'Quotation 4 - Pending',  1, 0, '2026-07-28', 'ali.raza'),
    (5,  'QT-2026-0005', 5,  'Approved', 386400.00,  17, 65688.00,   61600.00, 452088.00,  '2026-08-20', 'Quotation 5 - Approved', 1, 0, '2026-07-21', 'ali.raza'),
    (6,  'QT-2026-0006', 6,  'Approved', 354450.00,  17, 60256.50,   36550.00, 414706.50,  '2026-08-25', 'Quotation 6 - Approved', 1, 0, '2026-07-19', 'ali.raza'),
    (7,  'QT-2026-0007', 7,  'Approved', 1155750.00, 17, 196477.50,  249250.00,1352227.50, '2026-08-30', 'Quotation 7 - Approved', 1, 0, '2026-07-17', 'ali.raza'),
    (8,  'QT-2026-0008', 8,  'Rejected', 865200.00,  17, 147084.00,  368400.00,1012284.00, '2026-07-30', 'Quotation 8 - Rejected', 1, 0, '2026-07-11', 'ali.raza'),
    (9,  'QT-2026-0009', 9,  'Rejected', 130950.00,  17, 22261.50,   28050.00, 153211.50,  '2026-07-30', 'Quotation 9 - Rejected', 1, 0, '2026-07-09', 'ali.raza'),
    (10, 'QT-2026-0010', 10, 'Expired',  189400.00,  17, 32198.00,   7600.00,  221598.00,  '2026-07-26', 'Quotation 10 - Expired', 1, 0, '2026-07-06', 'ali.raza'),
    (11, 'QT-2026-0011', 1,  'Approved', 149040.00,  17, 25336.80,   12960.00, 174376.80,  '2026-08-30', 'Quotation 11 - Approved',1, 0, '2026-07-23', 'ali.raza'),
    (12, 'QT-2026-0012', 2,  'Draft',    47000.00,   17, 7990.00,    0.00,     54990.00,   '2026-08-30', 'Quotation 12 - Draft',   1, 0, '2026-07-30', 'ali.raza'),
    (13, 'QT-2026-0013', 3,  'Pending',  617000.00,  17, 104890.00,  78000.00, 721890.00,  '2026-08-30', 'Quotation 13 - Pending', 1, 0, '2026-07-27', 'ali.raza'),
    (14, 'QT-2026-0014', 4,  'Approved', 66600.00,   17, 11322.00,   7400.00,  77922.00,   '2026-08-20', 'Quotation 14 - Approved',1, 0, '2026-07-16', 'ali.raza'),
    (15, 'QT-2026-0015', 5,  'Approved', 204675.00,  17, 34794.75,   7825.00,  239469.75,  '2026-08-30', 'Quotation 15 - Approved',1, 0, '2026-07-13', 'ali.raza'),
    (16, 'QT-2026-0016', 6,  'Rejected', 812000.00,  17, 138040.00,  248000.00,950040.00,  '2026-07-30', 'Quotation 16 - Rejected',1, 0, '2026-07-01', 'ali.raza'),
    (17, 'QT-2026-0017', 7,  'Draft',    38600.00,   17, 6562.00,    0.00,     45162.00,   '2026-08-30', 'Quotation 17 - Draft',   1, 0, '2026-07-31', 'ali.raza'),
    (18, 'QT-2026-0018', 8,  'Approved', 267450.00,  17, 45466.50,   28050.00, 312916.50,  '2026-08-25', 'Quotation 18 - Approved',1, 0, '2026-07-20', 'ali.raza'),
    (19, 'QT-2026-0019', 9,  'Pending',  411650.00,  17, 69980.50,   69350.00, 481630.50,  '2026-08-30', 'Quotation 19 - Pending', 1, 0, '2026-07-29', 'ali.raza'),
    (20, 'QT-2026-0020', 10, 'Expired',  256500.00,  17, 43605.00,   0.00,     300105.00,  '2026-07-28', 'Quotation 20 - Expired', 1, 0, '2026-06-26', 'ali.raza'),
    (21, 'QT-2026-0021', 1,  'Approved', 1506350.00, 17, 256079.50,  292650.00,1762429.50, '2026-08-30', 'Quotation 21 - Approved',1, 0, '2026-07-11', 'ali.raza'),
    (22, 'QT-2026-0022', 2,  'Draft',    25400.00,   17, 4318.00,    0.00,     29718.00,   '2026-08-30', 'Quotation 22 - Draft',   1, 0, '2026-07-31', 'ali.raza'),
    (23, 'QT-2026-0023', 3,  'Approved', 69360.00,   17, 11791.20,   6640.00,  81151.20,   '2026-08-20', 'Quotation 23 - Approved',1, 0, '2026-07-15', 'ali.raza'),
    (24, 'QT-2026-0024', 4,  'Pending',  357880.00,  17, 60839.60,   43620.00, 418719.60,  '2026-08-30', 'Quotation 24 - Pending', 1, 0, '2026-07-28', 'ali.raza'),
    (25, 'QT-2026-0025', 5,  'Rejected', 803700.00,  17, 136629.00,  291300.00,940329.00,  '2026-07-30', 'Quotation 25 - Rejected',1, 0, '2026-07-03', 'ali.raza');
    SET IDENTITY_INSERT Quotations OFF;
END
GO

-- ============================================================================
-- 6. QUOTATION ITEMS  (63 line items across 25 quotations)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM QuotationItems)
BEGIN
    SET IDENTITY_INSERT QuotationItems ON;
    INSERT INTO QuotationItems
        (QuotationItemId, QuotationId, ProductId, Quantity, UnitPrice,
         DiscountPercent, DiscountAmount, TaxRate, LineTotal,
         IsActive, IsDeleted, CreatedAt, CreatedBy)
    VALUES
    -- Q1 (Draft)
    (1,  1,  1,  2,  120000, 0,  0,      17, 280800.00,  1, 0, '2026-07-30', 'ali.raza'),
    (2,  1,  4,  4,  8500,   5,  1700,   17, 37791.00,   1, 0, '2026-07-30', 'ali.raza'),
    -- Q2 (Draft)
    (3,  2,  2,  1,  85000,  0,  0,      17, 99450.00,   1, 0, '2026-07-29', 'ali.raza'),
    (4,  2,  5,  3,  4200,   0,  0,      17, 14742.00,   1, 0, '2026-07-29', 'ali.raza'),
    (5,  2,  6,  5,  3500,   0,  0,      17, 20475.00,   1, 0, '2026-07-29', 'ali.raza'),
    -- Q3 (Pending)
    (6,  3,  1,  5,  120000, 10, 60000,  17, 631800.00,  1, 0, '2026-07-26', 'ali.raza'),
    (7,  3,  2,  2,  85000,  10, 17000,  17, 179010.00,  1, 0, '2026-07-26', 'ali.raza'),
    (8,  3,  3,  3,  38000,  5,  5700,   17, 126711.00,  1, 0, '2026-07-26', 'ali.raza'),
    -- Q4 (Pending)
    (9,  4,  9,  2,  28000,  5,  2800,   17, 62244.00,   1, 0, '2026-07-28', 'ali.raza'),
    (10, 4,  10, 4,  15000,  0,  0,      17, 70200.00,   1, 0, '2026-07-28', 'ali.raza'),
    -- Q5 (Approved)
    (11, 5,  1,  3,  120000, 15, 54000,  17, 358020.00,  1, 0, '2026-07-21', 'ali.raza'),
    (12, 5,  3,  2,  38000,  10, 7600,   17, 80028.00,   1, 0, '2026-07-21', 'ali.raza'),
    (13, 5,  7,  1,  12000,  0,  0,      17, 14040.00,   1, 0, '2026-07-21', 'ali.raza'),
    -- Q6 (Approved)
    (14, 6,  2,  4,  85000,  10, 34000,  17, 358020.00,  1, 0, '2026-07-19', 'ali.raza'),
    (15, 6,  4,  6,  8500,   5,  2550,   17, 56686.50,   1, 0, '2026-07-19', 'ali.raza'),
    -- Q7 (Approved — large order, needs approval)
    (16, 7,  1,  8,  120000, 20, 192000, 17, 898560.00,  1, 0, '2026-07-17', 'ali.raza'),
    (17, 7,  2,  3,  85000,  15, 38250,  17, 253597.50,  1, 0, '2026-07-17', 'ali.raza'),
    (18, 7,  3,  5,  38000,  10, 19000,  17, 200070.00,  1, 0, '2026-07-17', 'ali.raza'),
    -- Q8 (Rejected — high discounts flagged)
    (19, 8,  1,  10, 120000, 30, 360000, 17, 982800.00,  1, 0, '2026-07-11', 'ali.raza'),
    (20, 8,  5,  8,  4200,   25, 8400,   17, 29484.00,   1, 0, '2026-07-11', 'ali.raza'),
    -- Q9 (Rejected)
    (21, 9,  9,  3,  28000,  20, 16800,  17, 78624.00,   1, 0, '2026-07-09', 'ali.raza'),
    (22, 9,  10, 5,  15000,  15, 11250,  17, 74587.50,   1, 0, '2026-07-09', 'ali.raza'),
    -- Q10 (Expired)
    (23, 10, 3,  4,  38000,  5,  7600,   17, 168948.00,  1, 0, '2026-07-06', 'ali.raza'),
    (24, 10, 6,  6,  3500,   0,  0,      17, 24570.00,   1, 0, '2026-07-06', 'ali.raza'),
    (25, 10, 7,  2,  12000,  0,  0,      17, 28080.00,   1, 0, '2026-07-06', 'ali.raza'),
    -- Q11 (Approved)
    (26, 11, 4,  10, 8500,   8,  6800,   17, 91494.00,   1, 0, '2026-07-23', 'ali.raza'),
    (27, 11, 5,  10, 4200,   8,  3360,   17, 45208.80,   1, 0, '2026-07-23', 'ali.raza'),
    (28, 11, 6,  10, 3500,   8,  2800,   17, 37674.00,   1, 0, '2026-07-23', 'ali.raza'),
    -- Q12 (Draft)
    (29, 12, 8,  2,  9500,   0,  0,      17, 22230.00,   1, 0, '2026-07-30', 'ali.raza'),
    (30, 12, 9,  1,  28000,  0,  0,      17, 32760.00,   1, 0, '2026-07-30', 'ali.raza'),
    -- Q13 (Pending — large)
    (31, 13, 1,  4,  120000, 12, 57600,  17, 494208.00,  1, 0, '2026-07-27', 'ali.raza'),
    (32, 13, 2,  2,  85000,  12, 20400,  17, 175032.00,  1, 0, '2026-07-27', 'ali.raza'),
    (33, 13, 10, 3,  15000,  0,  0,      17, 52650.00,   1, 0, '2026-07-27', 'ali.raza'),
    -- Q14 (Approved)
    (34, 14, 7,  3,  12000,  10, 3600,   17, 37908.00,   1, 0, '2026-07-16', 'ali.raza'),
    (35, 14, 8,  4,  9500,   10, 3800,   17, 40014.00,   1, 0, '2026-07-16', 'ali.raza'),
    -- Q15 (Approved)
    (36, 15, 9,  2,  28000,  0,  0,      17, 65520.00,   1, 0, '2026-07-13', 'ali.raza'),
    (37, 15, 3,  3,  38000,  5,  5700,   17, 126711.00,  1, 0, '2026-07-13', 'ali.raza'),
    (38, 15, 4,  5,  8500,   5,  2125,   17, 47238.75,   1, 0, '2026-07-13', 'ali.raza'),
    -- Q16 (Rejected — large, high discounts)
    (39, 16, 1,  6,  120000, 25, 180000, 17, 631800.00,  1, 0, '2026-07-01', 'ali.raza'),
    (40, 16, 2,  4,  85000,  20, 68000,  17, 318240.00,  1, 0, '2026-07-01', 'ali.raza'),
    -- Q17 (Draft)
    (41, 17, 5,  3,  4200,   0,  0,      17, 14742.00,   1, 0, '2026-07-31', 'ali.raza'),
    (42, 17, 6,  4,  3500,   0,  0,      17, 16380.00,   1, 0, '2026-07-31', 'ali.raza'),
    (43, 17, 7,  1,  12000,  0,  0,      17, 14040.00,   1, 0, '2026-07-31', 'ali.raza'),
    -- Q18 (Approved)
    (44, 18, 1,  2,  120000, 10, 24000,  17, 252720.00,  1, 0, '2026-07-20', 'ali.raza'),
    (45, 18, 4,  3,  8500,   10, 2550,   17, 26851.50,   1, 0, '2026-07-20', 'ali.raza'),
    (46, 18, 10, 2,  15000,  5,  1500,   17, 33345.00,   1, 0, '2026-07-20', 'ali.raza'),
    -- Q19 (Pending)
    (47, 19, 2,  5,  85000,  15, 63750,  17, 422662.50,  1, 0, '2026-07-29', 'ali.raza'),
    (48, 19, 9,  2,  28000,  10, 5600,   17, 58968.00,   1, 0, '2026-07-29', 'ali.raza'),
    -- Q20 (Expired)
    (49, 20, 3,  6,  38000,  0,  0,      17, 266760.00,  1, 0, '2026-06-26', 'ali.raza'),
    (50, 20, 8,  3,  9500,   0,  0,      17, 33345.00,   1, 0, '2026-06-26', 'ali.raza'),
    -- Q21 (Approved — very large)
    (51, 21, 1,  10, 120000, 18, 216000, 17, 1151280.00, 1, 0, '2026-07-11', 'ali.raza'),
    (52, 21, 2,  5,  85000,  15, 63750,  17, 422662.50,  1, 0, '2026-07-11', 'ali.raza'),
    (53, 21, 9,  3,  28000,  10, 8400,   17, 88452.00,   1, 0, '2026-07-11', 'ali.raza'),
    (54, 21, 10, 6,  15000,  5,  4500,   17, 100035.00,  1, 0, '2026-07-11', 'ali.raza'),
    -- Q22 (Draft)
    (55, 22, 4,  2,  8500,   0,  0,      17, 19890.00,   1, 0, '2026-07-31', 'ali.raza'),
    (56, 22, 5,  2,  4200,   0,  0,      17, 9828.00,    1, 0, '2026-07-31', 'ali.raza'),
    -- Q23 (Approved)
    (57, 23, 6,  8,  3500,   10, 2800,   17, 29484.00,   1, 0, '2026-07-15', 'ali.raza'),
    (58, 23, 7,  4,  12000,  8,  3840,   17, 51667.20,   1, 0, '2026-07-15', 'ali.raza'),
    -- Q24 (Pending)
    (59, 24, 8,  5,  9500,   12, 5700,   17, 48906.00,   1, 0, '2026-07-28', 'ali.raza'),
    (60, 24, 1,  2,  120000, 12, 28800,  17, 247104.00,  1, 0, '2026-07-28', 'ali.raza'),
    (61, 24, 3,  3,  38000,  8,  9120,   17, 122709.60,  1, 0, '2026-07-28', 'ali.raza'),
    -- Q25 (Rejected — very high discounts)
    (62, 25, 1,  7,  120000, 28, 235200, 17, 707616.00,  1, 0, '2026-07-03', 'ali.raza'),
    (63, 25, 2,  3,  85000,  22, 56100,  17, 232713.00,  1, 0, '2026-07-03', 'ali.raza');
    SET IDENTITY_INSERT QuotationItems OFF;
END
GO

-- ============================================================================
-- 7. APPROVALS
-- Condition: status in (Pending, Approved, Rejected) AND TotalAmount >= 500,000
-- Qualifying quotations: Q3, Q7, Q8, Q13, Q16, Q21, Q25
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM Approvals)
BEGIN
    SET IDENTITY_INSERT Approvals ON;
    INSERT INTO Approvals
        (ApprovalId, QuotationId, RequestedBy, RequestedAt, ApprovedBy, ApprovedAt,
         Status, Remarks, Threshold,
         IsActive, IsDeleted, CreatedAt, CreatedBy)
    VALUES
    (1, 3,  'ali.raza', '2026-07-27', NULL,           NULL,         'Pending',  NULL,                            500000, 1, 0, '2026-07-27', 'ali.raza'),
    (2, 7,  'ali.raza', '2026-07-18', 'hamid.sheikh', '2026-07-19', 'Approved', 'Pricing justified. Approved.',  500000, 1, 0, '2026-07-18', 'ali.raza'),
    (3, 8,  'ali.raza', '2026-07-12', 'hamid.sheikh', '2026-07-13', 'Rejected', 'Discount too high, rejected.',  500000, 1, 0, '2026-07-12', 'ali.raza'),
    (4, 13, 'ali.raza', '2026-07-28', NULL,           NULL,         'Pending',  NULL,                            500000, 1, 0, '2026-07-28', 'ali.raza'),
    (5, 16, 'ali.raza', '2026-07-02', 'hamid.sheikh', '2026-07-03', 'Rejected', 'Discount too high, rejected.',  500000, 1, 0, '2026-07-02', 'ali.raza'),
    (6, 21, 'ali.raza', '2026-07-12', 'hamid.sheikh', '2026-07-13', 'Approved', 'Pricing justified. Approved.',  500000, 1, 0, '2026-07-12', 'ali.raza'),
    (7, 25, 'ali.raza', '2026-07-04', 'hamid.sheikh', '2026-07-05', 'Rejected', 'Discount too high, rejected.',  500000, 1, 0, '2026-07-04', 'ali.raza');
    SET IDENTITY_INSERT Approvals OFF;
END
GO

-- ============================================================================
-- 8. STATUS HISTORY  (58 entries — auto-increment ID)
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM StatusHistories)
BEGIN
    INSERT INTO StatusHistories
        (QuotationId, OldStatus, NewStatus, ChangedBy, ChangedAt, Remarks, CreatedAt, CreatedBy)
    VALUES
    -- Q1 (Draft): 1 entry
    (1,  '', 'Draft', 'ali.raza', '2026-07-30', NULL, '2026-07-30', 'ali.raza'),
    -- Q2 (Draft): 1 entry
    (2,  '', 'Draft', 'ali.raza', '2026-07-29', NULL, '2026-07-29', 'ali.raza'),
    -- Q3 (Pending): 2 entries
    (3,  '', 'Draft',   'ali.raza', '2026-07-26', NULL, '2026-07-26', 'ali.raza'),
    (3,  'Draft', 'Pending', 'ali.raza', '2026-07-27', NULL, '2026-07-27', 'ali.raza'),
    -- Q4 (Pending): 2 entries
    (4,  '', 'Draft',   'ali.raza', '2026-07-28', NULL, '2026-07-28', 'ali.raza'),
    (4,  'Draft', 'Pending', 'ali.raza', '2026-07-29', NULL, '2026-07-29', 'ali.raza'),
    -- Q5 (Approved): 3 entries
    (5,  '', 'Draft',    'ali.raza',     '2026-07-21', NULL,                             '2026-07-21', 'ali.raza'),
    (5,  'Draft',   'Pending',  'ali.raza',     '2026-07-22', NULL,                             '2026-07-22', 'ali.raza'),
    (5,  'Pending', 'Approved', 'hamid.sheikh', '2026-07-23', 'Approved by manager.',            '2026-07-23', 'hamid.sheikh'),
    -- Q6 (Approved): 3 entries
    (6,  '', 'Draft',    'ali.raza',     '2026-07-19', NULL,                             '2026-07-19', 'ali.raza'),
    (6,  'Draft',   'Pending',  'ali.raza',     '2026-07-20', NULL,                             '2026-07-20', 'ali.raza'),
    (6,  'Pending', 'Approved', 'hamid.sheikh', '2026-07-21', 'Approved by manager.',            '2026-07-21', 'hamid.sheikh'),
    -- Q7 (Approved): 3 entries
    (7,  '', 'Draft',    'ali.raza',     '2026-07-17', NULL,                             '2026-07-17', 'ali.raza'),
    (7,  'Draft',   'Pending',  'ali.raza',     '2026-07-18', NULL,                             '2026-07-18', 'ali.raza'),
    (7,  'Pending', 'Approved', 'hamid.sheikh', '2026-07-19', 'Approved by manager.',            '2026-07-19', 'hamid.sheikh'),
    -- Q8 (Rejected): 3 entries
    (8,  '', 'Draft',    'ali.raza',     '2026-07-11', NULL,                                           '2026-07-11', 'ali.raza'),
    (8,  'Draft',   'Pending',  'ali.raza',     '2026-07-12', NULL,                                           '2026-07-12', 'ali.raza'),
    (8,  'Pending', 'Rejected', 'hamid.sheikh', '2026-07-13', 'Rejected - pricing too aggressive.',           '2026-07-13', 'hamid.sheikh'),
    -- Q9 (Rejected): 3 entries
    (9,  '', 'Draft',    'ali.raza',     '2026-07-09', NULL,                                           '2026-07-09', 'ali.raza'),
    (9,  'Draft',   'Pending',  'ali.raza',     '2026-07-10', NULL,                                           '2026-07-10', 'ali.raza'),
    (9,  'Pending', 'Rejected', 'hamid.sheikh', '2026-07-11', 'Rejected - pricing too aggressive.',           '2026-07-11', 'hamid.sheikh'),
    -- Q10 (Expired): 2 entries
    (10, '', 'Draft',   'ali.raza', '2026-07-06', NULL, '2026-07-06', 'ali.raza'),
    (10, 'Draft', 'Expired', 'ali.raza', '2026-07-07', NULL, '2026-07-07', 'ali.raza'),
    -- Q11 (Approved): 3 entries
    (11, '', 'Draft',    'ali.raza',     '2026-07-23', NULL,                             '2026-07-23', 'ali.raza'),
    (11, 'Draft',   'Pending',  'ali.raza',     '2026-07-24', NULL,                             '2026-07-24', 'ali.raza'),
    (11, 'Pending', 'Approved', 'hamid.sheikh', '2026-07-25', 'Approved by manager.',            '2026-07-25', 'hamid.sheikh'),
    -- Q12 (Draft): 1 entry
    (12, '', 'Draft', 'ali.raza', '2026-07-30', NULL, '2026-07-30', 'ali.raza'),
    -- Q13 (Pending): 2 entries
    (13, '', 'Draft',   'ali.raza', '2026-07-27', NULL, '2026-07-27', 'ali.raza'),
    (13, 'Draft', 'Pending', 'ali.raza', '2026-07-28', NULL, '2026-07-28', 'ali.raza'),
    -- Q14 (Approved): 3 entries
    (14, '', 'Draft',    'ali.raza',     '2026-07-16', NULL,                             '2026-07-16', 'ali.raza'),
    (14, 'Draft',   'Pending',  'ali.raza',     '2026-07-17', NULL,                             '2026-07-17', 'ali.raza'),
    (14, 'Pending', 'Approved', 'hamid.sheikh', '2026-07-18', 'Approved by manager.',            '2026-07-18', 'hamid.sheikh'),
    -- Q15 (Approved): 3 entries
    (15, '', 'Draft',    'ali.raza',     '2026-07-13', NULL,                             '2026-07-13', 'ali.raza'),
    (15, 'Draft',   'Pending',  'ali.raza',     '2026-07-14', NULL,                             '2026-07-14', 'ali.raza'),
    (15, 'Pending', 'Approved', 'hamid.sheikh', '2026-07-15', 'Approved by manager.',            '2026-07-15', 'hamid.sheikh'),
    -- Q16 (Rejected): 3 entries
    (16, '', 'Draft',    'ali.raza',     '2026-07-01', NULL,                                           '2026-07-01', 'ali.raza'),
    (16, 'Draft',   'Pending',  'ali.raza',     '2026-07-02', NULL,                                           '2026-07-02', 'ali.raza'),
    (16, 'Pending', 'Rejected', 'hamid.sheikh', '2026-07-03', 'Rejected - pricing too aggressive.',           '2026-07-03', 'hamid.sheikh'),
    -- Q17 (Draft): 1 entry
    (17, '', 'Draft', 'ali.raza', '2026-07-31', NULL, '2026-07-31', 'ali.raza'),
    -- Q18 (Approved): 3 entries
    (18, '', 'Draft',    'ali.raza',     '2026-07-20', NULL,                             '2026-07-20', 'ali.raza'),
    (18, 'Draft',   'Pending',  'ali.raza',     '2026-07-21', NULL,                             '2026-07-21', 'ali.raza'),
    (18, 'Pending', 'Approved', 'hamid.sheikh', '2026-07-22', 'Approved by manager.',            '2026-07-22', 'hamid.sheikh'),
    -- Q19 (Pending): 2 entries
    (19, '', 'Draft',   'ali.raza', '2026-07-29', NULL, '2026-07-29', 'ali.raza'),
    (19, 'Draft', 'Pending', 'ali.raza', '2026-07-30', NULL, '2026-07-30', 'ali.raza'),
    -- Q20 (Expired): 2 entries
    (20, '', 'Draft',   'ali.raza', '2026-06-26', NULL, '2026-06-26', 'ali.raza'),
    (20, 'Draft', 'Expired', 'ali.raza', '2026-06-27', NULL, '2026-06-27', 'ali.raza'),
    -- Q21 (Approved): 3 entries
    (21, '', 'Draft',    'ali.raza',     '2026-07-11', NULL,                             '2026-07-11', 'ali.raza'),
    (21, 'Draft',   'Pending',  'ali.raza',     '2026-07-12', NULL,                             '2026-07-12', 'ali.raza'),
    (21, 'Pending', 'Approved', 'hamid.sheikh', '2026-07-13', 'Approved by manager.',            '2026-07-13', 'hamid.sheikh'),
    -- Q22 (Draft): 1 entry
    (22, '', 'Draft', 'ali.raza', '2026-07-31', NULL, '2026-07-31', 'ali.raza'),
    -- Q23 (Approved): 3 entries
    (23, '', 'Draft',    'ali.raza',     '2026-07-15', NULL,                             '2026-07-15', 'ali.raza'),
    (23, 'Draft',   'Pending',  'ali.raza',     '2026-07-16', NULL,                             '2026-07-16', 'ali.raza'),
    (23, 'Pending', 'Approved', 'hamid.sheikh', '2026-07-17', 'Approved by manager.',            '2026-07-17', 'hamid.sheikh'),
    -- Q24 (Pending): 2 entries
    (24, '', 'Draft',   'ali.raza', '2026-07-28', NULL, '2026-07-28', 'ali.raza'),
    (24, 'Draft', 'Pending', 'ali.raza', '2026-07-29', NULL, '2026-07-29', 'ali.raza'),
    -- Q25 (Rejected): 3 entries
    (25, '', 'Draft',    'ali.raza',     '2026-07-03', NULL,                                           '2026-07-03', 'ali.raza'),
    (25, 'Draft',   'Pending',  'ali.raza',     '2026-07-04', NULL,                                           '2026-07-04', 'ali.raza'),
    (25, 'Pending', 'Rejected', 'hamid.sheikh', '2026-07-05', 'Rejected - pricing too aggressive.',           '2026-07-05', 'hamid.sheikh');
END
GO

-- ============================================================================
-- 9. DISCOUNT EVALUATIONS  (50 entries for Q1–Q20, AI/ML training data)
-- Product margin reference:
--   P1(Laptop)   margin 25.00%  normalMin 2.50   normalMax 15.00
--   P2(Desktop)  margin 27.06%  normalMin 2.71   normalMax 16.24
--   P3(Monitor)  margin 28.95%  normalMin 2.90   normalMax 17.37
--   P4(Keyboard) margin 31.76%  normalMin 3.18   normalMax 19.06
--   P5(Mouse)    margin 30.95%  normalMin 3.10   normalMax 18.57
--   P6(Hub)      margin 37.14%  normalMin 3.71   normalMax 22.28
--   P7(Webcam)   margin 29.17%  normalMin 2.92   normalMax 17.50
--   P8(Headset)  margin 31.58%  normalMin 3.16   normalMax 18.95
--   P9(Switch)   margin 28.57%  normalMin 2.86   normalMax 17.14
--   P10(HDD)     margin 30.00%  normalMin 3.00   normalMax 18.00
-- ============================================================================
IF NOT EXISTS (SELECT 1 FROM DiscountEvaluations)
BEGIN
    INSERT INTO DiscountEvaluations
        (QuotationItemId, ProductId, ProposedDiscount, NormalRangeMin, NormalRangeMax,
         AnomalyScore, IsAnomaly, Explanation,
         UserOverride, UserConfirmation, EvaluatedBy, EvaluatedAt, CreatedAt, CreatedBy)
    VALUES
    -- Q1 items (Draft)
    (1,  1,  0,  2.50, 15.00, 0.0000, 0, 'Discount 0% is within normal range (2.5%-15%).',             0, NULL, 'ali.raza', '2026-07-30', '2026-07-30', 'ali.raza'),
    (2,  4,  5,  3.18, 19.06, 0.0000, 0, 'Discount 5% is within normal range (3.18%-19.06%).',         0, NULL, 'ali.raza', '2026-07-30', '2026-07-30', 'ali.raza'),
    -- Q2 items (Draft)
    (3,  2,  0,  2.71, 16.24, 0.0000, 0, 'Discount 0% is within normal range (2.71%-16.24%).',         0, NULL, 'ali.raza', '2026-07-29', '2026-07-29', 'ali.raza'),
    (4,  5,  0,  3.10, 18.57, 0.0000, 0, 'Discount 0% is within normal range (3.1%-18.57%).',          0, NULL, 'ali.raza', '2026-07-29', '2026-07-29', 'ali.raza'),
    (5,  6,  0,  3.71, 22.28, 0.0000, 0, 'Discount 0% is within normal range (3.71%-22.28%).',         0, NULL, 'ali.raza', '2026-07-29', '2026-07-29', 'ali.raza'),
    -- Q3 items (Pending)
    (6,  1,  10, 2.50, 15.00, 0.0000, 0, 'Discount 10% is within normal range (2.5%-15%).',            0, NULL, 'ali.raza', '2026-07-26', '2026-07-26', 'ali.raza'),
    (7,  2,  10, 2.71, 16.24, 0.0000, 0, 'Discount 10% is within normal range (2.71%-16.24%).',        0, NULL, 'ali.raza', '2026-07-26', '2026-07-26', 'ali.raza'),
    (8,  3,  5,  2.90, 17.37, 0.0000, 0, 'Discount 5% is within normal range (2.9%-17.37%).',          0, NULL, 'ali.raza', '2026-07-26', '2026-07-26', 'ali.raza'),
    -- Q4 items (Pending)
    (9,  9,  5,  2.86, 17.14, 0.0000, 0, 'Discount 5% is within normal range (2.86%-17.14%).',         0, NULL, 'ali.raza', '2026-07-28', '2026-07-28', 'ali.raza'),
    (10, 10, 0,  3.00, 18.00, 0.0000, 0, 'Discount 0% is within normal range (3%-18%).',               0, NULL, 'ali.raza', '2026-07-28', '2026-07-28', 'ali.raza'),
    -- Q5 items (Approved)
    (11, 1,  15, 2.50, 15.00, 0.0000, 0, 'Discount 15% is within normal range (2.5%-15%).',            0, NULL, 'ali.raza', '2026-07-21', '2026-07-21', 'ali.raza'),
    (12, 3,  10, 2.90, 17.37, 0.0000, 0, 'Discount 10% is within normal range (2.9%-17.37%).',         0, NULL, 'ali.raza', '2026-07-21', '2026-07-21', 'ali.raza'),
    (13, 7,  0,  2.92, 17.50, 0.0000, 0, 'Discount 0% is within normal range (2.92%-17.5%).',          0, NULL, 'ali.raza', '2026-07-21', '2026-07-21', 'ali.raza'),
    -- Q6 items (Approved)
    (14, 2,  10, 2.71, 16.24, 0.0000, 0, 'Discount 10% is within normal range (2.71%-16.24%).',        0, NULL, 'ali.raza', '2026-07-19', '2026-07-19', 'ali.raza'),
    (15, 4,  5,  3.18, 19.06, 0.0000, 0, 'Discount 5% is within normal range (3.18%-19.06%).',         0, NULL, 'ali.raza', '2026-07-19', '2026-07-19', 'ali.raza'),
    -- Q7 items (Approved — QI16 is ANOMALY)
    (16, 1,  20, 2.50, 15.00, 0.3333, 1, 'Proposed discount 20% exceeds normal max 15% based on product margin 25%.', 1, 1, 'ali.raza', '2026-07-17', '2026-07-17', 'ali.raza'),
    (17, 2,  15, 2.71, 16.24, 0.0000, 0, 'Discount 15% is within normal range (2.71%-16.24%).',        0, NULL, 'ali.raza', '2026-07-17', '2026-07-17', 'ali.raza'),
    (18, 3,  10, 2.90, 17.37, 0.0000, 0, 'Discount 10% is within normal range (2.9%-17.37%).',         0, NULL, 'ali.raza', '2026-07-17', '2026-07-17', 'ali.raza'),
    -- Q8 items (Rejected — both ANOMALY)
    (19, 1,  30, 2.50, 15.00, 1.0000, 1, 'Proposed discount 30% exceeds normal max 15% based on product margin 25%.', 1, 0, 'ali.raza', '2026-07-11', '2026-07-11', 'ali.raza'),
    (20, 5,  25, 3.10, 18.57, 0.3463, 1, 'Proposed discount 25% exceeds normal max 18.57% based on product margin 30.95%.', 1, 0, 'ali.raza', '2026-07-11', '2026-07-11', 'ali.raza'),
    -- Q9 items (Rejected — QI21 is ANOMALY)
    (21, 9,  20, 2.86, 17.14, 0.1669, 1, 'Proposed discount 20% exceeds normal max 17.14% based on product margin 28.57%.', 1, 0, 'ali.raza', '2026-07-09', '2026-07-09', 'ali.raza'),
    (22, 10, 15, 3.00, 18.00, 0.0000, 0, 'Discount 15% is within normal range (3%-18%).',               0, NULL, 'ali.raza', '2026-07-09', '2026-07-09', 'ali.raza'),
    -- Q10 items (Expired)
    (23, 3,  5,  2.90, 17.37, 0.0000, 0, 'Discount 5% is within normal range (2.9%-17.37%).',          0, NULL, 'ali.raza', '2026-07-06', '2026-07-06', 'ali.raza'),
    (24, 6,  0,  3.71, 22.28, 0.0000, 0, 'Discount 0% is within normal range (3.71%-22.28%).',         0, NULL, 'ali.raza', '2026-07-06', '2026-07-06', 'ali.raza'),
    (25, 7,  0,  2.92, 17.50, 0.0000, 0, 'Discount 0% is within normal range (2.92%-17.5%).',          0, NULL, 'ali.raza', '2026-07-06', '2026-07-06', 'ali.raza'),
    -- Q11 items (Approved)
    (26, 4,  8,  3.18, 19.06, 0.0000, 0, 'Discount 8% is within normal range (3.18%-19.06%).',         0, NULL, 'ali.raza', '2026-07-23', '2026-07-23', 'ali.raza'),
    (27, 5,  8,  3.10, 18.57, 0.0000, 0, 'Discount 8% is within normal range (3.1%-18.57%).',          0, NULL, 'ali.raza', '2026-07-23', '2026-07-23', 'ali.raza'),
    (28, 6,  8,  3.71, 22.28, 0.0000, 0, 'Discount 8% is within normal range (3.71%-22.28%).',         0, NULL, 'ali.raza', '2026-07-23', '2026-07-23', 'ali.raza'),
    -- Q12 items (Draft)
    (29, 8,  0,  3.16, 18.95, 0.0000, 0, 'Discount 0% is within normal range (3.16%-18.95%).',         0, NULL, 'ali.raza', '2026-07-30', '2026-07-30', 'ali.raza'),
    (30, 9,  0,  2.86, 17.14, 0.0000, 0, 'Discount 0% is within normal range (2.86%-17.14%).',         0, NULL, 'ali.raza', '2026-07-30', '2026-07-30', 'ali.raza'),
    -- Q13 items (Pending)
    (31, 1,  12, 2.50, 15.00, 0.0000, 0, 'Discount 12% is within normal range (2.5%-15%).',            0, NULL, 'ali.raza', '2026-07-27', '2026-07-27', 'ali.raza'),
    (32, 2,  12, 2.71, 16.24, 0.0000, 0, 'Discount 12% is within normal range (2.71%-16.24%).',        0, NULL, 'ali.raza', '2026-07-27', '2026-07-27', 'ali.raza'),
    (33, 10, 0,  3.00, 18.00, 0.0000, 0, 'Discount 0% is within normal range (3%-18%).',               0, NULL, 'ali.raza', '2026-07-27', '2026-07-27', 'ali.raza'),
    -- Q14 items (Approved)
    (34, 7,  10, 2.92, 17.50, 0.0000, 0, 'Discount 10% is within normal range (2.92%-17.5%).',         0, NULL, 'ali.raza', '2026-07-16', '2026-07-16', 'ali.raza'),
    (35, 8,  10, 3.16, 18.95, 0.0000, 0, 'Discount 10% is within normal range (3.16%-18.95%).',        0, NULL, 'ali.raza', '2026-07-16', '2026-07-16', 'ali.raza'),
    -- Q15 items (Approved)
    (36, 9,  0,  2.86, 17.14, 0.0000, 0, 'Discount 0% is within normal range (2.86%-17.14%).',         0, NULL, 'ali.raza', '2026-07-13', '2026-07-13', 'ali.raza'),
    (37, 3,  5,  2.90, 17.37, 0.0000, 0, 'Discount 5% is within normal range (2.9%-17.37%).',          0, NULL, 'ali.raza', '2026-07-13', '2026-07-13', 'ali.raza'),
    (38, 4,  5,  3.18, 19.06, 0.0000, 0, 'Discount 5% is within normal range (3.18%-19.06%).',         0, NULL, 'ali.raza', '2026-07-13', '2026-07-13', 'ali.raza'),
    -- Q16 items (Rejected — both ANOMALY)
    (39, 1,  25, 2.50, 15.00, 0.6667, 1, 'Proposed discount 25% exceeds normal max 15% based on product margin 25%.', 1, 0, 'ali.raza', '2026-07-01', '2026-07-01', 'ali.raza'),
    (40, 2,  20, 2.71, 16.24, 0.2315, 1, 'Proposed discount 20% exceeds normal max 16.24% based on product margin 27.06%.', 1, 0, 'ali.raza', '2026-07-01', '2026-07-01', 'ali.raza'),
    -- Q17 items (Draft)
    (41, 5,  0,  3.10, 18.57, 0.0000, 0, 'Discount 0% is within normal range (3.1%-18.57%).',          0, NULL, 'ali.raza', '2026-07-31', '2026-07-31', 'ali.raza'),
    (42, 6,  0,  3.71, 22.28, 0.0000, 0, 'Discount 0% is within normal range (3.71%-22.28%).',         0, NULL, 'ali.raza', '2026-07-31', '2026-07-31', 'ali.raza'),
    (43, 7,  0,  2.92, 17.50, 0.0000, 0, 'Discount 0% is within normal range (2.92%-17.5%).',          0, NULL, 'ali.raza', '2026-07-31', '2026-07-31', 'ali.raza'),
    -- Q18 items (Approved)
    (44, 1,  10, 2.50, 15.00, 0.0000, 0, 'Discount 10% is within normal range (2.5%-15%).',            0, NULL, 'ali.raza', '2026-07-20', '2026-07-20', 'ali.raza'),
    (45, 4,  10, 3.18, 19.06, 0.0000, 0, 'Discount 10% is within normal range (3.18%-19.06%).',        0, NULL, 'ali.raza', '2026-07-20', '2026-07-20', 'ali.raza'),
    (46, 10, 5,  3.00, 18.00, 0.0000, 0, 'Discount 5% is within normal range (3%-18%).',               0, NULL, 'ali.raza', '2026-07-20', '2026-07-20', 'ali.raza'),
    -- Q19 items (Pending)
    (47, 2,  15, 2.71, 16.24, 0.0000, 0, 'Discount 15% is within normal range (2.71%-16.24%).',        0, NULL, 'ali.raza', '2026-07-29', '2026-07-29', 'ali.raza'),
    (48, 9,  10, 2.86, 17.14, 0.0000, 0, 'Discount 10% is within normal range (2.86%-17.14%).',        0, NULL, 'ali.raza', '2026-07-29', '2026-07-29', 'ali.raza'),
    -- Q20 items (Expired)
    (49, 3,  0,  2.90, 17.37, 0.0000, 0, 'Discount 0% is within normal range (2.9%-17.37%).',          0, NULL, 'ali.raza', '2026-06-26', '2026-06-26', 'ali.raza'),
    (50, 8,  0,  3.16, 18.95, 0.0000, 0, 'Discount 0% is within normal range (3.16%-18.95%).',         0, NULL, 'ali.raza', '2026-06-26', '2026-06-26', 'ali.raza');
END
GO

-- ============================================================================
-- DONE. Verify row counts:
-- ============================================================================
SELECT 'Users'               AS TableName, COUNT(*) AS Rows FROM Users
UNION ALL SELECT 'Customers',          COUNT(*) FROM Customers
UNION ALL SELECT 'Products',           COUNT(*) FROM Products
UNION ALL SELECT 'Discounts',          COUNT(*) FROM Discounts
UNION ALL SELECT 'Quotations',         COUNT(*) FROM Quotations
UNION ALL SELECT 'QuotationItems',     COUNT(*) FROM QuotationItems
UNION ALL SELECT 'Approvals',          COUNT(*) FROM Approvals
UNION ALL SELECT 'StatusHistories',    COUNT(*) FROM StatusHistories
UNION ALL SELECT 'DiscountEvaluations',COUNT(*) FROM DiscountEvaluations;
GO
