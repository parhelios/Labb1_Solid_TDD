﻿using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;

namespace WebShop.Application.Observers;
public class PushNotificationObserver : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"Push Notification: New product added - {entity.Name}");
    }
}