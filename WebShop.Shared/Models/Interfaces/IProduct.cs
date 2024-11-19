﻿namespace WebShop.Shared.Models.Interfaces;

public interface IProduct : IEntity
{
    string Name { get; set; }
    double Price { get; set; }
    int Amount { get; set; }
}