﻿using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;

namespace WebShop.Infrastructure.Observers;
public class SmsNotificationObserver : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"SMS Notification: New product added - {entity.Name}");
    }
}