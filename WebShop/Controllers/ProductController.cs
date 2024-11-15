using Microsoft.AspNetCore.Mvc;
using WebShop.UnitOfWork;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        public ProductController()
        {
        }

        // Endpoint för att hämta alla produkter
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            // Beh�ver anv�nda repository via Unit of Work för att hämta produkter
            return Ok();
        }

        // Endpoint f�r att l�gga till en ny produkt
        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            // L�gger till produkten via repository

            // Sparar f�r�ndringar

            // Notifierar observat�rer om att en ny produkt har lagts till

            return Ok();
        }
    }
}
