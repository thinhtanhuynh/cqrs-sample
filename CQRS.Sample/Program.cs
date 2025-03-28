using Scalar.AspNetCore;
using CQRS.Sample.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add DbContext
builder.Services.AddDbContext<OrderWriteDbContext>(options =>
   options.UseMySQL(builder.Configuration.GetConnectionString("OrderDbWrite")!));

builder.Services.AddDbContext<OrderReadDbContext>(options =>
   options.UseMySQL(builder.Configuration.GetConnectionString("OrderDbRead")!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.MapOpenApi();
   app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();