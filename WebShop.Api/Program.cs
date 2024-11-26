using Microsoft.EntityFrameworkCore;
using WebShop.Application.Interfaces;
using WebShop.DataAccess;
using WebShop.Domain.Entities;
using WebShop.Infrastructure.DataAccess;
using WebShop.Infrastructure.Factory;
using WebShop.Infrastructure.Interfaces;
using WebShop.Infrastructure.Observer;
using WebShop.Infrastructure.Observer.Notification;
using WebShop.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//TODO: Rensa upp överflödiga services

// Registrera Unit of Work i DI-container
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<ISubjectFactory, SubjectFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddScoped(typeof(ISubject<>), typeof(Subject<>));
builder.Services.AddScoped(typeof(ISubjectManager), typeof(SubjectManager));
// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 
builder.Services.AddTransient<INotificationObserver<Product>, EmailNotificationObserver>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(
    options => options.UseSqlServer(connectionString)
);

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    dbContext.Database.Migrate();
}

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
