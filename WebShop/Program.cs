using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;
using WebShop.Shared.Observer;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// Registrera Unit of Work i DI-container
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); //TODO: Ev ta bort.
builder.Services.AddTransient<INotificationObserver<Product>, EmailNotification>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(
    options => options.UseSqlServer(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
