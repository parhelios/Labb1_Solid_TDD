using WebShop.Repositories;

namespace WebShop.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
         // Repository för produkter
         // Sparar förändringar (om du använder en databas)
         
         //TODO: Se över 
         Task CommitAsync();
         IProductRepository Products { get; }
            
         void NotifyProductAdded(Product product); // Notifierar observatörer om ny produkt
    }
}

