using LibraryShop.Data;
using LibraryShop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. REGISTER SERVICES (Dependency Injection container)
// ============================================================

// Add controller support
builder.Services.AddControllers();

// Add OpenAPI / Swagger (for the built-in API docs UI)
builder.Services.AddOpenApi();

// Register EF Core with SQL Server.
// The connection string is read from appsettings.json -> "ConnectionStrings" -> "Default"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Register our custom services using Scoped lifetime.
// Scoped = one instance per HTTP request. This is the standard for database services.
// AddTransient = new instance every time it's requested (for lightweight, stateless services)
// AddSingleton = one instance for the entire app lifetime (for caches, config, etc.)
builder.Services.AddScoped<IBookService,     BookService>();
builder.Services.AddScoped<IOrderService,    OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// ============================================================
// 2. BUILD THE APP
// ============================================================
var app = builder.Build();

// ============================================================
// 3. CONFIGURE THE HTTP PIPELINE (middleware)
// ============================================================

// Show the Swagger UI at / in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/openapi/v1.json", "LibraryShop API v1");
        opt.RoutePrefix = string.Empty; // serve Swagger UI at http://localhost:PORT/
    });
}

app.UseHttpsRedirection();

// Map all controllers (reads [Route] attributes on controller classes)
app.MapControllers();

app.Run();
