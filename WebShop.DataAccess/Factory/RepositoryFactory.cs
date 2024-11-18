﻿using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;

namespace WebShop.DataAccess.Factory;

public class RepositoryFactory(MyDbContext context) : IRepositoryFactory
{
    public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
    {
        if (typeof(TEntity) == typeof(Product))
            return (IRepository<TEntity>) new ProductRepository(context);

        return new Repository<TEntity>(context);
    }
}