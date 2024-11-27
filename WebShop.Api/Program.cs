using Microsoft.EntityFrameworkCore;
using WebShop.Application.Interfaces;
using WebShop.Application.Subjects;
using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;
using WebShop.Infrastructure;
using WebShop.Infrastructure.DataAccess;
using WebShop.Infrastructure.Interfaces;
using WebShop.Infrastructure.Observers;
using WebShop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<ISubjectFactory, SubjectFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(ISubjectManager), typeof(SubjectManager));
// builder.Services.AddTransient<INotificationObserver<Product>, EmailNotificationObserver>();
builder.Services.AddSingleton<ISubject<Product>, ProductSubject>();

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
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
