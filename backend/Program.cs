using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT Key is missing in appsettings.json");

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
        };
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine("RECEIVED TOKEN STRING: '" + context.Token + "'");
                Console.WriteLine("AUTH HEADER: '" + context.Request.Headers["Authorization"] + "'");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT Auth Failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        bool canConnect = dbContext.Database.CanConnect();
        if (canConnect)
        {
            Console.WriteLine("=== Database Connection Successful! ===");
        }
        else
        {
            Console.WriteLine("=== Could not connect to the database. ===");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"=== Database Connection Error: {ex.Message} ===");
    }
}
// --- CONNECTION TEST END ---
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    if (!db.Users.Any())
    {
        var dummyUsers = new List<User>
        {
            new User
            { 
                Username = "dawood", 
                Email = "dawood@admin.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), 
                Role = "Manager",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new User 
            { 
                Username = "sales_ali", 
                Email = "ali@sales.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Sales@123"), 
                Role = "SalesRep",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        db.Users.AddRange(dummyUsers);
        db.SaveChanges();
        Console.WriteLine("Dummy Users Seeded Successfully!");
    }
}
app.Run();

