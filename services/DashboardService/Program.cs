using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using DashboardService.Data;
using DashboardService.Features;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
if (!string.IsNullOrWhiteSpace(keyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? builder.Configuration["ConnectionStrings:Postgres"]
    ?? builder.Configuration["ConnectionStrings--Postgres"]
    ?? throw new InvalidOperationException("PostgreSQL connection string is missing.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenCors", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("OpenCors");
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", service = "DashboardService" }));
app.Run();
