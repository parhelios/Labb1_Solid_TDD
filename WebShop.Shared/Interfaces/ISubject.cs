﻿namespace WebShop.Shared.Interfaces;

public interface ISubject<TEntity> where TEntity : IEntity
{
    void Attach(INotificationObserver<TEntity> observer);
    void Detach(INotificationObserver<TEntity> observer);
    void Notify(TEntity entity);
}